using IslamicHabitTracker.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IslamicHabitTracker.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing habit progress-related database operations
    /// </summary>
    public interface IHabitProgressRepository
    {
        /// <summary>
        /// Records progress for a habit
        /// </summary>
        /// <param name="progress">The progress object to create</param>
        /// <returns>The created progress entry</returns>
        Task<HabitProgress> CreateAsync(HabitProgress progress);

        /// <summary>
        /// Retrieves progress for a specific habit within a date range
        /// </summary>
        /// <param name="habitId">The habit's ID</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of progress entries within the date range</returns>
        Task<List<HabitProgress>> GetProgressByDateRangeAsync(
            int habitId,
            DateTime startDate,
            DateTime endDate);

        /// <summary>
        /// Updates an existing progress entry
        /// </summary>
        /// <param name="progress">The progress object with updated information</param>
        /// <returns>The updated progress entry</returns>
        Task<HabitProgress> UpdateAsync(HabitProgress progress);

        /// <summary>
        /// Retrieves progress for a specific date and habit
        /// </summary>
        /// <param name="habitId">The habit's ID</param>
        /// <param name="date">The specific date</param>
        /// <returns>The progress entry if found, null otherwise</returns>
        Task<HabitProgress> GetByDateAsync(int habitId, DateTime date);

        /// <summary>
        /// Deletes a progress entry
        /// </summary>
        /// <param name="id">The ID of the progress entry to delete</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        Task<bool> DeleteAsync(int progressId);

        /// <summary>
        /// Retrieves progress for a specific progress entry
        /// </summary>
        /// <param name="progressId">The ID of the progress entry</param>
        /// <returns>The progress entry if found, null otherwise</returns>
        Task<HabitProgress> GetByIdAsync(int progressId);

        /// <summary>
        /// Retrieves progress history for a specific habit
        /// </summary>
        /// <param name="habitId">The habit's ID</param>
        /// <returns>List of progress entries for the habit</returns>
        Task<List<HabitProgress>> GetProgressHistoryAsync(int habitId);

        /// <summary>
        /// Retrieves all progress entries for a specific habit
        /// </summary>
        /// <param name="habitId">The habit's ID</param>
        /// <returns>List of all progress entries for the habit</returns>
        Task<List<HabitProgress>> GetAllForHabitAsync(int habitId);
    }
}
