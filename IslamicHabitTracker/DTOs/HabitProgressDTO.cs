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
        public DateTime Date { get; set; }

        [Required]
        [Range(0, 100)]
        public int Value { get; set; }

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for updating habit progress
    /// </summary>
    public class UpdateHabitProgressDTO
    {
        [Range(0, 100)]
        public int Value { get; set; }

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;
    }
} 