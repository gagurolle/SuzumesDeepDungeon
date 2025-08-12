using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SuzumesDeepDungeon.Data;
using SuzumesDeepDungeon.DTO;
using SuzumesDeepDungeon.MockData;
using SuzumesDeepDungeon.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SuzumesDeepDungeon.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DatabaseContext _context;

        public AuthController(IConfiguration config, DatabaseContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Username and password are required");
            }
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username);
           
            if (user == null)
            {

                return Unauthorized("Invalid username or password");
            }

            //ToDo: использовать хеширование
            if (user.Password != model.Password)
            {
                return Unauthorized("Invalid username or password");
            }
            

            var token = GenerateJwtToken(user.Username, user.Role == UserRole.Admin);

            return Ok(new
            {
                token,
                username = user.Username,
                role = user.Role.ToString(),
                isAdmin = user.Role == UserRole.Admin
            });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Unauthorized("Invalid username or password");
        }

        private string GenerateJwtToken(string username, bool isAdmin)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    
}
