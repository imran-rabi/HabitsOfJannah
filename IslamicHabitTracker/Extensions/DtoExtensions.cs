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
                Password = dto.Password,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
                Habits = new List<Habit>(),
                Achievements = new List<Achievement>()
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
                LastLoginAt = user.LastLoginAt ?? user.CreatedAt,
                TotalHabits = user.Habits?.Count ?? 0,
                ActiveHabits = user.Habits?.Count(h => h.IsActive) ?? 0,
                ProfilePictureUrl = user.ProfilePicture != null ? 
                    $"data:image/jpeg;base64,{Convert.ToBase64String(user.ProfilePicture)}" : null
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

        public static HabitResponseDTO ToHabitDto(this Habit habit)
        {
            return new HabitResponseDTO
            {
                Id = habit.Id,
                Name = habit.Name,
                Description = habit.Description,
                Frequency = habit.Frequency,
                StartDate = habit.StartDate,
                EndDate = habit.EndDate,
                TargetValue = habit.TargetValue,
                ReminderTime = habit.ReminderTime,
                Notes = habit.Notes,
                IsActive = habit.IsActive,
                CreatedAt = habit.CreatedAt,
                UpdatedAt = habit.UpdatedAt,
                TodayProgress = habit.Progress?
                    .Where(p => p.Date.Date == DateTime.UtcNow.Date)
                    .Select(p => p.ToDto())
                    .FirstOrDefault(),
                RecentProgress = habit.Progress?
                    .OrderByDescending(p => p.Date)
                    .Take(7)
                    .Select(p => p.ToDto())
                    .ToList() ?? new List<HabitProgressDTO>()
            };
        }

        public static List<HabitResponseDTO> ToHabitDtos(this IEnumerable<Habit> habits)
        {
            return habits.Select(h => h.ToHabitDto()).ToList();
        }

        public static HabitProgressDTO ToDto(this HabitProgress progress)
        {
            return new HabitProgressDTO
            {
                HabitId = progress.HabitId,
                Date = progress.Date,
                Type = progress.Type,
                Value = progress.Value,
                Notes = progress.Notes,
                Mood = progress.Mood
            };
        }

        public static HabitProgress ToProgress(this UpdateHabitProgressDTO dto)
        {
            return new HabitProgress
            {
                Value = dto.Value,
                Notes = dto.Notes
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
    }
} 