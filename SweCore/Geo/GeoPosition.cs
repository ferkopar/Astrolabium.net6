using System;

namespace SweCore.Geo
{

    /// <summary>
    /// Geographic position
    /// </summary>
    public class GeoPosition
    {

        /// <summary>
        /// Create a new position
        /// </summary>
        public GeoPosition() {
        }

        /// <summary>
        /// Create a new position
        /// </summary>
        public GeoPosition(Longitude lon, Latitude lat, Double alt) {
            this.Longitude = lon;
            this.Latitude = lat;
            this.Altitude = alt;
        }

        /// <summary>
        /// String value
        /// </summary>
        public override string ToString() {
            return String.Format("{0}, {1}, {2} m", Longitude, Latitude, Altitude);
        }

        /// <summary>
        /// Longitude
        /// </summary>
        public Longitude Longitude { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public Latitude Latitude { get; set; }

        /// <summary>
        /// Altitude in meters
        /// </summary>
        public Double Altitude { get; set; }

    }

}
