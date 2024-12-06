using IslamicHabitTracker.Models;
using IslamicHabitTracker.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using IslamicHabitTracker.Services.Interfaces;
using System.Collections.Generic;

namespace IslamicHabitTracker.Services
{
    /// <summary>
    /// Service for handling habit progress-related business logic
    /// </summary>
    public class HabitProgressService : IHabitProgressService
    {
        private readonly IHabitProgressRepository _progressRepository;
        private readonly IHabitRepository _habitRepository;

        /// <summary>
        /// Constructor for HabitProgressService
        /// </summary>
        public HabitProgressService(
            IHabitProgressRepository progressRepository,
            IHabitRepository habitRepository)
        {
            _progressRepository = progressRepository;
            _habitRepository = habitRepository;
        }

        /// <summary>
        /// Records progress for a habit
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="progress">Progress details</param>
        /// <returns>The recorded progress</returns>
        public async Task<HabitProgress> RecordProgressAsync(int habitId, int userId, HabitProgress progress)
        {
            var habit = await _habitRepository.GetByIdAsync(habitId);
            if (habit == null || habit.UserId != userId)
            {
                throw new InvalidOperationException("Habit not found or unauthorized");
            }

            return await _progressRepository.RecordProgressAsync(progress);
        }

        /// <summary>
        /// Gets progress for a habit on a specific date
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="date">The date for which to get the progress</param>
        /// <returns>The progress for the specified date</returns>
        public async Task<HabitProgress> GetProgressByDateAsync(int habitId, int userId, DateTime date)
        {
            var habit = await _habitRepository.GetByIdAsync(habitId);
            if (habit == null || habit.UserId != userId)
            {
                throw new InvalidOperationException("Habit not found or unauthorized");
            }

            return await _progressRepository.GetProgressByDateAsync(habitId, date);
        }

        /// <summary>
        /// Gets progress by ID
        /// </summary>
        /// <param name="progressId">ID of the progress entry</param>
        /// <param name="userId">ID of the user</param>
        /// <returns>The progress entry</returns>
        public async Task<HabitProgress> GetProgressByIdAsync(int progressId, int userId)
        {
            var progress = await _progressRepository.GetProgressByIdAsync(progressId);
            if (progress == null)
            {
                return null;
            }

            var habit = await _habitRepository.GetByIdAsync(progress.HabitId);
            if (habit.UserId != userId)
            {
                throw new InvalidOperationException("Unauthorized");
            }

            return progress;
        }

        /// <summary>
        /// Updates existing progress entry
        /// </summary>
        /// <param name="progress">The updated progress information</param>
        /// <returns>The updated progress</returns>
        public async Task<HabitProgress> UpdateProgressAsync(HabitProgress progress)
        {
            return await _progressRepository.UpdateProgressAsync(progress);
        }

        /// <summary>
        /// Gets progress for a habit within a date range
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="startDate">The start date of the range</param>
        /// <param name="endDate">The end date of the range</param>
        /// <returns>The progress entries within the specified date range</returns>
        public async Task<IEnumerable<HabitProgress>> GetProgressByDateRangeAsync(
            int habitId, int userId, DateTime startDate, DateTime endDate)
        {
            var habit = await _habitRepository.GetByIdAsync(habitId);
            if (habit == null || habit.UserId != userId)
            {
                throw new InvalidOperationException("Habit not found or unauthorized");
            }

            return await _progressRepository.GetProgressByDateRangeAsync(habitId, startDate, endDate);
        }

        /// <summary>
        /// Deletes a progress entry
        /// </summary>
        /// <param name="progressId">ID of the progress entry</param>
        /// <param name="userId">ID of the user</param>
        /// <returns>True if the progress entry was deleted, false otherwise</returns>
        public async Task<bool> DeleteProgressAsync(int progressId, int userId)
        {
            var progress = await GetProgressByIdAsync(progressId, userId);
            if (progress == null)
            {
                return false;
            }

            return await _progressRepository.DeleteProgressAsync(progressId);
        }

        public async Task<HabitProgress> UpdateProgressAsync(int userId, int progressId, HabitProgress updateProgress)
        {
            var progress = await GetProgressByIdAsync(progressId, userId);
            if (progress == null)
                throw new InvalidOperationException("Progress not found or unauthorized");

            progress.Value = updateProgress.Value;
            progress.Notes = updateProgress.Notes;
            progress.UpdatedAt = DateTime.UtcNow;

            return await _progressRepository.UpdateProgressAsync(progress);
        }
    }
}
