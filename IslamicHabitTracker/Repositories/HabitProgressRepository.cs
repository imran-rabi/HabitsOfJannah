using IslamicHabitTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IslamicHabitTracker.Repositories.Interfaces;
using IslamicHabitTracker.Data;

namespace IslamicHabitTracker.Repositories
{
    /// <summary>
    /// Repository for managing HabitProgress-related database operations
    /// </summary>
    public class HabitProgressRepository : IHabitProgressRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor that injects the database context
        /// </summary>
        /// <param name="context">The application's database context</param>
        public HabitProgressRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Records progress for a habit
        /// </summary>
        /// <param name="progress">The progress object to create</param>
        /// <returns>The created progress entry</returns>
        public async Task<HabitProgress> CreateAsync(HabitProgress progress)
        {
            _context.HabitProgress.Add(progress);
            await _context.SaveChangesAsync();
            return progress;
        }

        /// <summary>
        /// Retrieves progress for a specific habit within a date range
        /// </summary>
        /// <param name="habitId">The habit's ID</param>
        /// <param name="startDate">Start date of the range</param>
        /// <param name="endDate">End date of the range</param>
        /// <returns>List of progress entries within the date range</returns>
        public async Task<List<HabitProgress>> GetProgressByDateRangeAsync(
            int habitId, 
            DateTime startDate, 
            DateTime endDate)
        {
            return await _context.HabitProgress
                .Where(p => p.HabitId == habitId 
                    && p.Date.Date >= startDate.Date 
                    && p.Date.Date <= endDate.Date)
                .OrderBy(p => p.Date)
                .ToListAsync();
        }

        /// <summary>
        /// Updates an existing progress entry
        /// </summary>
        /// <param name="progress">The progress object with updated information</param>
        /// <returns>The updated progress entry</returns>
        public async Task<HabitProgress> UpdateAsync(HabitProgress progress)
        {
            _context.HabitProgress.Update(progress);
            await _context.SaveChangesAsync();
            return progress;
        }

        /// <summary>
        /// Retrieves progress for a specific date and habit
        /// </summary>
        /// <param name="habitId">The habit's ID</param>
        /// <param name="date">The specific date</param>
        /// <returns>The progress entry if found, null otherwise</returns>
        public async Task<HabitProgress> GetByDateAsync(int habitId, DateTime date)
        {
            return await _context.HabitProgress
                .FirstOrDefaultAsync(p => p.HabitId == habitId
                    && p.Date.Date == date.Date);
        }

        /// <summary>
        /// Deletes a progress entry
        /// </summary>
        /// <param name="id">The ID of the progress entry to delete</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var progress = await _context.HabitProgress.FindAsync(id);
            if (progress == null) return false;

            _context.HabitProgress.Remove(progress);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves a progress entry by its ID
        /// </summary>
        /// <param name="progressId">The ID of the progress entry to retrieve</param>
        /// <returns>The progress entry if found, null otherwise</returns>
        public async Task<HabitProgress> GetByIdAsync(int progressId)
        {
            return await _context.HabitProgress
                .Include(p => p.Habit)
                .FirstOrDefaultAsync(p => p.Id == progressId);
        }

        /// <summary>
        /// Retrieves progress history for a specific habit
        /// </summary>
        /// <param name="habitId">The habit's ID</param>
        /// <returns>List of progress entries for the habit</returns>
        public async Task<List<HabitProgress>> GetProgressHistoryAsync(int habitId)
        {
            return await _context.HabitProgress
                .Where(p => p.HabitId == habitId)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all progress entries for a specific habit
        /// </summary>
        /// <param name="habitId">The habit's ID</param>
        /// <returns>List of progress entries for the habit</returns>
        public async Task<List<HabitProgress>> GetAllForHabitAsync(int habitId)
        {
            return await _context.HabitProgress
                .Where(p => p.HabitId == habitId)
                .OrderBy(p => p.Date)
                .ToListAsync();
        }
    }
}
