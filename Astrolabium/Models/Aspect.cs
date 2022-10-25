using System;
using System.Diagnostics;
using SwephCore.Geo;


namespace Astrolabium.Models
{
    public class Aspect
    {
        public IAspectable Body1 { get; }
        public IAspectable Body2 { get; }
        public double Angle{ get; }
        public AspectType AspectType{ get; }
        public Aspect(IAspectable body1,IAspectable body2)
        {
            bool IsRealAspect(AspectType aspectType)
            {
                const int halfOrbis = 2;
                return Angle >= (int)aspectType - halfOrbis && Angle <= (int)aspectType + halfOrbis;
            }        
            Body1 = body1; Body2 = body2;
            Angle = Math.Abs( new Longitude(Body1.Longitude) - new Longitude(Body2.Longitude));
            AspectType = AspectType.Noaspect;
            if (IsRealAspect(AspectType.Conjunction)) AspectType = AspectType.Conjunction;
            else if (IsRealAspect(AspectType.SemiSextil)) AspectType = AspectType.SemiSextil;
            else if (IsRealAspect(AspectType.Sextil)) AspectType = AspectType.Sextil;
            else if (IsRealAspect(AspectType.Quadrat)) AspectType = AspectType.Quadrat;
            else if (IsRealAspect(AspectType.Trigon)) AspectType = AspectType.Trigon;
            else if (IsRealAspect(AspectType.Quinqux)) AspectType = AspectType.Quinqux;
            else if (IsRealAspect(AspectType.Opposition)) AspectType = AspectType.Opposition;
        }

        public string AspectDisplayChar
        {
            get
            {
                if (AspectType == AspectType.Conjunction) return "q";
                if (AspectType == AspectType.SemiSextil) return"i";
                if (AspectType == AspectType.Sextil) return "t";
                if (AspectType == AspectType.Quadrat) return "r";
                if (AspectType == AspectType.Trigon) return "e";
                if (AspectType == AspectType.Quinqux) return "u";
                if (AspectType == AspectType.Opposition) return "w";
                throw new NotImplementedException("implementálatlan fényszög");
            }
        }
        public override string ToString()
        {
            Debug.Assert(Body1 != null, nameof(Body1) + " != null");
            Debug.Assert(Body2 != null, nameof(Body2) + " != null");
            return $"{Body1} in {AspectType.ToString()} with {Body2}";
        }
    }
}

namespace Astrolabium.Models
{
    public enum AspectType
    {
        Conjunction = 0,
        SemiSextil = 30,
        Sextil = 60,
        Quadrat = 45,
        Trigon = 120,
        Quinqux = 150,
        Opposition = 180,
        Noaspect
    }
}