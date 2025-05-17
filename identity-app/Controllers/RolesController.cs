using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{

    public class RolesController : Controller
    {
        // Private fields to hold the dependencies
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        // Constructor that takes dependencies as parameters
        public RolesController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            // Assign the dependencies to the private fields
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_roleManager.Roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppRole model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            // Attempt to find the role by its ID
            var role = await _roleManager.FindByIdAsync(id);

            // Check if the role exists and has a valid name
            if (role != null && role.Name != null)
            {
                // Retrieve the users associated with this role
                ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);

                // Return the view to edit the role, passing the role object to the view
                return View(role);
            }

            // If the role is not found, redirect to the Index action
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppRole model)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Attempt to find the role by its ID
                var role = await _roleManager.FindByIdAsync(model.Id);

                // Check if the role exists
                if (role != null)
                {
                    // Update the role's name with the new name from the model
                    role.Name = model.Name;

                    // Attempt to update the role in the database
                    var result = await _roleManager.UpdateAsync(role);

                    // Check if the update operation succeeded
                    if (result.Succeeded)
                    {
                        // Redirect to the Index action if the update was successful
                        return RedirectToAction("Index");
                    }

                    // If the update operation failed, add the errors to the model state
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    if (role.Name != null)
                    {
                        // Retrieve the users associated with this role for the view
                        ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);
                    }
                    
                }
            }
            // Return the view with the model to allow the user to correct any errors
            return View(model);
        }
    }
}