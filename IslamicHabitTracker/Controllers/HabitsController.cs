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
    [Route("api/[controller]")]
    [Authorize]
    public class HabitsController : BaseController
    {
        private readonly IHabitService _habitService;
        private readonly IHabitProgressService _progressService;
        private readonly ILogger<HabitsController> _logger;

        public HabitsController(IHabitService habitService, IHabitProgressService progressService, ILogger<HabitsController> logger)
        {
            _habitService = habitService;
            _progressService = progressService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new habit
        /// </summary>
        /// <param name="habitDto">Habit creation information</param>
        [HttpPost]
        [ProducesResponseType(typeof(HabitResponseDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] HabitDTO habitDto)
        {
            try
            {
                _logger.LogInformation($"Received habit data: {JsonSerializer.Serialize(habitDto)}");

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    _logger.LogWarning($"Validation errors: {JsonSerializer.Serialize(errors)}");
                    return BadRequest(new { message = "Validation failed", errors });
                }

                var userId = User.GetUserId();
                _logger.LogInformation($"Creating habit for user {userId}");

                var habit = await _habitService.CreateAsync(userId, habitDto);
                var response = habit.ToHabitDto();

                _logger.LogInformation($"Successfully created habit with ID: {habit.Id}");
                return CreatedAtAction(nameof(GetById), new { id = habit.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating habit");
                return StatusCode(500, new { message = "Failed to create habit", error = ex.Message });
            }
        }

        /// <summary>
        /// Get all habits for current user
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(HabitDTO[]), 200)]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.GetUserId();
            var habits = await _habitService.GetAllByUserIdAsync(userId);
            return Ok(habits.Select(h => h.ToHabitDto()));
        }

        /// <summary>
        /// Get a specific habit by ID
        /// </summary>
        /// <param name="id">Habit ID</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HabitDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = User.GetUserId();
            var habit = await _habitService.GetByIdAsync(id, userId);
            return Ok(habit.ToHabitDto());
        }

        /// <summary>
        /// Update an existing habit
        /// </summary>
        /// <param name="id">Habit ID</param>
        /// <param name="habitDto">Updated habit information</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(HabitDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] HabitDTO habitDto)
        {
            var userId = User.GetUserId();
            var habit = await _habitService.UpdateAsync(id, userId, habitDto);
            return Ok(habit.ToHabitDto());
        }

        /// <summary>
        /// Delete a habit
        /// </summary>
        /// <param name="id">Habit ID</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.GetUserId();
            await _habitService.DeleteAsync(id, userId);
            return Ok();
        }

        /// <summary>
        /// Get habit statistics
        /// </summary>
        /// <param name="id">Habit ID</param>
        /// <param name="startDate">Start date for statistics</param>
        /// <param name="endDate">End date for statistics</param>
        [HttpGet("{id}/statistics")]
        [ProducesResponseType(typeof(HabitStatisticsDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStatistics(int id)
        {
            var userId = User.GetUserId();
            var stats = await _habitService.GetStatisticsAsync(id, userId);
            return Ok(stats);
        }

        [HttpGet("{id}/calendar")]
        public async Task<IActionResult> GetCalendar(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var userId = User.GetUserId();
            var calendar = await _habitService.GetCalendarAsync(id, userId, startDate, endDate);
            return Ok(calendar);
        }

        [HttpPost("{id}/progress")]
        public async Task<IActionResult> RecordProgress(int id, [FromBody] HabitProgressDTO progressDto)
        {
            var userId = User.GetUserId();
            var progress = await _progressService.RecordProgressAsync(id, userId, progressDto);
            return Ok(progress.ToDto());
        }

        [HttpGet("{id}/progress")]
        public async Task<IActionResult> GetProgress(int id)
        {
            var userId = User.GetUserId();
            var progress = await _progressService.GetProgressHistoryAsync(id, userId);
            return Ok(progress.Select(p => p.ToDto()));
        }
    }
}
