using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FerkopaUtils
{
    public static class DatetimeExtender
    {
        public static DateTime ChangeYear(this DateTime d, int year) => new DateTime(year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
        public static DateTime ChangeMonth(this DateTime d, int month) => new DateTime(d.Year, month, d.Day, d.Hour, d.Minute, d.Second);
        public static DateTime ChangeDay(this DateTime d, int day) => new DateTime(d.Year, d.Month, day, d.Hour, d.Minute, d.Second);
        public static DateTime ChangeHour(this DateTime d, int hour) => new DateTime(d.Year, d.Month, d.Day, hour, d.Minute, d.Second);
        public static DateTime ChangeMinute(this DateTime d, int minute) => new DateTime(d.Year, d.Month, d.Day, d.Hour, minute, d.Second);
        public static DateTime ChangeSecond(this DateTime d, int second) => new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, second);
    }
}
