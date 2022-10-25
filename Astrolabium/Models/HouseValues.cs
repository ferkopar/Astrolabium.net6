using System;
using Astrolabium.Common;

namespace Astrolabium.Models
{
    public class HouseValues:IAspectable
    {
        public int    House { get; set; }
        public string HouseName { get; set; }
        public double Cusp { get; set; }
        public double Longitude { get => Cusp; set => Cusp = value; }
        public string HouseString
        {
            get
            {
                return ((char)(0x2160 + House)).ToString();
            }
        }

        public override string ToString() => $"House{HouseName}";
    }
}
