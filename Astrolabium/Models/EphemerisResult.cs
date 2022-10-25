
using System.Collections.Generic;
using System.Linq;
using SweCore.Date;
using SweCore.Planets;

namespace Astrolabium.Models
{
    /// <summary>
    /// Ephemeris calculations
    /// </summary>
    public class EphemerisResult
    {
        /// <summary>
        /// New result
        /// </summary>
        public EphemerisResult() {
            Planets = new List<PlanetValues>();
            Houses = new List<HouseValues>();
            AsmCs = new List<HouseValues>();
            Aspects = new List<Aspect>();
            AspectsDict = new Dictionary<(PlanetValues, PlanetValues), Aspect>();
        }

        /// <summary>
        /// Reset the result
        /// </summary>
        public void Reset() {
            DateUTC = new DateUt();
            JulianDay = new JulianDay();
            EphemerisTime = new EphemerisTime();
            SideralTime = 0;
            MeanEclipticObliquity = 0;
            TrueEclipticObliquity = 0;
            NutationLongitude = 0;
            NutationObliquity = 0;
            Planets.Clear();
            Houses.Clear();
            AsmCs.Clear();
            Aspects.Clear();
            AspectsDict.Clear();
        }
        public void CalculateAspects()
        {
            for (int i = 0; i < Planets.Count; i++)
            {
                for (int j = i + 1; j < Planets.Count; j++)
                {
                    var aspect = new Aspect(Planets[i], Planets[j]);
                    AspectsDict.Add((Planets[i], Planets[j]),aspect);
                    Aspects.Add(aspect);
                } }
        }

        public PlanetValues this[Planet? toSearh] => Planets.First(p => p.Planet == toSearh);

        /// <summary>
        /// Date UTC
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public DateUt DateUTC { get; set; }

        /// <summary>
        /// Julian Day
        /// </summary>
        public JulianDay JulianDay { get; set; }

        /// <summary>
        /// Ephemeris time
        /// </summary>
        public EphemerisTime EphemerisTime { get; set; }

        /// <summary>
        /// Delat T in seconds
        /// </summary>
        public double DeltaTSec => EphemerisTime.DeltaT * 86400.0;

        /// <summary>
        /// Sideral time
        /// </summary>
        public double SideralTime { get; set; }

        /// <summary>
        /// Sideral time in degrees
        /// </summary>
        public double SideralTimeInDegrees => SideralTime * 15;

        /// <summary>
        /// ARMC : Sideral time in degrees
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public double ARMC => SideralTime * 15;

        /// <summary>
        /// Mean ecliptic obliquity
        /// </summary>
        public double MeanEclipticObliquity { get; set; }

        /// <summary>
        /// True ecliptic obliquity
        /// </summary>
        public double TrueEclipticObliquity { get; set; }

        /// <summary>
        /// Nutation in longitude
        /// </summary>
        public double NutationLongitude { get; set; }

        /// <summary>
        /// Nutation in obliquity
        /// </summary>
        public double NutationObliquity { get; set; }

        /// <summary>
        /// Planets calculation result
        /// </summary>
        public List<PlanetValues> Planets { get; }

        /// <summary>
        /// Houses
        /// </summary>
        public List<HouseValues> Houses { get; }

        /// <summary>
        /// Ascendants, MC etc.
        /// </summary>
        public List<HouseValues> AsmCs { get; }

        public List<Aspect> Aspects { get; }

        public Dictionary<(PlanetValues,PlanetValues),Aspect> AspectsDict { get; }

        public HouseValues Ascendant => AsmCs[0];
    }
}
