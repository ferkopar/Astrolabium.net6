using System;
using System.Diagnostics;

namespace Astrolabium.Common
{/// <summary>
 /// A package related to a ZodiacHouse
 /// </summary>
    public class ZodiacHouse : HoroscopeElement, ICloneable
    {
        public Name Value { get; set; }
        public ZodiacHouse(Name zhouse) => Value = zhouse;
        public enum Name
        {
            Ari = 1, Tau = 2, Gem = 3, Can = 4, Leo = 5, Vir = 6,
            Lib = 7, Sco = 8, Sag = 9, Cap = 10, Aqu = 11, Pis = 12
        }
        public enum RiseType
        {
            RisesWithHead, RisesWithFoot, RisesWithBoth
        }

        // ReSharper disable once UnusedMember.Local
        private static Name[] _allNames = {
            Name.Ari, Name.Tau, Name.Gem, Name.Can, Name.Leo, Name.Vir,
            Name.Lib, Name.Sco, Name.Sag, Name.Cap, Name.Aqu, Name.Pis
        };
        public object Clone()
        {
            return new ZodiacHouse(Value);
        }
        public override string ToString()
        {
           
                return Value.ToString();
        }

        public int Normalize()
        {
            return Basics.Normalize_inc(1, 12, (int)Value);
        }
        public ZodiacHouse Add(int i)
        {
            var znum = Basics.Normalize_inc(1, 12, (int)(Value) + i - 1);
            return new ZodiacHouse((Name)znum);
        }
        public ZodiacHouse AddReverse(int i)
        {
            int znum = Basics.Normalize_inc(1, 12, (int)(Value) - i + 1);
            return new ZodiacHouse((Name)znum);
        }
        public int NumHousesBetweenReverse(ZodiacHouse zrel)
        {
            return Basics.Normalize_inc(1, 12, (14 - NumHousesBetween(zrel)));
        }
        public int NumHousesBetween(ZodiacHouse zrel)
        {
            int ret = Basics.Normalize_inc(1, 12, (int)zrel.Value - (int)Value + 1);
            Trace.Assert(ret >= 1 && ret <= 12, "ZodiacHouse.numHousesBetween failed");
            return ret;
        }
        public bool IsDaySign()
        {
            switch (Value)
            {
                case Name.Ari:
                case Name.Tau:
                case Name.Gem:
                case Name.Can:
                    return false;

                case Name.Leo:
                case Name.Vir:
                case Name.Lib:
                case Name.Sco:
                    return true;

                case Name.Sag:
                case Name.Cap:
                    return false;

                case Name.Aqu:
                case Name.Pis:
                    return true;

                default:
                    Trace.Assert(false, "isDaySign internal error");
                    break;
            }
            return true;
        }
        public bool IsOdd()
        {
            switch (Value)
            {
                case Name.Ari:
                case Name.Gem:
                case Name.Leo:
                case Name.Lib:
                case Name.Sag:
                case Name.Aqu:
                    return true;

                case Name.Tau:
                case Name.Can:
                case Name.Vir:
                case Name.Sco:
                case Name.Cap:
                case Name.Pis:
                    return false;

                default:
                    Trace.Assert(false, "isOdd internal error");
                    break;
                   
            }
            return true;
        }
        public bool IsOddFooted()
        {
            switch (Value)
            {
                case Name.Ari: return true;
                case Name.Tau: return true;
                case Name.Gem: return true;
                case Name.Can: return false;
                case Name.Leo: return false;
                case Name.Vir: return false;
                case Name.Lib: return true;
                case Name.Sco: return true;
                case Name.Sag: return true;
                case Name.Cap: return false;
                case Name.Aqu: return false;
                case Name.Pis: return false;
            }
            Trace.Assert(false, "ZOdiacHouse::isOddFooted");
            return false;
        }
    
