using IslamicHabitTracker.Models;
using IslamicHabitTracker.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using IslamicHabitTracker.Services.Interfaces;
using IslamicHabitTracker.Helpers.Interfaces;
using BC = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Logging;

namespace IslamicHabitTracker.Services
{
    /// <summary>
    /// Service for handling user-related business logic
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHelper _jwtHelper;
        private readonly ILogger<UserService> _logger;

        /// <summary>
        /// Constructor for UserService
        /// </summary>
        /// <param name="userRepository">Repository for user data access</param>
        /// <param name="jwtHelper">Helper for JWT token operations</param>
        /// <param name="logger">Logger for logging</param>
        public UserService(IUserRepository userRepository, IJwtHelper jwtHelper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="user">User information for registration</param>
        /// <returns>The registered user with JWT token</returns>
        public async Task<(User User, string Token)> RegisterAsync(User user)
        {
            // Check if email already exists
            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already registered");
            }

            // Set default values for new user using local time
            user.CreatedAt = DateTime.Now;
            user.LastLoginAt = DateTime.Now;
            user.Password = BC.HashPassword(user.Password);

            // Save the user
            var createdUser = await _userRepository.CreateAsync(user);
            
            // Generate JWT token
            var token = _jwtHelper.GenerateToken(createdUser);

            return (createdUser, token);
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns>The authenticated user and JWT token</returns>
        public async Task<(User User, string Token)> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !BC.Verify(password, user.Password))
            {
                throw new InvalidOperationException("Invalid email or password");
            }

            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            var token = _jwtHelper.GenerateToken(user);
            return (user, token);
        }

        /// <summary>
        /// Updates user profile information
        /// </summary>
        /// <param name="userId">ID of the user to update</param>
        /// <param name="updateUser">Updated user information</param>
        /// <returns>The updated user</returns>
        public async Task<User> UpdateProfileAsync(int userId, User updateUser)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            // Update only allowed fields
            user.Name = updateUser.Name;
            user.UpdatedAt = DateTime.UtcNow;

            return await _userRepository.UpdateAsync(user);
        }

        /// <summary>
        /// Changes user's password
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="currentPassword">Current password</param>
        /// <param name="newPassword">New password</param>
        /// <returns>True if password was changed successfully</returns>
        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || !BC.Verify(currentPassword, user.Password))
            {
                throw new InvalidOperationException("Invalid current password");
            }

            user.Password = BC.HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>The user</returns>
        public async Task<User> GetByIdAsync(int userId)
        {
            _logger.LogInformation($"Getting user by ID: {userId}");
            try
            {
                var user = await _userRepository.GetByIdAsync(userId, includeHabits: true);
                _logger.LogInformation($"User found: {user?.Name ?? "null"}");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting user by ID: {userId}");
                throw;
            }
        }

        /// <summary>
        /// Updates user's profile picture
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="imageData">Binary data of the profile picture</param>
        public async Task<User> UpdateProfilePictureAsync(int userId, byte[] imageData)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            user.ProfilePicture = imageData;
            user.UpdatedAt = DateTime.UtcNow;

            return await _userRepository.UpdateAsync(user);
        }
    }
} 
