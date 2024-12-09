using System.Security.Claims;

namespace IslamicHabitTracker.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new InvalidOperationException("User ID claim not found");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                throw new InvalidOperationException("Invalid user ID format");

            return userId;
        }
    }
} 