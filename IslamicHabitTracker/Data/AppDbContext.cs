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

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Password).IsRequired();
            });

            modelBuilder.Entity<Habit>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Frequency).IsRequired();
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.ReminderTime).HasMaxLength(10);
                entity.Property(e => e.TargetValue).HasDefaultValue(1);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Habits)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<HabitProgress>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Value).IsRequired();
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.Mood).HasMaxLength(50);

                entity.HasOne(e => e.Habit)
                    .WithMany(h => h.Progress)
                    .HasForeignKey(e => e.HabitId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
