using IslamicHabitTracker.DTOs;
using IslamicHabitTracker.Models;
using IslamicHabitTracker.Services.Interfaces;
using IslamicHabitTracker.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IslamicHabitTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
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
            (User user, string token) = await _userService.RegisterAsync(userDto.ToUser());
            return Ok(new { User = user.ToProfileDto(), Token = token });
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
            (User user, string token) = await _userService.LoginAsync(loginDto.Email, loginDto.Password);
            return Ok(new { User = user.ToProfileDto(), Token = token });
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
            var userId = User.GetUserId();
            var user = await _userService.GetByIdAsync(userId);
            return Ok(user.ToProfileDto());
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        /// <param name="userDto">Updated user information</param>
        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(typeof(UserProfileDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDTO userDto)
        {
            var userId = User.GetUserId();
            var updatedUser = await _userService.UpdateProfileAsync(userId, userDto.ToUser());
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
            var userId = User.GetUserId();
            await _userService.ChangePasswordAsync(userId, passwordDto.CurrentPassword, passwordDto.NewPassword);
            return Ok();
        }
    }
}
