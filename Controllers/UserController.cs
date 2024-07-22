using JobPortal_New.Interfaces.Repositories;
using JobPortal_New.Modeles.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using JobPortal_New.Dto;
using Microsoft.AspNetCore.Authorization;
using JobPortal_New.Repositories;
using JobPortal_New.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace JobPortal_New.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly IUserRepository _iUser;
        private readonly ITokenRepository _iToken;
        private readonly MyDbContext _context;
        public UserController(IUserRepository iUser, ITokenRepository iToken,MyDbContext context)
        {
            _iUser = iUser;
            _iToken = iToken;
            _context = context;
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> addUser(UserDto model)
        {
            return await _iUser.AddUser(model);
        }

        [HttpPost("registerAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> addAdmin(UserDto model)
        {
            model.role = 3;
            return await _iUser.AddAdmin(model);
        }

        [HttpDelete("deleteUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> deleteUser(int userid)
        {

            return await _iUser.DeleteUser(userid);
        }


        [HttpPost("updateUser")]
        [Authorize]
        public async Task<IActionResult> updateUser(int userid,UserDto user)
        {
            return await _iUser.UpdateUser(userid,user);
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> changePassword(string oldPassword, string newPassword, int userId)
        {

            return await _iUser.ChangePassword(oldPassword, newPassword,userId);
        }

        [HttpGet("exportAll-candidates")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportAllCandidates()
        {
            string filePath = await _iUser.ExportAllCandidates();
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AllCandidates.xlsx");
        }


        [HttpGet("exportAll-Recruiters")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ExportAllRecruiters()
        {
            string filePath = await _iUser.ExportAllCandidates();
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AllRecruiters.xlsx");
        }



        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is missing." });
            }

            _iToken.InvalidateToken(token);
            return Ok(new { message = "Successfully logged out." });
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> forgotPassword(string email)
        {
            return await _iUser.ForgotPassword(email);
        }


        [HttpPost("resetPassword")]
         public async Task<IActionResult> ResetPassword(OtpDto model, string newPassword)
         {
            return await _iUser.ResetPassword(model, newPassword);
         }

        [HttpPost("activateUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> activateUser(int userId)
        {
            return await _iUser.ActivateUser(userId);
        }

        [HttpPost("deactivateUser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> deactivateUser(int userId)
        {
            return await _iUser.DeactivateUser(userId);
        }

        [HttpGet("getAllCandidates")]
        public async  Task<List<User>> getAllCandidates()
        {
            var users =  await _iUser.getAllCandidates();
            return users;
        }

        //Views
        [HttpGet("getallUserView")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _iUser.getAllCandidates();
            return View(users);
        }

        [HttpGet("getOption")]
        public async Task<IActionResult> chooseOptions()
        {
            return View();
        }



        [HttpGet("ExportAllRecruiters1")]
        public async Task<IActionResult> ExportAllRecruiters1()
        {
            var user = _context.Users.Include(u => u.roles).Where(u => u.IsActive == true).ToList();
            return View(user);
        }

    }
}
