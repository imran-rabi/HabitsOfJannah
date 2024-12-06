using System;
using System.ComponentModel.DataAnnotations;


namespace IslamicHabitTracker.DTOs
{
    /// <summary>
    /// DTO for creating a new habit
    /// </summary>
    public class CreateHabitDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty; 

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Daily|Weekly|Monthly|Custom)$")]
        public string Frequency { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Range(1, 31)]
        public int? CustomFrequencyDays { get; set; }

        public string ReminderTime { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Notes { get; set; } = string.Empty;
    }
}
