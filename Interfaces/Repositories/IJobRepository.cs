using JobPortal_New.Dto;
using JobPortal_New.Modeles.Entites;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal_New.Interfaces.Repositories
{
    public interface IJobRepository
    {
        Task<IActionResult> AddJob(JobDto model);
        Task<IActionResult> DeactivateJob(int jobId, int userId);

        Task<List<Job>> DeactivateJobsByUserId(int userId);

        Task<String> ExportAllJobs();
    }
}
