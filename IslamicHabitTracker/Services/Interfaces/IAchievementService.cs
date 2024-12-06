using IslamicHabitTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using IslamicHabitTracker.DTOs;

namespace IslamicHabitTracker.Services.Interfaces
{
    /// <summary>
    /// Interface for achievement-related business logic operations
    /// </summary>
    public interface IAchievementService
    {
        /// <summary>
        /// Checks and awards achievements based on user's progress
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="habitId">ID of the habit</param>
        /// <returns>List of newly awarded achievements</returns>
        Task<IEnumerable<Achievement>> CheckAndAwardAchievementsAsync(int userId, int habitId);

        /// <summary>
        /// Gets all achievements for a user
        /// </summary>
        Task<IEnumerable<Achievement>> GetUserAchievementsAsync(int userId);

        /// <summary>
        /// Gets achievements for a specific habit
        /// </summary>
        Task<IEnumerable<Achievement>> GetHabitAchievementsAsync(int userId, int habitId);

        /// <summary>
        /// Gets achievement progress for a user
        /// </summary>
        Task<IEnumerable<AchievementProgressDTO>> GetUserAchievementProgressAsync(int userId);
    }
} 