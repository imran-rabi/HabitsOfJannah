using System.Security.Claims;

namespace IslamicHabitTracker.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new UnauthorizedAccessException("User ID claim not found");

            if (!int.TryParse(claim.Value, out int userId))
                throw new UnauthorizedAccessException("Invalid user ID claim");

            return userId;
        }
    }
} 