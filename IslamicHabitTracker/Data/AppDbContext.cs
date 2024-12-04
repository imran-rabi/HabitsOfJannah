using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitProgress> HabitProgress { get; set; }
    public DbSet<Achievement> Achievements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Relationships
        modelBuilder.Entity<Habit>()
            .HasOne(h => h.User)
            .WithMany(u => u.Habits)
            .HasForeignKey(h => h.UserID);

        modelBuilder.Entity<HabitProgress>()
            .HasKey(hp => hp.ProgressID);
            
        modelBuilder.Entity<HabitProgress>()
            .HasOne(hp => hp.Habit)
            .WithMany(h => h.HabitProgresses)
            .HasForeignKey(hp => hp.HabitID); 

        modelBuilder.Entity<Achievement>()
            .HasOne(a => a.User)
            .WithMany(u => u.Achievements)
            .HasForeignKey(a => a.UserID);
    }
}
