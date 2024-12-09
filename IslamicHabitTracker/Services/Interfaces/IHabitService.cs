using IslamicHabitTracker.Models;
using IslamicHabitTracker.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IslamicHabitTracker.Services.Interfaces
{
    /// <summary>
    /// Interface for habit-related business logic operations
    /// </summary>
    public interface IHabitService
    {
        /// <summary>
        /// Creates a new habit for a user
        /// </summary>
        /// <param name="habit">The habit to create</param>
        /// <returns>The created habit</returns>
        Task<Habit> CreateAsync(int userId, HabitDTO habitDto);

        /// <summary>
        /// Gets all habits for a specific user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>List of user's habits with their progress</returns>
        Task<List<Habit>> GetAllByUserIdAsync(int userId);

        /// <summary>
        /// Updates an existing habit
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="habitId">ID of the habit to update</param>
        /// <param name="updateHabit">Updated habit information</param>
        /// <returns>The updated habit</returns>
        Task<Habit> UpdateAsync(int habitId, int userId, HabitDTO habitDto);

        /// <summary>
        /// Deletes a habit
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="habitId">ID of the habit to delete</param>
        /// <returns>True if deletion was successful</returns>
        Task<bool> DeleteAsync(int habitId, int userId);

        /// <summary>
        /// Gets habit statistics for a specific time period
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="startDate">Start date of the period</param>
        /// <param name="endDate">End date of the period</param>
        /// <returns>Statistics for the habit</returns>
        Task<HabitStatsDTO> GetStatisticsAsync(int habitId, int userId);

        /// <summary>
        /// Gets a habit by ID for a specific user
        /// </summary>
        /// <param name="id">ID of the habit</param>
        /// <param name="userId">ID of the user</param>
        /// <returns>The habit</returns>
        Task<Habit> GetByIdAsync(int habitId, int userId);

        /// <summary>
        /// Gets calendar data for a specific time period
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="startDate">Start date of the period</param>
        /// <param name="endDate">End date of the period</param>
        /// <returns>Calendar data for the habit</returns>
        Task<List<CalendarDayDTO>> GetCalendarAsync(int habitId, int userId, DateTime startDate, DateTime endDate);
    }
} 