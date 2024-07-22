using JobPortal_New.Interfaces.Repositories;
using JobPortal_New.Modeles.Entites;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal_New.Controllers
{
    public class JobViewController : Controller
    {
        private readonly IJobRepository _iJob;
        public JobViewController(IJobRepository iJob)
        {
            _iJob = iJob;

        }

        [HttpGet("getallDe-activateUser")]
        public async Task<IActionResult> DeactivateUserMvc(int userId)
        {
            var jobs = await _iJob.DeactivateJobsByUserId(userId);
            return View(jobs);
        }
    }
}
