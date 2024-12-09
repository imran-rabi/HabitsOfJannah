using IslamicHabitTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using IslamicHabitTracker.Repositories.Interfaces;
using IslamicHabitTracker.Data;

namespace IslamicHabitTracker.Repositories
{
    /// <summary>
    /// Repository for managing User-related database operations
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor that injects the database context
        /// </summary>
        /// <param name="context">The application's database context</param>
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new user in the database
        /// </summary>
        /// <param name="user">The user object to create</param>
        /// <returns>The created user with updated ID</returns>
        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Retrieves a user by their ID
        /// </summary>
        /// <param name="id">The user's ID</param>
        /// <returns>The user if found, null otherwise</returns>
        public async Task<User> GetByIdAsync(int id)
        {
            return await GetByIdAsync(id, false);
        }

        /// <summary>
        /// Retrieves a user by their ID
        /// </summary>
        /// <param name="userId">The user's ID</param>
        /// <param name="includeHabits">Whether to include habits</param>
        /// <returns>The user if found, null otherwise</returns>
        public async Task<User> GetByIdAsync(int userId, bool includeHabits = false)
        {
            var query = _context.Users.AsQueryable();

            if (includeHabits)
            {
                query = query.Include(u => u.Habits);
            }

            return await query.FirstOrDefaultAsync(u => u.Id == userId);
        }

        /// <summary>
        /// Retrieves a user by their email address
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <returns>The user if found, null otherwise</returns>
        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Updates an existing user's information
        /// </summary>
        /// <param name="user">The user object with updated information</param>
        /// <returns>The updated user</returns>
        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// Deletes a user from the database
        /// </summary>
        /// <param name="id">The ID of the user to delete</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
