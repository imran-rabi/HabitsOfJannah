using IslamicHabitTracker.Models;

namespace IslamicHabitTracker.Helpers.Interfaces
{
    /// <summary>
    /// Interface for JWT token generation and validation
    /// </summary>
    public interface IJwtHelper
    {
        /// <summary>
        /// Generates a JWT token for the given user
        /// </summary>
        /// <param name="user">The user to generate token for</param>
        /// <returns>JWT token string</returns>
        string GenerateToken(User user);

        /// <summary>
        /// Validates a JWT token
        /// </summary>
        /// <param name="token">Token to validate</param>
        /// <returns>True if token is valid, false otherwise</returns>
        bool ValidateToken(string token);

        /// <summary>
        /// Gets the user ID from a JWT token
        /// </summary>
        /// <param name="token">JWT token</param>
        /// <returns>User ID if token is valid</returns>
        int? GetUserIdFromToken(string token);
    }
} 