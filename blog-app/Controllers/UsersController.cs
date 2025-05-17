using System.Globalization;
using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Login()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Posts");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName || x.Email == model.Email);
                if (user == null)
                {
                    _userRepository.CreateUser(new User
                    {
                        UserName = model.UserName,
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        Image = "p1.jpg"
                    });
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı ya da Eposta kullanımda.");
                }

            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Find the user by email and password
                var isUser = _userRepository.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

                // If user is found
                if (isUser != null)
                {
                    // Create a list of claims for the user
                    var userClaims = new List<Claim>
                    {
                        new(ClaimTypes.NameIdentifier, isUser.UserId.ToString()), // User ID
                        new(ClaimTypes.Name, isUser.UserName ?? ""), // Username
                        new(ClaimTypes.GivenName, isUser.Name ?? ""), // Name
                        new(ClaimTypes.UserData, isUser.Image ?? "") // User image
                    };

                    // If the user is an admin, add the admin role claim
                    if (isUser.Email == "user1@gmail.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    // Create a claims identity with the user claims and authentication scheme
                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    // Set authentication properties
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true // Persistent cookie
                    };

                    // Sign out any existing authentication
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    // Sign in the user with the new claims identity and authentication properties
                    await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                    // Redirect to the Posts index page
                    return RedirectToAction("Index", "Posts");
                }
                else
                {
                    // If user is not found, add a model error
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış.");
                }
            }
            // If model state is not valid, return the view with the model
            return View(model);
        }

        public IActionResult Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }

            var user = _userRepository
                                .Users
                                .Include(x => x.Posts)
                                .Include(x => x.Comments)
                                .ThenInclude(x => x.Post)
                                .FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

    }
}