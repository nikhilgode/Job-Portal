using JobPortal_New.Data;
using JobPortal_New.Interfaces.Repositories;
using JobPortal_New.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobPortal_New.Dto;
using JobPortal_New.Modeles.Entites;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal_New.Controllers
{
    public class AuthController : ControllerBase
    {

        private readonly MyDbContext _context;
        private readonly ITokenRepository _tokenService;
        private readonly IUserRepository _userRepository;

        public AuthController(MyDbContext context, ITokenRepository tokenService, IUserRepository userRepository)
        {
            _context = context;
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

    //    private readonly IUserRepository _iUser;
        //public UserController(IUserRepository iUser)
        //{
        //    _iUser = iUser;

        //}

        //[HttpPost("login")]
        //public async Task<IActionResult> Login(UserLoginDto userDto)
        //{
        //    var user = await _context.Users.SingleOrDefaultAsync(u => u.UserEmail == userDto.UserEmail);



        //    if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
        //    {
        //        return Unauthorized(new { message = "Invalid email or password" });
        //    }

        //    var token = _tokenService.GenerateToken(user);

        //    return Ok(new { token });
        //}


        [HttpPost("login")]
        public async Task<IActionResult> login(UserLoginDto userDto)
        {

            return await _userRepository.Login(userDto);
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

            _tokenService.InvalidateToken(token);
            return Ok(new { message = "Successfully logged out." });
        }
    }
}
