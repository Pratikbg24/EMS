using EmployeeManagementAPI.Data;
using EmployeeManagementAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagementAPI.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly ApplicationDbContext _context;
            private readonly IConfiguration _configuration;

            public AuthController(ApplicationDbContext context, IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;
            }
        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            // Find the user by email
            var user = _context.Employees.SingleOrDefault(u => u.Email == loginModel.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password))
            {
                // User not found or password doesn't match
                return Unauthorized("Invalid username or password.");
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            // Return user details and token
            return Ok(new
            {
                Token = token,
                UserDetails = new
                {
                    user.Id,
                    user.Email,
                    user.Role,
                    user.FirstName,
                    user.LastName
                }
            });
        }

        private string GenerateJwtToken(Employee user)
        {
            // Set up the claims for the user
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };

            // Create the signing key (ensure that the key is at least 32 characters)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Generate the JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Token expires in 30 minutes
                signingCredentials: creds
            );

            // Return the generated token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
