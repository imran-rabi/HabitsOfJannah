using System;
using System.ComponentModel.DataAnnotations;

namespace IslamicHabitTracker.DTOs
{
    /// <summary>
    /// DTO for recording habit progress
    /// </summary>
    public class HabitProgressDTO
    {
        [Required]
        public int HabitId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Type { get; set; } = "completion";

        [Required]
        [Range(0, 100)]
        public int Value { get; set; }

        public string? Notes { get; set; }

        public string? Mood { get; set; }
    }

    /// <summary>
    /// DTO for updating habit progress
    /// </summary>
    public class UpdateHabitProgressDTO
    {
        public int Value { get; set; }
        public string? Notes { get; set; }
    }

    public class CalendarDayDTO
    {
        public DateTime Date { get; set; }
        public bool HasProgress { get; set; }
        public int Value { get; set; }
        public string? Notes { get; set; }
    }
} 