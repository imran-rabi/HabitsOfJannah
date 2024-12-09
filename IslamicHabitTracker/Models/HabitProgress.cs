using System;

namespace IslamicHabitTracker.Models
{
    public class HabitProgress
    {
        public int Id { get; set; }
        public int HabitId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } // completion or percentage
        public int Value { get; set; }
        public string? Notes { get; set; }
        public string? Mood { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public Habit Habit { get; set; }
    }
}
