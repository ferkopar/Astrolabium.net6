
namespace SweCore.Date
{
    /// <summary>
    /// Represents a Julian Day as Ephemeris Time
    /// </summary>
    public struct EphemerisTime
    {

        /// <summary>
        /// Create a new Ephemeris Time
        /// </summary>
        /// <param name="day">The Julian Day</param>
        /// <param name="deltaT">The DeltaT value</param>
        public EphemerisTime(JulianDay day, double deltaT)
            : this() {
            JulianDay = day;
            DeltaT = deltaT;
            Value = JulianDay.Value + DeltaT;
        }

        /// <summary>
        /// Implicit cast a Ephemeris Time to double
        /// </summary>
        public static implicit operator double(EphemerisTime et) { return et.Value; }

        /// <summary>
        /// Convert to string
        /// </summary>
        public override string ToString() {
            return Value.ToString();
        }

        /// <summary>
        /// The Julian Day
        /// </summary>
        public JulianDay JulianDay { get; }

        /// <summary>
        /// The DelatT
        /// </summary>
        public double DeltaT { get; }

        /// <summary>
        /// The Ephemeris Time value
        /// </summary>
        public double Value { get; }


    }
}
