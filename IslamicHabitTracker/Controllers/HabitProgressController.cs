using IslamicHabitTracker.DTOs;
using IslamicHabitTracker.Services.Interfaces;
using IslamicHabitTracker.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using IslamicHabitTracker.Models;

namespace IslamicHabitTracker.Controllers
{
    [ApiController]
    [Route("api/habits/{habitId}/progress")]
    [Authorize]
    public class HabitProgressController : ControllerBase
    {
        private readonly IHabitProgressService _progressService;
        private readonly IHabitService _habitService;

        public HabitProgressController(
            IHabitProgressService progressService,
            IHabitService habitService)
        {
            _progressService = progressService;
            _habitService = habitService;
        }

        /// <summary>
        /// Record progress for today
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(HabitProgressDTO), 201)]
        public async Task<IActionResult> RecordProgress(
            int habitId, 
            [FromBody] HabitProgressDTO progressDto)
        {
            var userId = User.GetUserId();
            
            // Verify habit belongs to user
            await _habitService.GetByIdAsync(habitId, userId);

            var progress = new HabitProgress
            {
                HabitId = habitId,
                Date = progressDto.Date,
                Value = progressDto.Value,
                Notes = progressDto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _progressService.RecordProgressAsync(habitId, userId, progress);
            return CreatedAtAction(
                nameof(GetProgress), 
                new { habitId, date = result.Date }, 
                result.ToDto());
        }

        /// <summary>
        /// Get progress for a specific date
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(HabitProgressDTO), 200)]
        public async Task<IActionResult> GetProgress(
            int habitId, 
            [FromQuery] DateTime date)
        {
            var userId = User.GetUserId();
            await _habitService.GetByIdAsync(habitId, userId);

            var progress = await _progressService.GetProgressByDateAsync(habitId, userId, date);
            return Ok(progress.ToDto());
        }

        /// <summary>
        /// Update existing progress entry
        /// </summary>
        [HttpPut("{progressId}")]
        [ProducesResponseType(typeof(HabitProgressDTO), 200)]
        public async Task<IActionResult> UpdateProgress(
            int habitId,
            int progressId, 
            [FromBody] UpdateHabitProgressDTO updateDto)
        {
            var userId = User.GetUserId();
            await _habitService.GetByIdAsync(habitId, userId);

            var progress = await _progressService.GetProgressByIdAsync(progressId, userId);
            if (progress == null || progress.HabitId != habitId)
                return NotFound();

            progress.Value = updateDto.Value;
            progress.Notes = updateDto.Notes;
            progress.UpdatedAt = DateTime.UtcNow;

            var result = await _progressService.UpdateProgressAsync(progress);
            return Ok(result.ToDto());
        }

        /// <summary>
        /// Get progress for date range
        /// </summary>
        [HttpGet("range")]
        [ProducesResponseType(typeof(List<HabitProgressDTO>), 200)]
        public async Task<IActionResult> GetProgressRange(
            int habitId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var userId = User.GetUserId();
            await _habitService.GetByIdAsync(habitId, userId);

            var progress = await _progressService.GetProgressByDateRangeAsync(
                habitId, userId, startDate, endDate);
            return Ok(progress.Select(p => p.ToDto()));
        }
    }
}
