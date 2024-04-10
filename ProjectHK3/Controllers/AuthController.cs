using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectHK3.DTOs;
using ProjectHK3.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectHK3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ProjectHk3Context _context;

        public AuthController(IConfiguration config, ProjectHk3Context context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            // Authenticate user (replace with your own authentication logic)
            var user = _context.TaiKhoanMatKhaus.SingleOrDefault(tk => tk.TaiKhoan == loginDTO.Username && tk.MatKhau == loginDTO.Password);

            if (user == null)
            {
                return Unauthorized(); // Unauthorized if user not found or invalid credentials
            }

            // Generate JWT token
            var tokenString = GenerateJWTToken(user);

            // Return token
            return Ok(new { Token = tokenString });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            // Check if username is already taken
            if (await _context.TaiKhoanMatKhaus.AnyAsync(tk => tk.TaiKhoan == registerDTO.Username))
            {
                return BadRequest("Username is already taken");
            }

            // Create new user with default role set to 3
            var newUser = new TaiKhoanMatKhau
            {
                TaiKhoan = registerDTO.Username,
                MatKhau = registerDTO.Password,
                Role = 3 // Default role
                         // You may need to set other properties of the user based on your application requirements
            };

            _context.TaiKhoanMatKhaus.Add(newUser);
            await _context.SaveChangesAsync();

            return StatusCode(201); // Created
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // In a stateless JWT-based authentication system, logout operation usually does not require any action
            // Client can simply discard the JWT token
            return Ok();
        }

        private string GenerateJWTToken(TaiKhoanMatKhau user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var securityKey = new SymmetricSecurityKey(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.TaiKhoan.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
