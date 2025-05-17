using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductsAPI.DTO;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> CreateUser(UserDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                DateAdded = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return StatusCode(201);
            }

            return BadRequest(result.Errors);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid email or password" });
            }
            
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false); // false means we don't want to lock the user out

            if (result.Succeeded)
            {
                return Ok( new { token = GenerateJWT(user) }); 
            }

            return Unauthorized(); // 401 status code means unauthorized access
        }

        private object GenerateJWT(AppUser user)
        { 
            var tokenHandler = new JwtSecurityTokenHandler(); // to create a JWT token we need a token handler object
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Secret").Value ?? ""); // we need a secret key to sign the token

            // token descriptor does the actual work of creating the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // the subject is the user's claims
                // we are storing the user's id and username in the token
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // we are storing the user's id in the token
                    new Claim(ClaimTypes.Name, user.UserName ?? "") // we are storing the user's username in the token
                }),
                Expires = DateTime.UtcNow.AddDays(1), // the token will expire in 1 day
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // we are using HMACSHA256 to sign the token 
            };

            var token = tokenHandler.CreateToken(tokenDescriptor); // create the token
            return tokenHandler.WriteToken(token); // return the token
        }
    }
}