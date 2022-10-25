using SweCore.Planets;
using System.Collections.Generic;
namespace Astrolabium.Helpers
{

    public static class PlanetHelper
    {
        private static Dictionary<Planet,(string,string)> planetStringDictionry = new Dictionary<Planet, (string, string)>
        {
            {Planet.Sun,("\u0051","Nap") },
            {Planet.Moon,("\u0057","Hold" )},
            {Planet.Mercury,("\u0045","Merkúr") },
            {Planet.Venus,("\u0052","Vénusz") },
            {Planet.Mars,("\u0054","Mars")},
            {Planet.Jupiter,("\u0059","Jupiter")},
            {Planet.Saturn,("\u0055","Szaturnusz") },
            {Planet.Uranus,("\u0049","Uránusz") },
            {Planet.Neptune,("\u004f","Neptunusz") },
            {Planet.Pluto,("\u0050","Plútó") },
            {Planet.TrueNode,("\u007b","TrueNode") },
            {Planet.Eros,("\u00aa","Erosz")},
            {Planet.Sappho,("\u00b3","Szaphó") },
            {Planet.Psyhe,("\u00ab","Pszihé" )},
            {Planet.Ceres,("\u0043","Ceresz") },
            {Planet.Juno,("\u0042","Júnó") },
            {Planet.Pholus,("\u00ac","Fallosz") },
            {Planet.Chiron,("\u004d","Círion") }
        };
        
        public static string GetPlanetString(this Planet planet)
        {
            if (planetStringDictionry.ContainsKey(planet))
            {
                return planetStringDictionry[planet].Item1;
            }
            return "@";
        }

        public static string GetPlanetName(this Planet planet)
        {
            if (planetStringDictionry.ContainsKey(planet))
            {
                return planetStringDictionry[planet].Item2;
            }
            return "@";
        }
    }
}
