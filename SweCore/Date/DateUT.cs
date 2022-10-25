using System;
using System.Globalization;
using System.Text;

namespace SweCore.Date
{
    /// <summary>
    /// Date Universal Time
    /// </summary>
    public struct DateUt : IComparable, IComparable<DateUt>, IEquatable<DateUt>, IFormattable
    {
        private readonly int _year;
        private readonly int _month;
        private readonly int _day;
        private readonly int _hours;
        private readonly int _minutes;
        private readonly int _seconds;
        private const string DefaultFormat = "dd/MM/yyyy HH:mm:ss";

        /// <summary>
        /// New date from components
        /// </summary>
        public DateUt(int year, int month, int day, int hours, int minutes, int seconds)
            : this()
        {
            var jd = SweDate.DateToJulianDay(year, month, day, hours, minutes, seconds, DateCalendar.Julian);
            SweDate.JulianDayToDate(jd, DateCalendar.Julian, out _year, out _month, out _day, out _hours, out _minutes, out _seconds);
        }

        /// <summary>
        /// New date from components
        /// </summary>
        public DateUt(int year, int month, int day, double hour)
            : this()
        {
            var jd = SweDate.DateToJulianDay(year, month, day, hour, DateCalendar.Julian);
            SweDate.JulianDayToDate(jd, DateCalendar.Julian, out _year, out _month, out _day, out _hours, out _minutes, out _seconds);
        }

        /// <summary>
        /// New date from DateTime
        /// </summary>
        public DateUt(DateTime date)
            : this(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second)
        {
        }

        /// <summary>
        /// New date from DateTimeOffset
        /// </summary>
        /// <param name="date"></param>
        public DateUt(DateTimeOffset date)
            : this(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second)
        {
        }

        /// <summary>
        /// Compare with an object
        /// </summary>
        public int CompareTo(object obj)
        {
            switch (obj)
            {
                case DateUt _:
                    return CompareTo((DateUt)obj);

                case DateTime _:
                    return CompareTo(new DateUt((DateTime)obj));

                case DateTimeOffset _:
                    return CompareTo(new DateUt((DateTimeOffset)obj));
            }
            return -1;
        }

        /// <summary>
        /// Compare with another DateUt
        /// </summary>
        public int CompareTo(DateUt other)
        {
            int result;
            if ((result = Year - other.Year) != 0) return result;
            if ((result = Month - other.Month) != 0) return result;
            if ((result = Day - other.Day) != 0) return result;
            if ((result = Hours - other.Hours) != 0) return result;
            if ((result = Minutes - other.Minutes) != 0) return result;
            if ((result = Seconds - other.Seconds) != 0) return result;
            return result;
        }

        /// <summary>
        /// Compare two dates
        /// </summary>
        public static int Compare(DateUt date1, DateUt date2)
        {
            return date1.CompareTo(date2);
        }

        /// <summary>
        /// Test equality
        /// </summary>
        public bool Equals(DateUt other)
        {
            return CompareTo(other) == 0;
        }

