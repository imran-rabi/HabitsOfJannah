public class Habit
{
    public int HabitID { get; set; } // Primary Key
    public int UserID { get; set; } // Foreign Key
    public string HabitName { get; set; } = string.Empty; // Not Null
    public string Frequency { get; set; } = "Daily"; // Enum: Daily, Weekly, Monthly
    public TimeSpan? ReminderTime { get; set; } // Nullable
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default Timestamp

    // Navigation Properties
    public User User { get; set; } = null!;
    public ICollection<HabitProgress> HabitProgresses { get; set; } = new List<HabitProgress>();
}
