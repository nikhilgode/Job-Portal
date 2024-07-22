using JobPortal_New.Dto;
using JobPortal_New.Interfaces.Repositories;
using JobPortal_New.Modeles.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal_New.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobRepository _iJobs;
        public JobController(IJobRepository iJobs)
        {
            _iJobs = iJobs;
        }

        [HttpPost]
        [Route ("AddJob")]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<IActionResult> addJob(JobDto model)
        {
            return await _iJobs.AddJob(model);
        }


        [HttpPost]
        [Route("DeactivateJob")]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<IActionResult> deactivateJob(int jobId, int userId)
        {
            return await _iJobs.DeactivateJob(jobId, userId);
        }


        [HttpPost]
        [Route("DeactivateJobByUserId")]
        [Authorize(Roles = "Admin")]
        public async Task<List<Job>> deactivateJobsByUserId(int userId)
        {
            return await _iJobs.DeactivateJobsByUserId(userId);
        }


        [HttpPost]
        [Route("exportAllJobs")]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<ActionResult> exportAllJobs()
        {
            string filePath = await _iJobs.ExportAllJobs();
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AllRecruiters.xlsx");
        }


        //[HttpPost("addjob")]
        //public async Task<IActionResult> addJob(int jobId, int userId)
        //{
        //    return await _iJobs.DeactivateJob(jobId,userId);
        //}

    }
}
