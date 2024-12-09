using IslamicHabitTracker.DTOs;
using IslamicHabitTracker.Models;
using IslamicHabitTracker.Services.Interfaces;
using IslamicHabitTracker.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using IslamicHabitTracker.Controllers;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace IslamicHabitTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="userDto">User registration information</param>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserProfileDTO), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        {
            try
            {
                var user = userDto.ToUser();
                (User createdUser, string token) = await _userService.RegisterAsync(user);
                return Ok(new { User = createdUser.ToProfileDto(), Token = token });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="loginDto">Login credentials</param>
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserProfileDTO), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
        {
            try
            {
                (User user, string token) = await _userService.LoginAsync(loginDto.Email, loginDto.Password);
                return Ok(new { User = user.ToProfileDto(), Token = token });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get current user's profile
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDTO), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.GetUserId();
                _logger.LogInformation($"Getting profile for user ID: {userId}");

                var user = await _userService.GetByIdAsync(userId);
                _logger.LogInformation($"User retrieved: {user?.Name ?? "null"}");
                
                if (user == null)
                {
                    _logger.LogWarning($"User not found for ID: {userId}");
                    return NotFound(new { message = "User not found" });
                }

                try
                {
                    var userDto = user.ToProfileDto();
                    _logger.LogInformation($"Profile DTO created: {JsonSerializer.Serialize(userDto)}");
                    return Ok(userDto);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error converting user to DTO");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetProfile");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        /// <param name="updateDto">Updated user information</param>
        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO updateDto)
        {
            var userId = User.GetUserId();
            var user = await _userService.GetByIdAsync(userId);
            
            user.Name = updateDto.Name;
            user.Email = updateDto.Email;
            
            var updatedUser = await _userService.UpdateProfileAsync(userId, user);
            return Ok(updatedUser.ToProfileDto());
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="passwordDto">Password change information</param>
        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO passwordDto)
        {
            try
            {
                var userId = User.GetUserId();
                await _userService.ChangePasswordAsync(
                    userId, 
                    passwordDto.CurrentPassword, 
                    passwordDto.NewPassword
                );
                return Ok(new { message = "Password changed successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("profile-picture")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDTO), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            try
            {
                var userId = User.GetUserId();
                
                if (file == null || file.Length == 0)
                    return BadRequest(new { message = "No file uploaded" });

                var allowedTypes = new[] { "image/jpeg", "image/png" };
                if (!allowedTypes.Contains(file.ContentType))
                    return BadRequest(new { message = "Invalid file type. Please use JPEG or PNG" });

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                var user = await _userService.UpdateProfilePictureAsync(userId, imageData);
                var userDto = user.ToProfileDto();
                
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
