using Microsoft.AspNetCore.Mvc;
using MovieReviewApi.DTOs;
using MovieReviewApi.Interfaces;


namespace MovieReviewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly Serilog.ILogger _logger;
        public AdminController(IAdminService adminService, Serilog.ILogger logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _adminService.GetAllUsersAsync();
                _logger.Information("Fetched all users successfully.");
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error fetching users");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _adminService.GetUserByIdAsync(id);

                if (user == null)
                {
                    _logger.Warning("User with ID {Id} not found", id);
                    return NotFound("User not found");
                }

                _logger.Information("Fetched user with ID {Id} successfully", id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error fetching user with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] RegisterDto userDto)
        {
            try
            {
                var updatedUser = await _adminService.UpdateUserAsync(id, userDto);
                if (updatedUser == null)
                {
                    _logger.Warning("User with ID {Id} not found for update", id);
                    return NotFound("User not found");
                }

                _logger.Information("Updated user with ID {Id} successfully", id);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating user with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                _logger.Information("DeleteUser endpoint called with ID: {Id}", id);
                await _adminService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                _logger.Error("User not found with ID: {Id}", id);
                return NotFound();
            }

        }
    }
}