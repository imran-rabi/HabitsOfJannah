using Microsoft.EntityFrameworkCore;
using IslamicHabitTracker.Models;

namespace IslamicHabitTracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Habit> Habits { get; set; } = null!;
        public DbSet<HabitProgress> HabitProgress { get; set; } = null!;
        public DbSet<Achievement> Achievements { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configure relationships
            modelBuilder.Entity<Habit>()
                .HasOne(h => h.User)
                .WithMany(u => u.Habits)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HabitProgress>()
                .HasOne(p => p.Habit)
                .WithMany(h => h.Progress)
                .HasForeignKey(p => p.HabitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Achievement>()
                .HasOne(a => a.User)
                .WithMany(u => u.Achievements)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