        public RiseType RisesWith()
        {
            switch (Value)
            {
                case Name.Ari:
                case Name.Tau:
                case Name.Can:
                case Name.Sag:
                case Name.Cap:
                    return RiseType.RisesWithFoot;
                case Name.Gem:
                case Name.Leo:
                case Name.Vir:
                case Name.Lib:
                case Name.Sco:
                case Name.Aqu:
                    return RiseType.RisesWithHead;
                default:
                    return RiseType.RisesWithBoth;
            }
        }
        public ZodiacHouse LordsOtherSign()
        {
            Name ret = Name.Ari;
            switch (Value)
            {
                case Name.Ari: ret = Name.Sco; break;
                case Name.Tau: ret = Name.Lib; break;
                case Name.Gem: ret = Name.Vir; break;
                case Name.Can: ret = Name.Can; break;
                case Name.Leo: ret = Name.Leo; break;
                case Name.Vir: ret = Name.Gem; break;
                case Name.Lib: ret = Name.Tau; break;
                case Name.Sco: ret = Name.Ari; break;
                case Name.Sag: ret = Name.Pis; break;
                case Name.Cap: ret = Name.Aqu; break;
                case Name.Aqu: ret = Name.Cap; break;
                case Name.Pis: ret = Name.Sag; break;
                default: Debug.Assert(false, "ZodiacHouse::KalachakraMirrorSign"); break;
            }
            return new ZodiacHouse(ret);
        }
        public ZodiacHouse AdarsaSign()
        {
            Name ret = Name.Ari;
            switch (Value)
            {
                case Name.Ari: ret = Name.Sco; break;
                case Name.Tau: ret = Name.Lib; break;
                case Name.Gem: ret = Name.Vir; break;
                case Name.Can: ret = Name.Aqu; break;
                case Name.Leo: ret = Name.Cap; break;
                case Name.Vir: ret = Name.Gem; break;
                case Name.Lib: ret = Name.Tau; break;
                case Name.Sco: ret = Name.Ari; break;
                case Name.Sag: ret = Name.Pis; break;
                case Name.Cap: ret = Name.Leo; break;
                case Name.Aqu: ret = Name.Can; break;
                case Name.Pis: ret = Name.Sag; break;
                default: Debug.Assert(false, "ZodiacHouse::AdarsaSign"); break;
            }
            return new ZodiacHouse(ret);
        }
        public ZodiacHouse AbhimukhaSign()
        {
            Name ret = Name.Ari;
            switch (Value)
            {
                case Name.Ari: ret = Name.Sco; break;
                case Name.Tau: ret = Name.Lib; break;
                case Name.Gem: ret = Name.Sag; break;
                case Name.Can: ret = Name.Aqu; break;
                case Name.Leo: ret = Name.Cap; break;
                case Name.Vir: ret = Name.Pis; break;
                case Name.Lib: ret = Name.Tau; break;
                case Name.Sco: ret = Name.Ari; break;
                case Name.Sag: ret = Name.Gem; break;
                case Name.Cap: ret = Name.Leo; break;
                case Name.Aqu: ret = Name.Can; break;
                case Name.Pis: ret = Name.Vir; break;
                default: Debug.Assert(false, "ZodiacHouse::AbhimukhaSign"); break;
            }
            return new ZodiacHouse(ret);
        }
        public override LongitudeAstrolabium Cusp
        {
            get
            {
                int znum = (int)Value;
                double cusp = (znum - 1) * 30.0;
                return new LongitudeAstrolabium(cusp);
            }
        }
        public static string ToShortString(Name z)
        {
            switch (z)
            {
                case Name.Ari: return "Ar";
                case Name.Tau: return "Ta";
                case Name.Gem: return "Ge";
                case Name.Can: return "Cn";
                case Name.Leo: return "Le";
                case Name.Vir: return "Vi";
                case Name.Lib: return "Li";
                case Name.Sco: return "Sc";
                case Name.Sag: return "Sg";
                case Name.Cap: return "Cp";
                case Name.Aqu: return "Aq";
                case Name.Pis: return "Pi";
                default: return "";
            }
        }

        public string ToLocalisedString()
        {
            switch (Value)
            {
                case Name.Ari: return "Bak";
                case Name.Tau: return "Bika";
                case Name.Gem: return "Ikrek";
                case Name.Can: return "Rák";
                case Name.Leo: return "Oroszlán";
                case Name.Vir: return "Szűz";
                case Name.Lib: return "Mérleg";
                case Name.Sco: return "Skorpió";
                case Name.Sag: return "Nylas";
                case Name.Cap: return "Bak";
                case Name.Aqu: return "Vízöntő";
                case Name.Pis: return "Halak";
                default: return "";
            }
        }
        public string GetSignString()
        {
            string ret = "";
            switch (Value)
            {
                case Name.Ari: ret = "\u0061"; break;
                case Name.Tau: ret = "\u0073"; break;
                case Name.Gem: ret = "\u0064"; break;
                case Name.Can: ret = "\u0066"; break;
                case Name.Leo: ret = "\u0067"; break;
                case Name.Vir: ret = "\u0068"; break;
                case Name.Lib: ret = "\u006a"; break;
                case Name.Sco: ret = "\u006b"; break;
                case Name.Sag: ret = "\u006c"; break;
                case Name.Cap: ret = "\u0076"; break;
                case Name.Aqu: ret = "\u0078"; break;
                case Name.Pis: ret = "\u0063"; break;
            }
            return ret;
        }
    }
}
