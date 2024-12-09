using System;
using System.Collections.Generic;

namespace IslamicHabitTracker.Models
{
    public class Habit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Frequency { get; set; } // Daily, Weekly, Monthly
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TargetValue { get; set; }
        public string? ReminderTime { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public User User { get; set; }
        public ICollection<HabitProgress> Progress { get; set; }
    }
}
