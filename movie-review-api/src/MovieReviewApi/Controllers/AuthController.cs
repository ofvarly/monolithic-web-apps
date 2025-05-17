using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieReviewApi.DTOs;
using MovieReviewApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieReviewApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;

        public AuthController(IConfiguration configuration, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, Serilog.ILogger logger, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger.ForContext<AuthController>();
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.Error("Model state is invalid during registration.");
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            _logger.Information("User registration result: {Result}", result.Succeeded);
 
            if (result.Succeeded)
            {
                var role = string.IsNullOrEmpty(registerDto.Role) ? "User" : registerDto.Role;

                if (!await _roleManager.RoleExistsAsync(role))
                {
                    _logger.Information("Role {Role} does not exist. Creating role.", role);

                    await _roleManager.CreateAsync(new IdentityRole(role));
                    
                    _logger.Information("Role {Role} created successfully.", role);
                }

                await _userManager.AddToRoleAsync(user, role);

                _logger.Information("User {Username} added to role {Role}.", user.UserName, role);

                return Ok("User registered successfully.");
            }

            _logger.Error("User registration failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var existingUser = await _userManager.FindByNameAsync(loginDto.Username);

            if (existingUser == null)
            {
                _logger.Error("User not found: {Username}", loginDto.Username);
                return BadRequest("Invalid user.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(existingUser, loginDto.Password, false);

            if (result.Succeeded)
            {
                _logger.Information("User {Username} logged in successfully.", loginDto.Username);

                return Ok(new { token = GenerateJwtToken(existingUser) });
            }

            _logger.Error("Invalid password for user: {Username}", loginDto.Username);

            return Unauthorized("Invalid password.");
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? "");
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var roles = _userManager.GetRolesAsync(user).Result;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            _logger.Information("Claims added to token: {Claims}", claims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"] ?? "30")),
                SigningCredentials = creds
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
 
            _logger.Information("JWT token generated successfully for user: {Username}", user.UserName);
            return tokenHandler.WriteToken(token);
        }
    }
}