        /// <summary>
        /// Test equality with another object
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is DateUt) return Equals((DateUt)obj);
            return base.Equals(obj);
        }

        /// <summary>
        /// Get HashCode
        /// </summary>
        public override int GetHashCode()
        {
            return this.Year.GetHashCode()
                ^ this.Month.GetHashCode()
                ^ this.Day.GetHashCode()
                ^ this.Hours.GetHashCode()
                ^ this.Minutes.GetHashCode()
                ^ this.Seconds.GetHashCode()
                ;
        }

        /// <summary>
        /// To string
        /// </summary>
        public override string ToString()
        {
            return this.ToString(DefaultFormat);
        }

        /// <summary>
        /// To string with format
        /// </summary>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// To string with format
        /// </summary>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrWhiteSpace(format)) format = DefaultFormat;
            var dfi = (formatProvider ?? CultureInfo.CurrentCulture).GetFormat(typeof(DateTimeFormatInfo)) as DateTimeFormatInfo;
            dfi = dfi ?? CultureInfo.CurrentCulture.DateTimeFormat;
            var result = new StringBuilder();
            int cnt = 0, fl = format.Length;
            for (int i = 0; i < fl; i++)
            {
                char c = format[i];
                switch (c)
                {
                    case '\\':
                        i++;
                        if (i < format.Length)
                        {
                            result.Append(format[i]);
                        }
                        else
                        {
                            result.Append('\\');
                        }
                        break;

                    case 'd':
                        cnt = 0;
                        while (i < fl && format[i] == 'd') { cnt++; i++; }
                        i--;
                        var jd = SweDate.DateToJulianDay(this);
                        int nd = ((int)SweDate.DayOfWeek(jd)) + 1;
                        if (nd >= 7) nd -= 7;
                        if (cnt == 1) result.Append(this.Day);
                        else if (cnt == 2) result.Append(this.Day.ToString("D2"));
                        else if (cnt == 3) result.Append(dfi.AbbreviatedDayNames[nd]);
                        else result.Append(dfi.DayNames[nd]);
                        break;

                    case 'M':
                        cnt = 0;
                        while (i < fl && format[i] == 'M') { cnt++; i++; }
                        i--;
                        if (cnt == 1) result.Append(this.Month);
                        else if (cnt == 2) result.Append(this.Month.ToString("D2"));
                        else if (cnt == 3) result.Append(dfi.AbbreviatedMonthNames[this.Month - 1]);
                        else result.Append(dfi.MonthNames[this.Month - 1]);
                        break;

                    case 'y':
                        cnt = 0;
                        while (i < fl && format[i] == 'y') { cnt++; i++; }
                        i--;
                        if (cnt == 1) result.Append(this.Year % 100);
                        else if (cnt == 2) result.Append((this.Year % 100).ToString("D2"));
                        else result.Append(this.Year);
                        break;

                    case 'h':
                        cnt = 0;
                        while (i < fl && format[i] == 'h') { cnt++; i++; }
                        i--;
                        if (cnt == 1) result.Append(this.Hours % 12);
                        else result.Append((this.Hours % 12).ToString("D2"));
                        break;

                    case 'H':
                        cnt = 0;
                        while (i < fl && format[i] == 'H') { cnt++; i++; }
                        i--;
                        if (cnt == 1) result.Append(this.Hours);
                        else result.Append(this.Hours.ToString("D2"));
                        break;

                    case 'm':
                        cnt = 0;
                        while (i < fl && format[i] == 'm') { cnt++; i++; }
                        i--;
                        if (cnt == 1) result.Append(this.Minutes);
                        else result.Append(this.Minutes.ToString("D2"));
                        break;

                    case 's':
                        cnt = 0;
                        while (i < fl && format[i] == 's') { cnt++; i++; }
                        i--;
                        if (cnt == 1) result.Append(this.Seconds);
                        else result.Append(this.Seconds.ToString("D2"));
                        break;

                    case 't':
                        cnt = 0;
                        while (i < fl && format[i] == 't') { cnt++; i++; }
                        i--;
                        var des = this.Hours < 12 ? dfi.AMDesignator : dfi.PMDesignator;
                        if (cnt == 1) result.Append(des[0]);
                        else result.Append(des);
                        break;
                    //case '/':
                    //    result.Append(DateTime.MinValue.ToString("/"));
                    //    break;
                    default:
                        result.Append(c);
                        break;
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Convert to a DateTime
        /// </summary>
        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day, Hours, Minutes, Seconds);
        }

        /// <summary>
        /// Convert to a DateTimeOffset
        /// </summary>
        public DateTimeOffset ToDateTimeOffset()
        {
            return new DateTimeOffset(Year, Month, Day, Hours, Minutes, Seconds, TimeSpan.Zero);
        }

        /// <summary>
        /// Operator ==
        /// </summary>
        public static bool operator ==(DateUt date1, DateUt date2)
        {
            return Compare(date1, date2) == 0;
        }

        /// <summary>
        /// Operator !=
        /// </summary>
        public static bool operator !=(DateUt date1, DateUt date2)
        {
            return Compare(date1, date2) != 0;
        }

        /// <summary>
        /// Operator &lt;
        /// </summary>
        public static bool operator <(DateUt date1, DateUt date2)
        {
            return Compare(date1, date2) < 0;
        }

        /// <summary>
        /// Operator &gt;
        /// </summary>
        public static bool operator >(DateUt date1, DateUt date2)
        {
            return Compare(date1, date2) > 0;
        }

        /// <summary>
        /// Operator &lt;=
        /// </summary>
        public static bool operator <=(DateUt date1, DateUt date2)
        {
            return Compare(date1, date2) <= 0;
        }

        /// <summary>
        /// Operator &gt;=
        /// </summary>
        public static bool operator >=(DateUt date1, DateUt date2)
        {
            return Compare(date1, date2) >= 0;
        }

        /// <summary>
        /// Add offset
        /// </summary>
        public static DateUt operator +(DateUt date, TimeSpan offset)
        {
            var jd = SweDate.DateToJulianDay(date, DateCalendar.Gregorian);
            jd += offset.TotalDays;
            return SweDate.JulianDayToDate(jd, DateCalendar.Gregorian);
        }

        /// <summary>
        /// Sub offset
        /// </summary>
        public static DateUt operator -(DateUt date, TimeSpan offset)
        {
            var jd = SweDate.DateToJulianDay(date, DateCalendar.Gregorian);
            jd -= offset.TotalDays;
            return SweDate.JulianDayToDate(jd, DateCalendar.Gregorian);
        }

        /// <summary>
        /// Day
        /// </summary>
        public int Day => _day;

        /// <summary>
        /// Month
        /// </summary>
        public int Month => _month;

        /// <summary>
        /// Year
        /// </summary>
        public int Year => _year;

        /// <summary>
        /// Hours
        /// </summary>
        public int Hours => _hours;

        /// <summary>
        /// Minutes
        /// </summary>
        public int Minutes => _minutes;

        /// <summary>
        /// Seconds
        /// </summary>
        public int Seconds => _seconds;
    }
}