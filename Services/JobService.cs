using JobPortal_New.Data;
using JobPortal_New.Dto;
using JobPortal_New.HelperMethods;
using JobPortal_New.Interfaces.Repositories;
using JobPortal_New.Modeles.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortal_New.Services
{
    public class JobService : IJobRepository
    {

        private readonly MyDbContext _context;
       
        public JobService(MyDbContext context)
        {
            _context = context;
           
        }



        public async Task<IActionResult> AddJob(JobDto model)
        {
            try
            {
                var job = new Job();
                
                job.JobTitle = model.JobTitle;
                job.JobDescription = model.JobDescription;  
                job.JobLocation = model.JobLocation;
                job.PostedDate = DateTime.Now;
                //job.User = _context.Users.FirstOrDefault(r => r.UserId == model.UserId);
                job.UserId = model.UserId;
                job.IsActive = true;
                await _context.Jobs.AddAsync(job);
                await _context.SaveChangesAsync();
                return new OkObjectResult(new { message = "Job added Successfully" });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "An error occurred while adding Job", error = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }

        public async Task<IActionResult> DeactivateJob(int jobId, int userId)
        {
            try
            {
                var job = new Job();
                job = _context.Jobs.Where(j => j.JobId == jobId).FirstOrDefault();
                if (job == null)
                {
                    return new ObjectResult(new { message = "No Such Job Exist" });
                }
                if (job.UserId != userId)
                {
                    return new ObjectResult(new { message = "This Job Is Not Added By You" });
                }

                job.IsActive = false;
                await _context.SaveChangesAsync();
                return new OkObjectResult(new { message = "Job Deactivated Successfully" });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "An error occurred while Deactivating Job", error = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }

        //public List<JobDto> getJobsById(int userId)
        //{
        //    var result = _context.Jobs.Where(j => j.UserId == userId).ToList();

        //    foreach (var item in result)
        //    {

        //    }
        //}

        public async Task<List<Job>> DeactivateJobsByUserId(int userId)
        {
            
                var jobList =   _context.Jobs
                                       .Where(u => u.UserId == userId)
                                       .ToList();

            if (jobList.Count == 0)
            {
                throw new Exception("No Jobs added by user");
            }

            foreach (var job in jobList)
            {
                job.IsActive = false;
                _context.SaveChanges();
            }

            return jobList;
        }


        public async Task<String> ExportAllJobs()
        {
            var jobs = await _context.Jobs
                                           .Select(u => new JobDto
                                           {
                                               JobTitle = u.JobTitle,
                                               JobLocation = u.JobLocation,
                                               JobDescription = u.JobDescription,
                                               PostedDate = u.PostedDate,
                                               UserId = u.UserId,

                                           })
                                           .ToListAsync();

            string filePath = ExcelFiles.ExportToExcel(jobs, "Jobs");
            return filePath;
        }
    }
}
