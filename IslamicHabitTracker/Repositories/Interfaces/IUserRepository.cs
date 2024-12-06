using IslamicHabitTracker.Models;
using System.Threading.Tasks;

namespace IslamicHabitTracker.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing user-related database operations
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Creates a new user in the database
        /// </summary>
        /// <param name="user">The user object to create</param>
        /// <returns>The created user with updated ID</returns>
        Task<User> CreateAsync(User user);

        /// <summary>
        /// Retrieves a user by their ID
        /// </summary>
        /// <param name="id">The user's ID</param>
        /// <returns>The user if found, null otherwise</returns>
        Task<User> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves a user by their email address
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <returns>The user if found, null otherwise</returns>
        Task<User> GetByEmailAsync(string email);

        /// <summary>
        /// Updates an existing user's information
        /// </summary>
        /// <param name="user">The user object with updated information</param>
        /// <returns>The updated user</returns>
        Task<User> UpdateAsync(User user);

        /// <summary>
        /// Deletes a user from the database
        /// </summary>
        /// <param name="id">The ID of the user to delete</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        Task<bool> DeleteAsync(int id);
    }
}
