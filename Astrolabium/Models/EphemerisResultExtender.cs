using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SweCore.Planets;
using Astrolabium.Common;

namespace Astrolabium.Models
{
    public static class EphemerisResultExtender
    {

        public static SignValues GetPlanetInSign(this EphemerisResult result, Planet planet)
        {
            SweCore.Planets.Planet p = new SweCore.Planets.Planet(planet.Id);
            var x = result[p];
            return new Signs()[result[p].Longitude];
        }

        public static HouseValues GetPlanetInHouse(this EphemerisResult result, Planet planet)
        {
            SweCore.Planets.Planet p = new SweCore.Planets.Planet(planet.Id);
            var houses = result.Houses;
            var planetLongitude = new LongitudeAstrolabium(result[p].Longitude);
            if (planetLongitude.IsBetween(new LongitudeAstrolabium(houses[11].Longitude),
                new LongitudeAstrolabium(houses[0].Longitude)))
            {
                return houses[11];
            }
            for (int i = 0; i < houses.Count-1; i++)
            {
                if (planetLongitude.IsBetween(new LongitudeAstrolabium(houses[i].Longitude),
                    new LongitudeAstrolabium(houses[i+1].Longitude)))
                {
                    return houses[i];
                }
            }
            return null;
        }
    }
}
