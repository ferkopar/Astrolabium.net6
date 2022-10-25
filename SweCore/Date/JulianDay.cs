﻿using SweCore;
using System;

namespace SweCore.Date
{

    /// <summary>
    /// Represents a Julian Day in Universal Time
    /// </summary>
    public struct JulianDay
    {

        /// <summary>
        /// Create a new Julian Day from his value
        /// </summary>
        /// <param name="val">The Julian Day value</param>
        public JulianDay(double val, DateCalendar? calendar = null)
            : this() {
            this.Calendar = calendar ?? SweDate.GetCalendar(val);
            this.Value = val;
        }

        /// <summary>
        /// Create a new Julian Day from a DateUt
        /// </summary>
        /// <param name="date">Date source</param>
        /// <param name="calendar">Calendar source</param>
        public JulianDay(DateUt date, DateCalendar? calendar = null)
            : this() {
            this.Calendar = calendar ?? SweDate.GetCalendar(date.Year, date.Month, date.Day);
            this.Value = SweDate.DateToJulianDay(date, Calendar);
        }

        /// <summary>
        /// Returns the DateUt of this Julian Day
        /// </summary>
        public DateUt ToDateUT() {
            return SweDate.JulianDayToDate(Value, Calendar);
        }

        /// <summary>
        /// Retourne the DateTime of this JulianDay
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime() {
            return ToDateUT().ToDateTime();
        }

        /// <summary>
        /// Convert to string
        /// </summary>
        public override string ToString() {
            return Value.ToString();
        }

        /// <summary>
        /// Implicit cast between Julian Day and double
        /// </summary>
        public static implicit operator Double(JulianDay jd) {
            return jd.Value;
        }

        /// <summary>
        /// Calendar
        /// </summary>
        public DateCalendar Calendar { get; private set; }

        /// <summary>
        /// The absolute Julian Day value
        /// </summary>
        public double Value { get; set; }

    }

}
