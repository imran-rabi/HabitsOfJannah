using IslamicHabitTracker.DTOs;
using IslamicHabitTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IslamicHabitTracker.Extensions
{
    public static class DtoExtensions
    {
        public static User ToUser(this UserDTO dto)
        {
            return new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password
            };
        }

        public static UserProfileDTO ToProfileDto(this User user)
        {
            return new UserProfileDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                TotalHabits = user.Habits?.Count ?? 0,
                ActiveHabits = user.Habits?.Count(h => h.IsActive) ?? 0
            };
        }

        public static Habit ToHabit(this CreateHabitDTO dto, int userId)
        {
            return new Habit
            {
                UserId = userId,
                Name = dto.Name,
                Description = dto.Description,
                Frequency = dto.Frequency,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = true
            };
        }

        public static Habit ToHabit(this UpdateHabitDTO dto)
        {
            return new Habit
            {
                Name = dto.Name,
                Description = dto.Description,
                Frequency = dto.Frequency,
                StartDate = dto.StartDate ?? DateTime.UtcNow,
                EndDate = dto.EndDate,
                IsActive = dto.IsActive ?? true
            };
        }

        public static HabitDTO ToDto(this Habit habit)
        {
            return new HabitDTO
            {
                Id = habit.Id,
                Name = habit.Name,
                Description = habit.Description,
                Frequency = habit.Frequency,
                StartDate = habit.StartDate,
                EndDate = habit.EndDate,
                IsActive = habit.IsActive,
                CreatedAt = habit.CreatedAt,
                UpdatedAt = habit.UpdatedAt,
                RecentProgress = habit.Progress?.Select(p => p.ToDto()).ToList() ?? new List<HabitProgressDTO>()
            };
        }

        public static HabitProgressDTO ToDto(this HabitProgress progress)
        {
            return new HabitProgressDTO
            {
                Date = progress.Date,
                Value = progress.Value,
                Notes = progress.Notes
            };
        }

        public static HabitProgress ToProgress(this UpdateHabitProgressDTO dto)
        {
            return new HabitProgress
            {
                Value = dto.Value,
                Notes = dto.Notes,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static HabitStatisticsDTO ToDto(this HabitStatistics stats)
        {
            return new HabitStatisticsDTO
            {
                TotalDays = stats.TotalDays,
                CompletedDays = stats.CompletedDays,
                CompletionRate = stats.CompletionRate,
                CurrentStreak = stats.CurrentStreak,
                BestStreak = stats.BestStreak,
                LastCompletedDate = stats.LastCompletedDate,
                DailyProgress = stats.DailyProgress?.Select(p => new DailyProgressDTO 
                { 
                    Date = p.Date,
                    Completed = p.Completed,
                    Notes = p.Notes 
                }).ToList() ?? new List<DailyProgressDTO>()
            };
        }

        public static AchievementDTO ToDto(this Achievement achievement)
        {
            return new AchievementDTO
            {
                Id = achievement.Id,
                Name = achievement.Name,
                Description = achievement.Description,
                Type = achievement.Type,
                HabitId = achievement.HabitId,
                HabitName = achievement.Habit?.Name ?? string.Empty,
                AwardedAt = achievement.AwardedAt
            };
        }

        public static IEnumerable<AchievementDTO> ToDtos(this IEnumerable<Achievement> achievements)
        {
            return achievements.Select(a => a.ToDto());
        }

        public static IEnumerable<HabitDTO> ToDtos(this IEnumerable<Habit> habits)
        {
            return habits.Select(h => h.ToDto());
        }
    }
} 