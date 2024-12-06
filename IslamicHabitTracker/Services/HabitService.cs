using IslamicHabitTracker.Models;
using IslamicHabitTracker.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IslamicHabitTracker.Services.Interfaces;

namespace IslamicHabitTracker.Services
{
    /// <summary>
    /// Service for handling habit-related business logic
    /// </summary>
    public class HabitService : IHabitService
    {
        private readonly IHabitRepository _habitRepository;
        private readonly IHabitProgressRepository _progressRepository;

        /// <summary>
        /// Constructor for HabitService
        /// </summary>
        /// <param name="habitRepository">Repository for habit data access</param>
        /// <param name="progressRepository">Repository for habit progress data access</param>
        public HabitService(IHabitRepository habitRepository, IHabitProgressRepository progressRepository)
        {
            _habitRepository = habitRepository;
            _progressRepository = progressRepository;
        }

        /// <summary>
        /// Creates a new habit for a user
        /// </summary>
        /// <param name="habit">The habit to create</param>
        /// <returns>The created habit</returns>
        public async Task<Habit> CreateHabitAsync(Habit habit)
        {
            habit.CreatedAt = DateTime.UtcNow;
            habit.StartDate = habit.StartDate.Date; // Normalize to start of day
            
            return await _habitRepository.CreateAsync(habit);
        }

        /// <summary>
        /// Gets all habits for a specific user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>List of user's habits with their progress</returns>
        public async Task<IEnumerable<Habit>> GetUserHabitsAsync(int userId)
        {
            return await _habitRepository.GetUserHabitsAsync(userId);
        }

        /// <summary>
        /// Updates an existing habit
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="habitId">ID of the habit to update</param>
        /// <param name="updateHabit">Updated habit information</param>
        /// <returns>The updated habit</returns>
        public async Task<Habit> UpdateHabitAsync(int userId, int habitId, Habit updateHabit)
        {
            var habit = await _habitRepository.GetByIdAsync(habitId);
            if (habit == null || habit.UserId != userId)
            {
                throw new InvalidOperationException("Habit not found or unauthorized");
            }

            // Update allowed fields
            habit.Name = updateHabit.Name;
            habit.Description = updateHabit.Description;
            habit.Frequency = updateHabit.Frequency;
            habit.UpdatedAt = DateTime.UtcNow;

            return await _habitRepository.UpdateAsync(habit);
        }

        /// <summary>
        /// Deletes a habit
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="habitId">ID of the habit to delete</param>
        /// <returns>True if deletion was successful</returns>
        public async Task<bool> DeleteHabitAsync(int userId, int habitId)
        {
            var habit = await _habitRepository.GetByIdAsync(habitId);
            if (habit == null || habit.UserId != userId)
            {
                throw new InvalidOperationException("Habit not found or unauthorized");
            }

            return await _habitRepository.DeleteAsync(habitId);
        }

        /// <summary>
        /// Gets habit statistics for a specific time period
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="startDate">Start date of the period</param>
        /// <param name="endDate">End date of the period</param>
        /// <returns>Statistics for the habit</returns>
        public async Task<HabitStatistics> GetHabitStatisticsAsync(int habitId, DateTime startDate, DateTime endDate)
        {
            var habit = await _habitRepository.GetByIdAsync(habitId);
            if (habit == null)
                throw new InvalidOperationException("Habit not found");

            var progress = await _progressRepository.GetProgressByDateRangeAsync(habitId, startDate, endDate);
            var progressList = progress.ToList(); // Convert to list to avoid multiple enumeration

            var totalDays = (int)(endDate - startDate).TotalDays + 1;
            var completedDays = progressList.Count(p => p.Value >= 100);
            var lastCompleted = progressList
                .Where(p => p.Value >= 100)
                .OrderByDescending(p => p.Date)
                .FirstOrDefault()?.Date;

            var currentStreak = CalculateCurrentStreak(progressList);
            var bestStreak = CalculateBestStreak(progressList);
            var completionRate = totalDays > 0 ? (completedDays * 100.0) / totalDays : 0;

            return new HabitStatistics
            {
                TotalDays = totalDays,
                CompletedDays = completedDays,
                CurrentStreak = currentStreak,
                BestStreak = bestStreak,
                LastCompletedDate = lastCompleted,
                CompletionRate = completionRate,
                DailyProgress = GetDailyProgress(progressList, startDate, endDate)
            };
        }

        private int CalculateCurrentStreak(IEnumerable<HabitProgress> progress)
        {
            if (!progress.Any()) return 0;

            var streak = 0;
            var currentDate = DateTime.UtcNow.Date;
            var orderedProgress = progress
                .OrderByDescending(p => p.Date)
                .ToList();

            for (var date = currentDate; date >= orderedProgress.Min(p => p.Date); date = date.AddDays(-1))
            {
                var dayProgress = orderedProgress.FirstOrDefault(p => p.Date.Date == date);
                if (dayProgress == null || dayProgress.Value < 100)
                    break;
                streak++;
            }

            return streak;
        }

        private int CalculateBestStreak(IEnumerable<HabitProgress> progress)
        {
            if (!progress.Any()) return 0;

            var bestStreak = 0;
            var currentStreak = 0;
            var orderedProgress = progress
                .OrderBy(p => p.Date)
                .ToList();

            var currentDate = orderedProgress.First().Date;

            foreach (var p in orderedProgress)
            {
                if (p.Date.Date == currentDate && p.Value >= 100)
                {
                    currentStreak++;
                    bestStreak = Math.Max(bestStreak, currentStreak);
                }
                else if (p.Date.Date != currentDate)
                {
                    // Check if we missed any days
                    var daysDiff = (p.Date.Date - currentDate).Days;
                    if (daysDiff > 1)
                    {
                        currentStreak = p.Value >= 100 ? 1 : 0;
                    }
                    else
                    {
                        currentStreak = p.Value >= 100 ? currentStreak + 1 : 0;
                    }
                    bestStreak = Math.Max(bestStreak, currentStreak);
                }
                currentDate = p.Date.Date.AddDays(1);
            }

            return bestStreak;
        }

        private ICollection<DailyProgress> GetDailyProgress(
            IEnumerable<HabitProgress> progress, 
            DateTime startDate, 
            DateTime endDate)
        {
            var dailyProgress = new List<DailyProgress>();
            var progressByDate = progress.ToDictionary(p => p.Date.Date);

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                if (progressByDate.TryGetValue(date, out var dayProgress))
                {
                    dailyProgress.Add(new DailyProgress
                    {
                        Date = date,
                        Completed = dayProgress.Value >= 100,
                        Notes = dayProgress.Notes
                    });
                }
                else
                {
                    dailyProgress.Add(new DailyProgress
                    {
                        Date = date,
                        Completed = false,
                        Notes = string.Empty
                    });
                }
            }

            return dailyProgress;
        }

        public async Task<Habit> GetByIdAsync(int id, int userId)
        {
            var habit = await _habitRepository.GetByIdAsync(id);
            if (habit == null || habit.UserId != userId)
            {
                throw new InvalidOperationException("Habit not found or unauthorized");
            }
            return habit;
        }
    }
}