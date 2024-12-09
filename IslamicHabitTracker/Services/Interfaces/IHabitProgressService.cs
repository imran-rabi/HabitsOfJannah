using IslamicHabitTracker.Models;
using IslamicHabitTracker.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IslamicHabitTracker.Services.Interfaces
{
    /// <summary>
    /// Interface for habit progress-related business logic operations
    /// </summary>
    public interface IHabitProgressService
    {
        /// <summary>
        /// Records progress for a habit
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="progress">Progress details</param>
        /// <returns>The recorded progress</returns>
        Task<HabitProgress> RecordProgressAsync(int habitId, int userId, HabitProgressDTO progressDto);

        /// <summary>
        /// Updates existing progress entry
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="progressId">ID of the progress entry</param>
        /// <param name="updateProgress">Updated progress information</param>
        /// <returns>The updated progress</returns>
        Task<HabitProgress> UpdateProgressAsync(int progressId, int userId, UpdateHabitProgressDTO progressDto);

        /// <summary>
        /// Gets progress for a habit on a specific date
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="date">The date for which to get progress</param>
        /// <returns>The progress for the specified date</returns>
        Task<HabitProgress> GetProgressByDateAsync(int userId, int habitId, DateTime date);

        Task<HabitProgress> GetProgressByIdAsync(int progressId, int userId);
        Task<HabitProgress> UpdateProgressAsync(HabitProgress progress);
        Task<IEnumerable<HabitProgress>> GetProgressByDateRangeAsync(int habitId, int userId, DateTime startDate, DateTime endDate);
        Task<bool> DeleteProgressAsync(int progressId, int userId);
        Task<List<HabitProgress>> GetProgressHistoryAsync(int habitId, int userId);
        Task<HabitProgress> GetTodayProgressAsync(int habitId, int userId);
    }
} 