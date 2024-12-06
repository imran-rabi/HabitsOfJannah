using IslamicHabitTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using IslamicHabitTracker.Repositories.Interfaces;
using IslamicHabitTracker.Data;
namespace IslamicHabitTracker.Repositories
{
    /// <summary>
    /// Repository for managing Habit-related database operations
    /// </summary>
    public class HabitRepository : IHabitRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor that injects the database context
        /// </summary>
        /// <param name="context">The application's database context</param>
        public HabitRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new habit in the database
        /// </summary>
        /// <param name="habit">The habit object to create</param>
        /// <returns>The created habit with updated ID</returns>
        public async Task<Habit> CreateAsync(Habit habit)
        {
            await _context.Habits.AddAsync(habit);
            await _context.SaveChangesAsync();
            return habit;
        }

        /// <summary>
        /// Retrieves all habits for a specific user
        /// </summary>
        /// <param name="userId">The user's ID</param>
        /// <returns>List of habits belonging to the user</returns>
        public async Task<IEnumerable<Habit>> GetUserHabitsAsync(int userId)
        {
            return await _context.Habits
                .Include(h => h.Progress)
                .Where(h => h.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific habit by its ID
        /// </summary>
        /// <param name="id">The habit's ID</param>
        /// <returns>The habit if found, null otherwise</returns>
        public async Task<Habit> GetByIdAsync(int id)
        {
            return await _context.Habits
                .Include(h => h.Progress)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        /// <summary>
        /// Updates an existing habit's information
        /// </summary>
        /// <param name="habit">The habit object with updated information</param>
        /// <returns>The updated habit</returns>
        public async Task<Habit> UpdateAsync(Habit habit)
        {
            _context.Habits.Update(habit);
            await _context.SaveChangesAsync();
            return habit;
        }

        /// <summary>
        /// Deletes a habit from the database
        /// </summary>
        /// <param name="id">The ID of the habit to delete</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var habit = await _context.Habits.FindAsync(id);
            if (habit == null) return false;

            _context.Habits.Remove(habit);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
