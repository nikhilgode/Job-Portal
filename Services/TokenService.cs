using JobPortal_New.Data;
using JobPortal_New.Interfaces.Repositories;
using JobPortal_New.Modeles.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobPortal_New.Services
{
    public class TokenService : ITokenRepository
    {

        //private readonly JwtSettings _jwtSettings;
        //private readonly MyDbContext _context;

        //public TokenService(IOptions<JwtSettings> jwtSettings,MyDbContext context)
        //{
        //    _jwtSettings = jwtSettings.Value;
        //    _context = context;
        //}

        private IConfiguration _config;
        private readonly MyDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenService(IConfiguration configuration, MyDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _config = configuration;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }



        //public string GenerateToken(User user)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //        new Claim(ClaimTypes.Name, user.UserId.ToString()),
        //        new Claim(ClaimTypes.Email, user.UserEmail),
        //        new Claim(ClaimTypes.Role, user.roles.ToString()) // Including role in the token
        //        }),
        //        Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}

        public string GenerateToken(User users)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Sub, users.UserName),
            new Claim(JwtRegisteredClaimNames.Email, users.UserEmail),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.NameId, users.UserId.ToString())
            };


            var userRoles = _context.Roles.Where(u => u.RoleId == users.roles.RoleId).ToList();
            var roleIds = userRoles.Select(u => u.RoleId).ToList();
            var roles = _context.Roles.Where(r => roleIds.Contains(r.RoleId)).ToList();
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
            }

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            return tokenHandler.WriteToken(token);
        }


        //public void InvalidateToken(string token)
        //{
        //    var blacklistedToken = new BlacklistedToken();

        //    blacklistedToken.Token = token;
        //    blacklistedToken.BlacklistedAt = DateTime.UtcNow;

        //    _context.BlacklistedTokens.Add(blacklistedToken);
        //    _context.SaveChanges();
        //}

        public void InvalidateToken(string token)
        {
            var blacklistedToken = new BlacklistedToken
            {
                Token = token,
                BlacklistedAt = DateTime.UtcNow
            };
            _context.BlacklistedTokens.Add(blacklistedToken);
            _context.SaveChanges();
        }

        public bool IsTokenValid(string token)
        {
            return !_context.BlacklistedTokens.Any(t => t.Token == token);
        }

    }
}
