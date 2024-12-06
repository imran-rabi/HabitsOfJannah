namespace IslamicHabitTracker.Helpers
{
    /// <summary>
    /// Settings for JWT token generation and validation
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Secret key for signing JWT tokens
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Number of days until token expiry
        /// </summary>
        public int ExpiryInDays { get; set; }

        /// <summary>
        /// Token issuer
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Token audience
        /// </summary>
        public string Audience { get; set; } = string.Empty;
    }
} 