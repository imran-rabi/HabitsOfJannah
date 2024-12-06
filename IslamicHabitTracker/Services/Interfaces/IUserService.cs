using IslamicHabitTracker.Models;
using System.Threading.Tasks;

namespace IslamicHabitTracker.Services.Interfaces
{
    /// <summary>
    /// Interface for user-related business logic operations
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="user">User information for registration</param>
        /// <returns>The registered user with JWT token</returns>
        Task<(User User, string Token)> RegisterAsync(User user);

        /// <summary>
        /// Authenticates a user and returns a JWT token
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns>The authenticated user and JWT token</returns>
        Task<(User User, string Token)> LoginAsync(string email, string password);

        /// <summary>
        /// Updates user profile information
        /// </summary>
        /// <param name="userId">ID of the user to update</param>
        /// <param name="updateUser">Updated user information</param>
        /// <returns>The updated user</returns>
        Task<User> UpdateProfileAsync(int userId, User updateUser);

        /// <summary>
        /// Changes user's password
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="currentPassword">Current password</param>
        /// <param name="newPassword">New password</param>
        /// <returns>True if password was changed successfully</returns>
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);

        /// <summary>
        /// Gets a user by ID
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>The user</returns>
        Task<User> GetByIdAsync(int userId);
    }
} 