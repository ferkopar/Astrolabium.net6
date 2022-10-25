using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SweCore;
using SweCore.Date;
using SweCore.Geo;
using SweCore.Planets;

namespace Astrolabium.Models
{
    /// <summary>
    /// Input values for calculation
    /// </summary>
    public class InputCalculation
    {

        public InputCalculation() {
            EphemerisMode = EphemerisMode.SwissEphemeris;
            JplFile = ""; //SwissEph.SE_FNAME_DFT;
            Planets = new List<Planet>();
            DateUT = new DateUt(DateTime.Now);
            Longitude = new Longitude(17, 27, 0, LongitudePolarity.East);
            Latitude = new Latitude(45, 57, 0, LatitudePolarity.North);
            HouseSystem = HouseSystem.Placidus;
            Planets.AddRange(new[] { 
                Planet.Sun, Planet.Moon, Planet.Mercury, Planet.Venus, Planet.Mars, Planet.Jupiter, 
                Planet.Saturn, Planet.Uranus, Planet.Neptune, Planet.Pluto,Planet.Eros, Planet.Sappho,
                 Planet.Psyhe,Planet.Ceres,Planet.Juno, Planet.TrueNode, Planet.Pholus, Planet.Chiron
            });
        }

        public EphemerisMode EphemerisMode { get; set; }

        public GeoPosition Position
        {
            get 
            {
                return new GeoPosition(Longitude, Latitude, 0);
            }
            set
            {
                Longitude = value.Longitude;
                Latitude = value.Latitude;
            }
        }
        public DateTime DateLocal { 
            get { return DateTime.MinValue; }
            set 
            {
               DateUT = new DateUt(value.ToUniversalTime());
            }
        }
        public string JplFile { get; set; }

        public DateUt? DateUT { get; set; }

        public DateUt? DateET { get; set; }

        public JulianDay? JulianDay { get; set; }

        public PositionCenter PositionCenter { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public Latitude Latitude { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public Longitude Longitude { get; set; }

        /// <summary>
        /// Altitude
        /// </summary>
        public int Altitude { get; set; }

        /// <summary>
        /// House system
        /// </summary>
        public HouseSystem HouseSystem { get; set; }

        /// <summary>
        /// Planets to calculate
        /// </summary>
        public List<Planet> Planets { get; private set; }



    }
}
