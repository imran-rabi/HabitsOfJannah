using System;
using System.Collections.Generic;

namespace IslamicHabitTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }

        // Navigation properties
        public virtual ICollection<Habit> Habits { get; set; }
        public virtual ICollection<Achievement> Achievements { get; set; }
    }
}
