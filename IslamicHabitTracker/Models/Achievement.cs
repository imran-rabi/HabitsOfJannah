using System;

namespace IslamicHabitTracker.Models
{
    public class Achievement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? HabitId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime AwardedAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; }  
        public virtual Habit Habit { get; set; }
    }
}
