using System;

namespace IslamicHabitTracker.Helpers.Interfaces
{
    /// <summary>
    /// Interface for date-related operations
    /// </summary>
    public interface IDateHelper
    {
        /// <summary>
        /// Gets the start of the day for a given date
        /// </summary>
        DateTime GetStartOfDay(DateTime date);

        /// <summary>
        /// Gets the end of the day for a given date
        /// </summary>
        DateTime GetEndOfDay(DateTime date);

        /// <summary>
        /// Gets the start date of the week for a given date
        /// </summary>
        DateTime GetStartOfWeek(DateTime date);

        /// <summary>
        /// Gets the end date of the week for a given date
        /// </summary>
        DateTime GetEndOfWeek(DateTime date);

        /// <summary>
        /// Checks if a date falls within a given range
        /// </summary>
        bool IsDateInRange(DateTime date, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets the number of days between two dates
        /// </summary>
        int GetDaysBetween(DateTime startDate, DateTime endDate);
    }
} 