using System;
using System.Collections.Generic;

namespace IslamicHabitTracker.DTOs
{
    /// <summary>
    /// DTO for user's overall achievement progress
    /// </summary>
    public class UserAchievementProgressDTO
    {
        /// <summary>
        /// Total number of achievements available
        /// </summary>
        public int TotalAchievements { get; set; }

        /// <summary>
        /// Number of completed achievements
        /// </summary>
        public int CompletedAchievements { get; set; }

        /// <summary>
        /// Overall completion percentage
        /// </summary>
        public double OverallCompletion { get; set; }

        /// <summary>
        /// List of achievement progress
        /// </summary>
        public List<AchievementProgressDTO> Achievements { get; set; } = new List<AchievementProgressDTO>();

        /// <summary>
        /// Achievement statistics by type
        /// </summary>
        public Dictionary<string, int> AchievementsByType { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Recent achievements (last 30 days)
        /// </summary>
        public List<AchievementDTO> RecentAchievements { get; set; } = new List<AchievementDTO>();
    }
} 