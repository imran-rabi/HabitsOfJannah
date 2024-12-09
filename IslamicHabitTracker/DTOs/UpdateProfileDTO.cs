using System.ComponentModel.DataAnnotations;

namespace IslamicHabitTracker.DTOs
{
    public class UpdateProfileDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
    }
} 