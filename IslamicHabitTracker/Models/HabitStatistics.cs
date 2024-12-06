using System;
using System.Collections.Generic;

namespace IslamicHabitTracker.Models
{
    public class HabitStatistics
    {
        public int TotalDays { get; set; }
        public int CompletedDays { get; set; }
        public double CompletionRate { get; set; }
        public int CurrentStreak { get; set; }
        public int BestStreak { get; set; }
        public DateTime? LastCompletedDate { get; set; }
        public ICollection<DailyProgress> DailyProgress { get; set; }
    }

    public class DailyProgress
    {
        public DateTime Date { get; set; }
        public bool Completed { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
} 