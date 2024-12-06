using IslamicHabitTracker.DTOs;
using IslamicHabitTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using IslamicHabitTracker.Extensions;

namespace IslamicHabitTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HabitsController : ControllerBase
    {
        private readonly IHabitService _habitService;

        public HabitsController(IHabitService habitService)
        {
            _habitService = habitService;
        }

        /// <summary>
        /// Create a new habit
        /// </summary>
        /// <param name="habitDto">Habit creation information</param>
        [HttpPost]
        [ProducesResponseType(typeof(HabitDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreateHabitDTO habitDto)
        {
            var userId = User.GetUserId();
            var habit = await _habitService.CreateHabitAsync(habitDto.ToHabit(userId));
            return CreatedAtAction(nameof(GetById), new { id = habit.Id }, habit.ToDto());
        }

        /// <summary>
        /// Get all habits for current user
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(HabitDTO[]), 200)]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.GetUserId();
            var habits = await _habitService.GetUserHabitsAsync(userId);
            return Ok(habits.ToDtos());
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
            return Ok(habit.ToDto());
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
        public async Task<IActionResult> Update(int id, [FromBody] UpdateHabitDTO habitDto)
        {
            var userId = User.GetUserId();
            var habit = await _habitService.UpdateHabitAsync(userId, id, habitDto.ToHabit());
            return Ok(habit.ToDto());
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
            await _habitService.DeleteHabitAsync(userId, id);
            return NoContent();
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
        public async Task<IActionResult> GetStatistics(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var userId = User.GetUserId();
            var statistics = await _habitService.GetHabitStatisticsAsync(id, startDate, endDate);
            return Ok(statistics.ToDto());
        }
    }
}
