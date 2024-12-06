using IslamicHabitTracker.Helpers.Interfaces;
using System;

namespace IslamicHabitTracker.Helpers
{
    /// <summary>
    /// Helper class for date-related operations
    /// </summary>
    public class DateHelper : IDateHelper
    {
        /// <summary>
        /// Gets the start of the day for a given date
        /// </summary>
        public DateTime GetStartOfDay(DateTime date)
        {
            return date.Date;
        }

        /// <summary>
        /// Gets the end of the day for a given date
        /// </summary>
        public DateTime GetEndOfDay(DateTime date)
        {
            return date.Date.AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Gets the start date of the week for a given date
        /// </summary>
        public DateTime GetStartOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Gets the end date of the week for a given date
        /// </summary>
        public DateTime GetEndOfWeek(DateTime date)
        {
            var startOfWeek = GetStartOfWeek(date);
            return startOfWeek.AddDays(6).Date.AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Checks if a date falls within a given range
        /// </summary>
        public bool IsDateInRange(DateTime date, DateTime startDate, DateTime endDate)
        {
            return date.Date >= startDate.Date && date.Date <= endDate.Date;
        }

        /// <summary>
        /// Gets the number of days between two dates
        /// </summary>
        public int GetDaysBetween(DateTime startDate, DateTime endDate)
        {
            return (int)(endDate.Date - startDate.Date).TotalDays + 1;
        }
    }
}
