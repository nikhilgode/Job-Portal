using JobPortal_New.Dto;
using JobPortal_New.Interfaces.Repositories;
using JobPortal_New.Modeles.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal_New.Controllers
{
    public class ApplicationController : ControllerBase
    {

        private readonly IApplicationRepository _iApplication;
        public ApplicationController(IApplicationRepository iAppliction)
        {
            _iApplication = iAppliction;

        }

        [HttpPost("applyJob")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> addUser(JobApplicationDto model)
        {
            return await _iApplication.ApplyJob(model);
        }

        [HttpGet("getJobByUserId")]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<ActionResult> getApplicationsByUserId(int userId, int pageNumber = 1, int pageSize = 10)
        {
            var jobApplications = await _iApplication.GetApplicationsByUserId(userId, pageNumber, pageSize);
            return Ok(jobApplications);

        }

        [HttpGet("getJobByJobId")]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<ActionResult> getApplicationsByJobId(int userId, int pageNumber = 1, int pageSize = 10)
        {
            var jobApplications = await _iApplication.GetApplicationsByUserId(userId, pageNumber, pageSize);
            return Ok(jobApplications);

        }

        [HttpGet("exportAll-Applications")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportAllRecruiters()
        {
            string filePath = await _iApplication.ExportAllApplications();
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AllRecruiters.xlsx");
        }
    }
}
