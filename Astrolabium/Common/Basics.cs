using System;
using System.Diagnostics;

namespace Astrolabium.Common
{
    /// <summary>
    /// Simple functions that don't belong anywhere else
    /// </summary>
    public static class Basics
    {
#pragma warning disable 1570
        /// <summary>
        /// Normalize a number between bounds
        /// </summary>
        /// <param name="lower">The lower bound of normalization</param>
        /// <param name="upper">The upper bound of normalization</param>
        /// <param name="x">The value to be normalized</param>
        /// <returns>The normalized value of x, where lower <= x <= upper </returns>
        public static int Normalize_inc(int lower, int upper, int x)
        {
            int size = upper - lower + 1;
            while (x > upper) x -= size;
            while (x < lower) x += size;
            Trace.Assert(x >= lower && x <= upper, "Basics.normalize failed");
            return x;
        }

        /// <summary>
        /// Normalize a number between bounds
        /// </summary>
        /// <param name="lower">The lower bound of normalization</param>
        /// <param name="upper">The upper bound of normalization</param>
        /// <param name="x">The value to be normalized</param>
        /// <returns>The normalized value of x, where lower = x <= upper </returns>
#pragma warning restore 1570
        public static double NormalizeExc(double lower, double upper, double x)
        {
            double size = upper - lower;
            while (x > upper) x -= size;
            while (x <= lower) x += size;
            Trace.Assert(x >= lower && x <= upper, "Basics.normalize failed");
            return x;
        }

        public static double NormalizeExcLower(double lower, double upper, double x)
        {
            double size = upper - lower;
            while (x >= upper) x -= size;
            while (x < lower) x += size;
            Trace.Assert(x >= lower && x <= upper, "Basics.normalize failed");
            return x;
        }

        public static Weekday BodyToWeekday(Body.Name b)
        {
            switch (b)
            {
                case Body.Name.Sun: return Weekday.Sunday;
                case Body.Name.Moon: return Weekday.Monday;
                case Body.Name.Mars: return Weekday.Tuesday;
                case Body.Name.Mercury: return Weekday.Wednesday;
                case Body.Name.Jupiter: return Weekday.Thursday;
                case Body.Name.Venus: return Weekday.Friday;
                case Body.Name.Saturn: return Weekday.Saturday;
            }
            Debug.Assert(false, $"bodyToWeekday({b})");
            throw new Exception();
        }
        public static Body.Name WeekdayRuler(Weekday w)
        {
            switch (w)
            {
                case Weekday.Sunday: return Body.Name.Sun;
                case Weekday.Monday: return Body.Name.Moon;
                case Weekday.Tuesday: return Body.Name.Mars;
                case Weekday.Wednesday: return Body.Name.Mercury;
                case Weekday.Thursday: return Body.Name.Jupiter;
                case Weekday.Friday: return Body.Name.Venus;
                case Weekday.Saturday: return Body.Name.Saturn;
                default:
                    Debug.Assert(false, "Basics::weekdayRuler");
                    break;
            }
            return Body.Name.Sun;
        }

        // This matches the sweph definitions for easy conversion
        public enum Weekday
        {
            Monday = 0, Tuesday = 1, Wednesday = 2, Thursday = 3, Friday = 4, Saturday = 5, Sunday = 6
        }

        public static string WeekdayToShortString(Weekday w)
        {
            switch (w)
            {
                case Weekday.Sunday: return "Su";
                case Weekday.Monday: return "Mo";
                case Weekday.Tuesday: return "Tu";
                case Weekday.Wednesday: return "We";
                case Weekday.Thursday: return "Th";
                case Weekday.Friday: return "Fr";
                case Weekday.Saturday: return "Sa";
            }
            return "";
        }


 
        /// <summary>
        /// Specify the Lord of a ZodiacHouse. The owernership of the nodes is not considered
        /// </summary>
        /// <param name="zh">The House whose lord should be returned</param>
        /// <returns>The lord of zh</returns>
        public static Body.Name SimpleLordOfZodiacHouse(ZodiacHouse.Name zh)
        {
            switch (zh)
            {
                case ZodiacHouse.Name.Ari: return Body.Name.Mars;
                case ZodiacHouse.Name.Tau: return Body.Name.Venus;
                case ZodiacHouse.Name.Gem: return Body.Name.Mercury;
                case ZodiacHouse.Name.Can: return Body.Name.Moon;
                case ZodiacHouse.Name.Leo: return Body.Name.Sun;
                case ZodiacHouse.Name.Vir: return Body.Name.Mercury;
                case ZodiacHouse.Name.Lib: return Body.Name.Venus;
                case ZodiacHouse.Name.Sco: return Body.Name.Mars;
                case ZodiacHouse.Name.Sag: return Body.Name.Jupiter;
                case ZodiacHouse.Name.Cap: return Body.Name.Saturn;
                case ZodiacHouse.Name.Aqu: return Body.Name.Saturn;
                case ZodiacHouse.Name.Pis: return Body.Name.Jupiter;
            }

            Trace.Assert(false,
                $"Basics.SimpleLordOfZodiacHouse for {(int) zh} failed");
            return Body.Name.Other;
        }
    }

}
