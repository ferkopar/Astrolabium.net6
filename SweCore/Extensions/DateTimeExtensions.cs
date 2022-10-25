﻿using System;

namespace SweCore.Extensions
{
    /// <summary>
    /// Extension methods for DateTime
    /// </summary>
    public static class DateTimeExtensions
    {

        /// <summary>
        /// Return the hour value of a DateTime
        /// </summary>
        /// <remarks>
        /// The hour value is the time as hours and minutes and secons as decimal part.
        /// </remarks>
        public static double GetHourValue(this DateTime date) {
            if (date == null) return 0.0;
            return (Double)date.Hour + (date.Minute / 60.0) + (date.Second / 3600.0);
        }

    }
}
