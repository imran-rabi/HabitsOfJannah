using IslamicHabitTracker.Models;
using IslamicHabitTracker.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using IslamicHabitTracker.Services.Interfaces;
using System.Collections.Generic;
using IslamicHabitTracker.DTOs;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace IslamicHabitTracker.Services
{
    /// <summary>
    /// Service for handling habit progress-related business logic
    /// </summary>
    public class HabitProgressService : IHabitProgressService
    {
        private readonly IHabitProgressRepository _progressRepository;
        private readonly IHabitRepository _habitRepository;
        private readonly ILogger<HabitProgressService> _logger;

        /// <summary>
        /// Constructor for HabitProgressService
        /// </summary>
        public HabitProgressService(
            IHabitProgressRepository progressRepository,
            IHabitRepository habitRepository,
            ILogger<HabitProgressService> logger)
        {
            _progressRepository = progressRepository;
            _habitRepository = habitRepository;
            _logger = logger;
        }

        /// <summary>
        /// Records progress for a habit
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="progressDto">Progress details</param>
        /// <returns>The recorded progress</returns>
        public async Task<HabitProgress> RecordProgressAsync(int habitId, int userId, HabitProgressDTO progressDto)
        {
            try
            {
                _logger.LogInformation($"Recording progress: {JsonSerializer.Serialize(progressDto)}");
                
                var habit = await _habitRepository.GetByIdAsync(habitId);
                if (habit == null)
                {
                    throw new InvalidOperationException($"Habit with ID {habitId} not found");
                }
                
                if (habit.UserId != userId)
                {
                    throw new InvalidOperationException("Unauthorized to record progress for this habit");
                }

                var progress = new HabitProgress
                {
                    HabitId = habitId,
                    Date = progressDto.Date.Date,
                    Type = progressDto.Type,
                    Value = progressDto.Value,
                    Notes = progressDto.Notes,
                    Mood = progressDto.Mood,
                    CreatedAt = DateTime.UtcNow
                };

                // Check if progress already exists for this date
                var existingProgress = await _progressRepository.GetByDateAsync(habitId, progress.Date);
                if (existingProgress != null)
                {
                    _logger.LogInformation($"Updating existing progress for date {progress.Date}");
                    existingProgress.Value = progress.Value;
                    existingProgress.Notes = progress.Notes;
                    existingProgress.Mood = progress.Mood;
                    return await _progressRepository.UpdateAsync(existingProgress);
                }

                _logger.LogInformation($"Creating new progress entry for date {progress.Date}");
                return await _progressRepository.CreateAsync(progress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording progress");
                throw;
            }
        }

        /// <summary>
        /// Updates existing progress entry
        /// </summary>
        /// <param name="progressId">ID of the progress entry</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="progressDto">The updated progress information</param>
        /// <returns>The updated progress</returns>
        public async Task<HabitProgress> UpdateProgressAsync(int progressId, int userId, UpdateHabitProgressDTO progressDto)
        {
            var progress = await _progressRepository.GetByIdAsync(progressId);
            if (progress == null || progress.Habit.UserId != userId)
                throw new InvalidOperationException("Progress entry not found");

            progress.Value = progressDto.Value;
            progress.Notes = progressDto.Notes;

            return await _progressRepository.UpdateAsync(progress);
        }

        /// <summary>
        /// Gets progress history for a habit
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="userId">ID of the user</param>
        /// <returns>The progress history for the specified habit</returns>
        public async Task<List<HabitProgress>> GetProgressHistoryAsync(int habitId, int userId)
        {
            var habit = await _habitRepository.GetByIdAsync(habitId);
            if (habit == null || habit.UserId != userId)
                throw new InvalidOperationException("Habit not found");

            return await _progressRepository.GetProgressHistoryAsync(habitId);
        }

        /// <summary>
        /// Gets today's progress for a habit
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="userId">ID of the user</param>
        /// <returns>Today's progress for the specified habit</returns>
        public async Task<HabitProgress> GetTodayProgressAsync(int habitId, int userId)
        {
            var habit = await _habitRepository.GetByIdAsync(habitId);
            if (habit == null || habit.UserId != userId)
                throw new InvalidOperationException("Habit not found");

            return await _progressRepository.GetByDateAsync(habitId, DateTime.UtcNow.Date);
        }

        /// <summary>
        /// Deletes a progress entry
        /// </summary>
        /// <param name="progressId">ID of the progress entry</param>
        /// <param name="userId">ID of the user</param>
        /// <returns>True if the progress entry was deleted, false otherwise</returns>
        public async Task<bool> DeleteProgressAsync(int progressId, int userId)
        {
            var progress = await _progressRepository.GetByIdAsync(progressId);
            if (progress == null || progress.Habit.UserId != userId)
                throw new InvalidOperationException("Progress entry not found");

            return await _progressRepository.DeleteAsync(progressId);
        }

        public async Task<HabitProgress> GetProgressByDateAsync(int userId, int habitId, DateTime date)
        {
            var habit = await _habitRepository.GetByIdAsync(habitId);
            if (habit == null || habit.UserId != userId)
                throw new InvalidOperationException("Habit not found");

            return await _progressRepository.GetByDateAsync(habitId, date.Date);
        }

        public async Task<HabitProgress> GetProgressByIdAsync(int progressId, int userId)
        {
            var progress = await _progressRepository.GetByIdAsync(progressId);
            if (progress == null || progress.Habit.UserId != userId)
                throw new InvalidOperationException("Progress entry not found");

            return progress;
        }

        public async Task<HabitProgress> UpdateProgressAsync(HabitProgress progress)
        {
            return await _progressRepository.UpdateAsync(progress);
        }

        public async Task<IEnumerable<HabitProgress>> GetProgressByDateRangeAsync(int habitId, int userId, DateTime startDate, DateTime endDate)
        {
            var habit = await _habitRepository.GetByIdAsync(habitId);
            if (habit == null || habit.UserId != userId)
                throw new InvalidOperationException("Habit not found");

            return await _progressRepository.GetProgressByDateRangeAsync(habitId, startDate.Date, endDate.Date);
        }
    }
}
