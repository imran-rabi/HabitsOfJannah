using IslamicHabitTracker.Models;   
using IslamicHabitTracker.Repositories.Interfaces;
using IslamicHabitTracker.Services.Interfaces;
using IslamicHabitTracker.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;

namespace IslamicHabitTracker.Services
{
    /// <summary>
    /// Service for handling achievement-related business logic
    /// </summary>
    public class AchievementService : IAchievementService
    {
        private readonly IHabitProgressRepository _progressRepository;
        private readonly IHabitRepository _habitRepository;

        /// <summary>
        /// Constructor for AchievementService
        /// </summary>
        public AchievementService(
            IHabitProgressRepository progressRepository,
            IHabitRepository habitRepository)
        {
            _progressRepository = progressRepository;
            _habitRepository = habitRepository;
        }

        /// <summary>
        /// Checks and awards achievements based on user's progress
        /// </summary>
        public async Task<IEnumerable<Achievement>> CheckAndAwardAchievementsAsync(int userId, int habitId)
        {
            var achievements = new List<Achievement>();
            var habit = await _habitRepository.GetByIdAsync(habitId);
            
            if (habit == null || habit.UserId != userId)
            {
                throw new InvalidOperationException("Habit not found or unauthorized");
            }

            // Get progress for the last 30 days
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddDays(-30);
            var progress = await _progressRepository.GetProgressByDateRangeAsync(habitId, startDate, endDate);

            // Check for different types of achievements
            achievements.AddRange(await CheckStreakAchievements(progress));
            achievements.AddRange(await CheckCompletionAchievements(progress));
            achievements.AddRange(await CheckMilestoneAchievements(habit, progress));

            return achievements;
        }

        /// <summary>
        /// Gets all achievements for a user
        /// </summary>
        public async Task<IEnumerable<Achievement>> GetUserAchievementsAsync(int userId)
        {
            var habits = await _habitRepository.GetUserHabitsAsync(userId);
            var achievements = new List<Achievement>();
            
            foreach (var habit in habits)
            {
                var habitAchievements = await CheckAndAwardAchievementsAsync(userId, habit.Id);
                achievements.AddRange(habitAchievements);
            }

            return achievements;
        }

        /// <summary>
        /// Gets achievements for a specific habit
        /// </summary>
        public async Task<IEnumerable<Achievement>> GetHabitAchievementsAsync(int userId, int habitId)
        {
            var habit = await _habitRepository.GetByIdAsync(habitId);
            if (habit == null || habit.UserId != userId)
            {
                throw new InvalidOperationException("Habit not found or unauthorized");
            }

            return await CheckAndAwardAchievementsAsync(userId, habitId);
        }

        /// <summary>
        /// Gets achievement progress for a user
        /// </summary>
        public async Task<IEnumerable<AchievementProgressDTO>> GetUserAchievementProgressAsync(int userId)
        {
            var habits = await _habitRepository.GetUserHabitsAsync(userId);
            var progressList = new List<AchievementProgressDTO>();

            foreach (var habit in habits)
            {
                var habitProgress = await _progressRepository.GetProgressByDateRangeAsync(
                    habit.Id,
                    DateTime.UtcNow.AddDays(-30),
                    DateTime.UtcNow);

                var streakProgress = CalculateStreakProgress(habit, habitProgress);
                var completionProgress = CalculateCompletionProgress(habit, habitProgress);
                var milestoneProgress = CalculateMilestoneProgress(habit, habitProgress);

                progressList.AddRange(new[] { streakProgress, completionProgress, milestoneProgress });
            }

            return progressList;
        }

        private AchievementProgressDTO CalculateStreakProgress(Habit habit, IEnumerable<HabitProgress> progress) 
        {
            var currentStreak = CalculateCurrentStreak(progress);
            return new AchievementProgressDTO
            {
                Name = "Daily Streak",
                Description = "Complete the habit every day",
                CurrentProgress = currentStreak,
                RequiredProgress = 30,
                IsCompleted = currentStreak >= 30,
                AchievementType = "Streak",
                HabitId = habit.Id,
                HabitName = habit.Name,
                LastUpdated = DateTime.UtcNow
            };
        }

        private AchievementProgressDTO CalculateCompletionProgress(Habit habit, IEnumerable<HabitProgress> progress)
        {
            var completedDays = progress.Count(p => p.Value >= 100);
            return new AchievementProgressDTO
            {
                Name = "Completion Master",
                Description = "Complete the habit fully for a number of days",
                CurrentProgress = completedDays,
                RequiredProgress = 20,
                IsCompleted = completedDays >= 20,
                AchievementType = "Completion",
                HabitId = habit.Id,
                HabitName = habit.Name,
                LastUpdated = DateTime.UtcNow
            };
        }

        private AchievementProgressDTO CalculateMilestoneProgress(Habit habit, IEnumerable<HabitProgress> progress)
        {
            var totalProgress = progress.Sum(p => p.Value);
            return new AchievementProgressDTO
            {
                Name = "Progress Milestone",
                Description = "Accumulate total progress points",
                CurrentProgress = totalProgress,
                RequiredProgress = 1000,
                IsCompleted = totalProgress >= 1000,
                AchievementType = "Milestone",
                HabitId = habit.Id,
                HabitName = habit.Name,
                LastUpdated = DateTime.UtcNow
            };
        }

        private int CalculateCurrentStreak(IEnumerable<HabitProgress> progress)
        {
            var streak = 0;
            var orderedProgress = progress.OrderByDescending(p => p.Date);
            var currentDate = DateTime.UtcNow.Date;

            foreach (var p in orderedProgress)
            {
                if (p.Date.Date == currentDate.AddDays(-streak) && p.Value >= 100)
                {
                    streak++;
                }
                else
                {
                    break;
                }
            }

            return streak;
        }

        private async Task<IEnumerable<Achievement>> CheckStreakAchievements(IEnumerable<HabitProgress> progress)
        {
            return await Task.Run(() => 
            {
                var achievements = new List<Achievement>();
                var streak = CalculateCurrentStreak(progress);
                
                if (streak >= 7) achievements.Add(new Achievement { Name = "Week Warrior", Type = "Streak" });
                if (streak >= 30) achievements.Add(new Achievement { Name = "Month Master", Type = "Streak" });
                
                return achievements;
            });
        }

        private async Task<IEnumerable<Achievement>> CheckCompletionAchievements(IEnumerable<HabitProgress> progress)
        {
            return await Task.Run(() => 
            {
                var completedDays = progress.Count(p => p.Value >= 100);
                var achievements = new List<Achievement>();
                
                if (completedDays >= 10) achievements.Add(new Achievement { Name = "Perfect Ten", Type = "Completion" });
                
                return achievements;
            });
        }

        private async Task<IEnumerable<Achievement>> CheckMilestoneAchievements(Habit habit, IEnumerable<HabitProgress> progress)
        {
            return await Task.Run(() => 
            {
                var totalProgress = progress.Sum(p => p.Value);
                var achievements = new List<Achievement>();
                
                if (totalProgress >= 1000) achievements.Add(new Achievement { Name = "Progress Pro", Type = "Milestone" });
                
                return achievements;
            });
        }
    }
}
