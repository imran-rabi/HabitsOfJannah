public class HabitProgress
{
    public int ProgressID { get; set; } // Primary Key
    public int HabitID { get; set; } // Foreign Key
    public DateTime Date { get; set; } // Not Null
    public bool Status { get; set; } = false; // Default: False

    // Navigation Properties
    public Habit Habit { get; set; } = null!;
}
