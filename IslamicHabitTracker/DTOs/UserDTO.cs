using System;
using System.ComponentModel.DataAnnotations;

namespace IslamicHabitTracker.DTOs
{
    /// <summary>
    /// DTO for user registration and profile updates
    /// </summary>
    public class UserDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;    

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for user login
    /// </summary>
    public class UserLoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO for user profile response
    /// </summary>
    
    /// <summary>
    /// DTO for password change
    /// </summary>
    public class ChangePasswordDTO
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty; 

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
