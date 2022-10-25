using SweCore.Planets;
using Astrolabium.Common;
using SweCore.Geo;
using System.Collections;
using System.Collections.Generic;

namespace Astrolabium.Models
{
    public class PlanetValues:IAspectable
    {
        public Planet Planet { get; set; }
        public string PlanetName { get; set; }
        public HouseValues House { get; set; }
        public double HousePosition { get; set; }
        public LongitudeAstrolabium HouseOffset 
        { 
            get 
            {
                return new LongitudeAstrolabium(HousePosition);
            } 
        }
        public double Longitude { get; set; }
        public LongitudeAstrolabium LongitudeA => new LongitudeAstrolabium(Longitude);
        public ZodiacHouse Sign => LongitudeA.ToZodiacHouse();
        public string SignDisplayString => Sign.GetSignString();
        public string SignLocalisedName => Sign.ToLocalisedString();
        public string OffsetInSign => LongitudeA.ToZodiacHouseOffsetShortStgrin();
        public double Latitude { get; set; }
        public double Distance { get; set; }
        public double LongitudeSpeed { get; set; }
        public double LatitudeSpeed { get; set; }
        public double DistanceSpeed { get; set; }
        /// <summary>
        /// Error in calculation
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Warning in calculation
        /// </summary>
        public string WarnMessage { get; set; }
        public string PlanetString
        { get
            {
                var s = Planet.GetPlanetString();
                if (s == "@") s = PlanetName;
                if (LongitudeSpeed < 0) s += "\u0024";
                return s;
            }
        }
       
        private bool isPlanetInHouse(HouseValues house,PlanetValues planet, List<HouseValues> houses)
        {
            if (house == null) return false;
            if (planet == null) return false;
            LongitudeAstrolabium planetLongitude = new LongitudeAstrolabium(planet.Longitude);
            LongitudeAstrolabium longitudeThis = new LongitudeAstrolabium(house.Cusp);
            LongitudeAstrolabium longitudeNext;
            var index = houses.IndexOf(house);
            if (index == 11)
            {
                longitudeNext = new LongitudeAstrolabium(houses[0].Longitude);
            }
            else
            {
                longitudeNext = new LongitudeAstrolabium(houses[index + 1].Longitude);
            }
            return planetLongitude.IsBetween(longitudeThis, longitudeNext);
            //var index = houses.IndexOf(house);
            //if (index == 11)
            //{
            //    longitudeNext = new LongitudeAstrolabium(houses[0].Longitude);
            //}
            //else 
            //{
            //    longitudeNext = new LongitudeAstrolabium(houses[index+1].Longitude);
            //}
            //if (longitudeThis > longitudeNext)
            //{
            //    if (planetLongitude > longitudeThis
            //        || planetLongitude < longitudeNext) return true;
            //}
            //if(planetLongitude > longitudeThis
            //         && planetLongitude <longitudeNext) return true;
            //return false;
        }
        public void SetHouse(List<HouseValues> houses)
        {
            for (int i = 0; i < houses.Count; i++)
            {
                if( isPlanetInHouse (houses[i],this,houses))
                {
                    House = houses[i];
                    break;
                }
            }
        }
       
        public override string ToString()
        {
            return PlanetName;
        }
    }
}
