using IslamicHabitTracker.DTOs;
using IslamicHabitTracker.Services.Interfaces;
using IslamicHabitTracker.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IslamicHabitTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AchievementsController : ControllerBase
    {
        private readonly IAchievementService _achievementService;

        public AchievementsController(IAchievementService achievementService)
        {
            _achievementService = achievementService;
        }

        /// <summary>
        /// Get all achievements for current user
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(AchievementDTO[]), 200)]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.GetUserId();
            var achievements = await _achievementService.GetUserAchievementsAsync(userId);
            return Ok(achievements.ToDtos());
        }

        /// <summary>
        /// Get achievements for a specific habit
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        [HttpGet("habit/{habitId}")]
        [ProducesResponseType(typeof(AchievementDTO[]), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByHabit(int habitId)
        {
            var userId = User.GetUserId();
            var achievements = await _achievementService.GetHabitAchievementsAsync(userId, habitId);
            return Ok(achievements.ToDtos());
        }
    }
}
