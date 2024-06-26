using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Microservice.Data;
using Users.Microservice.Models;

namespace Users.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(UserContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }



        /// <summary>
        /// Gets all the users
        /// </summary>
        /// <returns></returns>
        [HttpGet("all-user")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        /// <summary>
        /// Registers a user
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userDto)
        {
            // Check if email already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email address already registered" });
            }

            // If email does not exist, proceed with registration
            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully" });
        }


        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == userDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token, User = user });
        }

        /// <summary>
        /// Password change
        /// </summary>
        /// <param name="changePasswordDto"></param>
        /// <returns></returns>

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(UserChangePasswordDto changePasswordDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == changePasswordDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid current password" });
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Password changed successfully" });
        }




        /// <summary>
        /// Generate the JWT Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string GenerateJwtToken1(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email!)
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }




        /// <summary>
        /// Generates a key
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Fetch the secret key from the configuration
            var secretKey = _configuration["JwtSettings:SecretKey"];

            // Ensure the key is fetched successfully
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ApplicationException("JWT SecretKey is missing in configuration.");
            }

            var key = Encoding.UTF8.GetBytes(secretKey);

            // Ensure key length is at least 32 bytes (256 bits)
            if (key.Length < 32)
            {
                throw new ApplicationException("JWT SecretKey length is less than 32 bytes.");
            }

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];

            var Sectoken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email!)
                },
                expires: DateTime.UtcNow.AddMinutes(120),
                signingCredentials: credentials
            );

            var token = tokenHandler.WriteToken(Sectoken);
            return token;
        }








    }
}
