using JobPortal_New.Dto;
using JobPortal_New.Modeles.Entites;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal_New.Interfaces.Repositories
{
    public interface IUserRepository
    {


        Task<IActionResult> AddUser(UserDto model);

        Task<IActionResult> AddAdmin(UserDto model);
        Task<IActionResult> DeleteUser(int userId);

        Task<IActionResult> UpdateUser(int userId, UserDto model);

        Task<IActionResult> ChangePassword(string oldPassword, string newPassword, int userId);

        Task<IActionResult> Login(UserLoginDto user);
        Task<String> ExportAllCandidates();
        Task<String> ExportAllRecruiters();
        Task<List<User>> getAllCandidates();

        Task<IActionResult> ForgotPassword(string email);

        Task<IActionResult> ResetPassword(OtpDto model, string newPassword);
        Task<IActionResult> ActivateUser(int userId);

        Task<IActionResult> DeactivateUser(int userId);
    }
}
