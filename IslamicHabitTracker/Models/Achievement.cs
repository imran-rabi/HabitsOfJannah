public class Achievement
{
    public int AchievementID { get; set; } // Primary Key
    public int UserID { get; set; } // Foreign Key
    public string Description { get; set; } = string.Empty; // Not Null
    public DateTime AchievedAt { get; set; } = DateTime.UtcNow; // Default Timestamp

    // Navigation Properties
    public User User { get; set; } = null!;
}
