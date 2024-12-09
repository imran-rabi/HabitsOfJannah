using IslamicHabitTracker.DTOs;
using IslamicHabitTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using IslamicHabitTracker.Extensions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace IslamicHabitTracker.Controllers
{
    [ApiController]
    [Route("api/habits/{habitId}/progress")]
    [Authorize]
    public class HabitProgressController : BaseController
    {
        private readonly IHabitProgressService _progressService;
        private readonly ILogger<HabitProgressController> _logger;

        public HabitProgressController(IHabitProgressService progressService, ILogger<HabitProgressController> logger)
        {
            _progressService = progressService;
            _logger = logger;
        }

        /// <summary>
        /// Record progress for a habit
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="progressDto">Progress details</param>
        [HttpPost]
        [ProducesResponseType(typeof(HabitProgressDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RecordProgress(int habitId, [FromBody] HabitProgressDTO progressDto)
        {
            try
            {
                _logger.LogInformation($"Starting RecordProgress for habit {habitId}");
                _logger.LogInformation($"Request data: {JsonSerializer.Serialize(progressDto)}");
                _logger.LogInformation($"Headers: {JsonSerializer.Serialize(Request.Headers)}");

                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    _logger.LogWarning($"Invalid model state: {errors}");
                    return BadRequest(new { message = errors });
                }

                var userId = User.GetUserId();
                _logger.LogInformation($"User ID: {userId}");

                var progress = await _progressService.RecordProgressAsync(habitId, userId, progressDto);
                _logger.LogInformation($"Progress recorded with ID: {progress.Id}");

                return Ok(progress.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RecordProgress");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get progress by ID
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="id">ID of the progress entry</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HabitProgressDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int habitId, int id)
        {
            var userId = User.GetUserId();
            var progress = await _progressService.GetProgressByIdAsync(id, userId);
            if (progress == null || progress.HabitId != habitId)
                return NotFound();

            return Ok(progress.ToDto());
        }

        /// <summary>
        /// Update progress entry
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="id">ID of the progress entry</param>
        /// <param name="progressDto">Updated progress information</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(HabitProgressDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int habitId, int id, [FromBody] UpdateHabitProgressDTO progressDto)
        {
            var userId = User.GetUserId();
            var progress = await _progressService.UpdateProgressAsync(id, userId, progressDto);
            if (progress.HabitId != habitId)
                return NotFound();

            return Ok(progress.ToDto());
        }

        /// <summary>
        /// Delete progress entry
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        /// <param name="id">ID of the progress entry</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int habitId, int id)
        {
            var userId = User.GetUserId();
            var result = await _progressService.DeleteProgressAsync(id, userId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Get today's progress
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        [HttpGet("today")]
        [ProducesResponseType(typeof(HabitProgressDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTodayProgress(int habitId)
        {
            var userId = User.GetUserId();
            var progress = await _progressService.GetTodayProgressAsync(habitId, userId);
            if (progress == null)
                return NotFound();

            return Ok(progress.ToDto());
        }

        /// <summary>
        /// Get progress history
        /// </summary>
        /// <param name="habitId">ID of the habit</param>
        [HttpGet]
        [ProducesResponseType(typeof(HabitProgressDTO[]), 200)]
        public async Task<IActionResult> GetHistory(int habitId)
        {
            var userId = User.GetUserId();
            var progress = await _progressService.GetProgressHistoryAsync(habitId, userId);
            return Ok(progress.Select(p => p.ToDto()));
        }

        // Add OPTIONS method to handle preflight requests
        [HttpOptions]
        public IActionResult PreflightRoute()
        {
            return NoContent();
        }
    }
}
