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
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public int CurrentStreak { get; set; }
        public int BestStreak { get; set; }
        public double CompletionRate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<HabitProgressDTO> RecentProgress { get; set; } = new List<HabitProgressDTO>();
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
}
