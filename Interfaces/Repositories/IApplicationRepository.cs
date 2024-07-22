using JobPortal_New.Dto;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal_New.Interfaces.Repositories
{
    public interface IApplicationRepository
    {
        Task<IActionResult> ApplyJob(JobApplicationDto model);
        Task<IActionResult> GetApplicationsByUserId(int userId, int pageNumber, int pageSize);
        Task<IActionResult> GetApplicationsByJobId(int userId, int pageNumber, int pageSize);

        Task<String> ExportAllApplications();
    }
}
