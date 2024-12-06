using System;

namespace IslamicHabitTracker.DTOs
{
    /// <summary>
    /// DTO for achievement response data
    /// </summary>
    public class AchievementDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int? HabitId { get; set; }
        public string HabitName { get; set; } = string.Empty;
        public DateTime AwardedAt { get; set; }
    }

    /// <summary>
    /// DTO for achievement progress
    /// </summary>
    public class AchievementProgressDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CurrentProgress { get; set; }
        public int RequiredProgress { get; set; }
        public bool IsCompleted { get; set; }
        public string AchievementType { get; set; } = string.Empty;
        public int HabitId { get; set; }
        public string HabitName { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }
    }
} 