using JobPortal_New.Dto;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal_New.Interfaces.Repositories
{
    public interface IOtpRepository
    {
        Task<IActionResult> AddOtp(OtpDto model);

        Task<IActionResult> DeleteOtp(OtpDto model);
    }
}
