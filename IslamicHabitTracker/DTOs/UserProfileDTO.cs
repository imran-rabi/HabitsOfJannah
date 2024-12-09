using System;

namespace IslamicHabitTracker.DTOs
{
    public class UserProfileDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        public int TotalHabits { get; set; }
        public int ActiveHabits { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
} 