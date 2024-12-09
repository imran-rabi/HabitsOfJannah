using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IslamicHabitTracker.DTOs
{
    /// <summary>
    /// DTO for habit response data
    /// </summary>
    public class HabitDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Frequency is required")]
        public string Frequency { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        [Required(ErrorMessage = "Target value is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Target value must be greater than 0")]
        public int TargetValue { get; set; }
        
        public string ReminderTime { get; set; } = string.Empty;
        
        public string Notes { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for habit statistics
    /// </summary>
    public class HabitStatisticsDTO
    {
        public int TotalDays { get; set; }
        public int CompletedDays { get; set; }
        public double CompletionRate { get; set; }
        public int CurrentStreak { get; set; }
        public int BestStreak { get; set; }
        public DateTime? LastCompletedDate { get; set; }
        public List<DailyProgressDTO> DailyProgress { get; set; } = new List<DailyProgressDTO>();
    }

    /// <summary>
    /// DTO for daily progress in statistics
    /// </summary>
    public class DailyProgressDTO
    {
        public DateTime Date { get; set; }
        public bool Completed { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for habit response data
    /// </summary>
    public class HabitResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TargetValue { get; set; }
        public string? ReminderTime { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public HabitProgressDTO? TodayProgress { get; set; }
        public List<HabitProgressDTO> RecentProgress { get; set; } = new();
    }

    /// <summary>
    /// DTO for habit statistics
    /// </summary>
    public class HabitStatsDTO
    {
        public int CurrentStreak { get; set; }
        public int BestStreak { get; set; }
        public double CompletionRate { get; set; }
        public int TotalCheckins { get; set; }
    }
}
