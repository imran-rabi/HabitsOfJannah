using IslamicHabitTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IslamicHabitTracker.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing habit-related database operations
    /// </summary>
    public interface IHabitRepository
    {
        /// <summary>
        /// Creates a new habit in the database
        /// </summary>
        /// <param name="habit">The habit object to create</param>
        /// <returns>The created habit with updated ID</returns>
        Task<Habit> CreateAsync(Habit habit);

        /// <summary>
        /// Retrieves all habits for a specific user
        /// </summary>
        /// <param name="userId">The user's ID</param>
        /// <returns>List of habits belonging to the user</returns>
        Task<IEnumerable<Habit>> GetUserHabitsAsync(int userId);

        /// <summary>
        /// Retrieves a specific habit by its ID
        /// </summary>
        /// <param name="id">The habit's ID</param>
        /// <returns>The habit if found, null otherwise</returns>
        Task<Habit> GetByIdAsync(int id);

        /// <summary>
        /// Updates an existing habit's information
        /// </summary>
        /// <param name="habit">The habit object with updated information</param>
        /// <returns>The updated habit</returns>
        Task<Habit> UpdateAsync(Habit habit);

        /// <summary>
        /// Deletes a habit from the database
        /// </summary>
        /// <param name="id">The ID of the habit to delete</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        Task<bool> DeleteAsync(int id);
    }
}
