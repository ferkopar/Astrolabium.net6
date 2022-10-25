
using System.Diagnostics;

namespace Astrolabium.Common
{
    /// <summary>
    ///  A compile-time list of every body we will use in this program
    /// </summary>
    public class Body
    {
       
        public enum Name
        {
            EclNut = -1,
            Sun = 0,
            Moon = 1,
            Mercury = 2,
            Venus = 3,
            Mars = 4,
            Jupiter = 5,
            Saturn = 6,
            Uranus = 7,
            Neptun = 8,
            Pluto = 9,
            MeanNode = 10,
            TrueNode = 11,
            MeanApog = 12,
            OscuApog = 13,
            Earth = 14,
            Chirion = 15,
            Pholus = 16,
            Ceres = 17,
            Pallas = 18,
            Juno = 19,
            Vesta = 20,
            FictOffset = 40,
            NfictElem = 15,
            /* Hamburger or Uranian "planets" */
            Cupido = 40,
            PHades = 41,
            Zeus = 42,
            Kronos = 43,
            Apollon = 44,
            Admetos = 45,
            Vulcanus = 46,
            Poseidon = 47,
            /* other fictitious bodies */
            Isis = 48,
            Nirubu = 49,
            Harrington = 50,
            NeptuneLeverrier = 51,
            NeptuneAdams = 52,
            PlutpLowell = 53,
            PlutoPickering = 54,
            /**/
            /* Asc, Mc */
            Ascendant = 4000,
            MediumCoeli = 4001,
            Other = 21
        }
        public static int ToInt(Name b) => (int)b;
        public static LongitudeAstrolabium ExaltationDegree(Name b)
        {
            Debug.Assert(b >= Name.Sun && b <= Name.Saturn);
            double d = 0;
            switch (b)
            {
                case Name.Sun: d = 10; break;
                case Name.Moon: d = 33; break;
                case Name.Mars: d = 298; break;
                case Name.Mercury: d = 165; break;
                case Name.Jupiter: d = 95; break;
                case Name.Venus: d = 357; break;
                case Name.Saturn: d = 200; break;
            }
            return new LongitudeAstrolabium(d);
        }
        public static LongitudeAstrolabium DebilitationDegree(Name b)
        {
            return ExaltationDegree(b).Add(180);
        }
        public static string ToString(Name b)
        {



            //if (LayoutPersistManager.LoadStateFromRegistry("Culture") == "hu")
            //{
            //    return AstroResources.ResourceManager.GetString(b.ToString());
            //}
            //else
            //{
                return b.ToString();
            //}
            /*  switch (b)
            {
                case Name.Sun: return "Sun";
                case Name.Moon: return "Moon";
                case Name.Mars: return "Mars";
                case Name.Mercury: return "Mercury";
                case Name.Jupiter: return "Jupiter";
                case Name.Venus: return "Venus";
                case Name.Saturn: return "Saturn";
                case Name.Uranus: return "Uranus";
                case Name.Neptun: return "Neptun";
                case Name.Pluto: return "Pluto";
                case Name.TrueNode: return "TrueNode";
                case Name.MeanApog: return "MeanApog";
                case Name.Chirion: return "Chirion";
                case Name.Pholus: return "Pholus";
                case Name.Ceres: return "Ceres";
                case Name.Pallas: return "Pallas";
                case Name.Juno: return "Juno";
                case Name.Vesta: return "Vesta";
                case Name.FictOffset: return "FictOffset";
                //case Name.NfictElem: return "NfictElem";   
            }
            return "";
            */
        }
        public static string ToShortString(Name b)
        {
            switch (b)
            {

                case Name.Sun: return "Su";
                case Name.Moon: return "Mo";
                case Name.Mars: return "Ma";
                case Name.Mercury: return "Me";
                case Name.Jupiter: return "Ju";
                case Name.Venus: return "Ve";
                case Name.Saturn: return "Sa";
                case Name.Neptun: return "Ne";
                case Name.Pluto: return "Pl";
                case Name.TrueNode: return "TN";
                case Name.MeanApog: return "MA";
                case Name.Chirion: return "Ch";
                case Name.Pholus: return "Ph";
                case Name.Ceres: return "Ce";
                case Name.Pallas: return "Pa";
                case Name.Juno: return "Ju";
                case Name.Vesta: return "Ve";
                case Name.FictOffset: return "FOff";
                    //case Name.NfictElem: return "NfEl";   

            }
            Trace.Assert(false, "Basics.Body.toShortString");
            return "   ";
        }

    }
}
