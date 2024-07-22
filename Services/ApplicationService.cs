using JobPortal_New.Data;
using JobPortal_New.Dto;
using JobPortal_New.HelperMethods;
using JobPortal_New.Interfaces.Repositories;
using JobPortal_New.Modeles.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JobPortal_New.Services
{
    public class ApplicationService : IApplicationRepository
    {

        private readonly MyDbContext _context;
        private readonly IEmailRepository _emailService;
        public ApplicationService(MyDbContext context, IEmailRepository emailService)
        {
            _context = context;
            _emailService = emailService;
        }



        public async Task<IActionResult> ApplyJob(JobApplicationDto model)
        {
            try
            {
                var jobApplication = new Application();
                var getJob = _context.Jobs.Where(j => j.JobId == model.JobId).FirstOrDefault();
                if (getJob == null)
                {
                    return new ObjectResult(new { message = "No Such Job Exist" });
                }
                if (getJob.IsActive)
                {
                    jobApplication.AppliedDate = DateTime.Now;
                    jobApplication.Job = getJob;
                    jobApplication.User = _context.Users.Where(u => u.UserId == model.UserId).FirstOrDefault();
                }
                else
                {
                    return new ObjectResult(new { message = "This Job is no more accepting applications" });
                }
                await _context.Applicationes.AddAsync(jobApplication);
                await _context.SaveChangesAsync();

                var userEmail = jobApplication.User.UserEmail; // Assuming you have an Email property in the User entity
                var subject = "Job Application Confirmation";
                var message = $"Dear {jobApplication.User.UserName},\n\nYour application is successfully submitted for the position of {getJob.JobTitle}.\n\nBest regards,\nYour Application Team";

                await _emailService.SendEmailAsync(userEmail, subject, message);

                return new OkObjectResult(new { message = "Applied  Successfully...!" });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "An error occurred while Applying for job", error = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }



        // public async Task<List<JobDto> > GetApplicationsByUserId(int userId,int pageNumber,int pageSize)
        public async Task<IActionResult> GetApplicationsByUserId(int userId, int pageNumber, int pageSize)
        {
            try
            {
                var applicationsByUserId = _context.Applicationes
                                            .Where(app => app.UserId == userId)
                                            .Include(app => app.User)
                                            .Include(app => app.Job)
                                            .OrderByDescending(app => app.AppliedDate)
                                            .AsQueryable();

                int totalCount = applicationsByUserId.Count();
                var applications = await applicationsByUserId.Skip((pageNumber - 1) * pageSize)
                                                             .Take(pageSize)
                                                             .ToListAsync();
                var result = applications.Select(app => new JobDto
                {
                    UserId = app.UserId,
                    JobTitle = app.Job.JobTitle,
                    JobDescription = app.Job.JobDescription,
                    JobLocation = app.Job.JobLocation,
                }).ToList();

                //   return (result);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ObjectResult(new { message = "An error occurred...!", error = ex.Message })
                {
                    StatusCode = 500
                };
            }

        }

        public async Task<IActionResult> GetApplicationsByJobId(int jobId, int pageNumber, int pageSize)
        {
            try
            {
                var applicationsByJobId = _context.Applicationes
                                              .Where(app => app.JobId == jobId)
                                              .Include(app => app.User)
                                              .Include(app => app.Job)
                                              .OrderByDescending(app => app.AppliedDate)
                                              .AsQueryable();

                int totalCount = applicationsByJobId.Count();
                var applications = await applicationsByJobId.Skip((pageNumber - 1) * pageSize)
                                                          .Take(pageSize)
                                                          .ToListAsync();

                var result = applications.Select(app => new JobDto
                {
                    UserId = app.UserId,
                    JobTitle = app.Job.JobTitle,
                    JobDescription = app.Job.JobDescription,
                    JobLocation = app.Job.JobLocation,
                }).ToList();

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ObjectResult(new { message = "An error occurred...!", error = ex.Message })
                {
                    StatusCode = 500
                };
            }

        }

        public async Task<String> ExportAllApplications()
        {
            var applications = await _context.Applicationes
                                     .Select(u => new ExportAppliCationDto
                                     {
                                          applicant = u.User.UserName,
                                          jobName = u.Job.JobTitle,
                                          appliedDate = u.AppliedDate

                                     })
                                     .ToListAsync();

            string filePath = ExcelFiles.ExportToExcel(applications, "Applications");
            return filePath;
        }

    }
}
