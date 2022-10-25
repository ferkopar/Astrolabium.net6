using System;
using SweCore.Geo;
using Astrolabium.Sevices;


namespace Astrolabium.Models
{
    public class Location
    {
        private int geoNameId;

        public Location(){}

        public Location(int geoNameId){ 
            GeoNameId = geoNameId;
        }

        #region Public properties
        public int GeoNameId
        {
            get { return geoNameId; }
            private set 
            {
                geoNameId = value;
                var geoName = MainGeoName.GetGeoName(value);
                CityName = geoName.Name;
                CountryName = geoName.CountryName;
                GeoPos = new GeoPosition(new Longitude(geoName.Longitude)
                    , new Latitude(geoName.Latitude)
                    , Convert.ToDouble(geoName.Elevation));
                Timezone = geoName.Timezone;
            }
        }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public GeoPosition GeoPos { get; set; }
        public string Timezone { get; set; }
        public static Location Default { get { return new Location(3047942); }  }
        #endregion
        public override string ToString()
        {
            return CityName;
        }
        public override bool Equals(object obj)
        {
            // If parameter cannot be cast to Location return false.
            Location p = obj as Location;
            if ((object) p == null)
            {
                return false;
            }
            // Return true if the fields match:
            return Equals(p);
        }
        public bool Equals(Location p)
        {
            // If parameter is null return false
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return p.GeoNameId == GeoNameId;
        }
        public override int GetHashCode() => geoNameId % 1000;
        public static bool operator == (Location a, Location b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            // Return true if the fields match:
            return a.GeoNameId == b.GeoNameId;
        }
        public static bool operator != (Location a, Location b)
        {
            return !(a == b);
        }
    }
}
