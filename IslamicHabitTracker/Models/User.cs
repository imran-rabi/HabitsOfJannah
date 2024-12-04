public class User
{
    public int UserID { get; set; } // Primary Key
    public string Name { get; set; } = string.Empty; // Not Null
    public string Email { get; set; } = string.Empty; // Unique, Not Null
    public string Password { get; set; } = string.Empty; // Not Null
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default Timestamp

    // Navigation Properties
    public ICollection<Habit> Habits { get; set; } = new List<Habit>();
    public ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
}
