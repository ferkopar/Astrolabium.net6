using System;
using System.CodeDom;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Media.Animation;
using static Astrolabium.Common.Basics;


namespace Astrolabium.Common
{
    [Serializable]
    [TypeConverter(typeof(LongitudeConverter))]
    public class LongitudeAstrolabium
    {
        private double _mLon;
        public bool Retrograd { get; set; }
        public LongitudeAstrolabium(double lon, bool retrograd) : this(lon)
        {
            Retrograd = retrograd;
        }
        public LongitudeAstrolabium(double lon)
        {
            while (lon > 360.0) lon -= 360.0;
            while (lon < 0) lon += 360.0;
            _mLon = lon;
            _mLon = NormalizeExc(0, 360, lon);
            Retrograd = false;

        }
        public double Value
        {
            get => _mLon;
            set
            {
                Trace.Assert(value >= 0 && value <= 360);
                _mLon = value;
            }
        }
        public LongitudeAstrolabium Add(LongitudeAstrolabium b)
        {
            return new LongitudeAstrolabium(NormalizeExcLower(0, 360, Value + b.Value));
        }
        public LongitudeAstrolabium Add(double b)
        {
            return Add(new LongitudeAstrolabium(b));
        }
        public LongitudeAstrolabium Sub(LongitudeAstrolabium b)
        {
            return new LongitudeAstrolabium(NormalizeExcLower(0, 360, Value - b.Value));
        }
        public LongitudeAstrolabium Sub(double b) => Sub(new LongitudeAstrolabium(b));
        public double Normalize() => NormalizeExcLower(0, 360, Value);
        public bool IsBetween(LongitudeAstrolabium cuspLower, LongitudeAstrolabium cuspHigher)
        {
            var diff1 = Sub(cuspLower).Value;
            var diff2 = Sub(cuspHigher).Value;

            var bRet = (cuspHigher.Sub(cuspLower).Value <= 180) && diff1 <= diff2;

            Console.WriteLine(@"Is it true that {0} < {1} < {2}? {3}", this, cuspLower, cuspHigher, bRet);
            return bRet;
        }
        public static LongitudeAstrolabium operator +(LongitudeAstrolabium l1, LongitudeAstrolabium l2)
        {
            return l1.Add(l2);
        }
        public static LongitudeAstrolabium operator +(LongitudeAstrolabium l1, double angle)
        {
            return l1.Add(angle);
        }
        public static LongitudeAstrolabium operator -(LongitudeAstrolabium l1, LongitudeAstrolabium l2)
        {
            return l1.Sub(l2);
        }
        public static LongitudeAstrolabium operator -(LongitudeAstrolabium l1, double angle)
        {
            return l1.Sub( angle);
        }
        public static bool operator <(LongitudeAstrolabium l1,LongitudeAstrolabium l2)
        {
            return l1.Value < l2.Value;
        }
        public static bool operator >(LongitudeAstrolabium l1, LongitudeAstrolabium l2)
        {
            return l1.Value > l2.Value;
        }
        /// <summary>
        /// A két hosszúság felezőpontját adja vissza
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public static LongitudeAstrolabium operator %(LongitudeAstrolabium l1, LongitudeAstrolabium l2)
        {
            return l1- new LongitudeAstrolabium((l1 - l2).Value / 2);
        }

        public ZodiacHouse ToZodiacHouse()
        {
            return new ZodiacHouse((ZodiacHouse.Name)(int)(Math.Floor(Value / 30.0) + 1.0));
        }
        public double ToZodiacHouseBase()
        {
            var znum = (int)(ToZodiacHouse().Value);
            var cusp = (znum - 1) * 30.0;
            return cusp;
        }
        public double ToZodiacHouseOffset()
        {
            var znum = (int)(ToZodiacHouse().Value);
            var cusp = (znum - 1) * 30.0;
            var ret = Value - cusp;
            Trace.Assert(ret >= 0.0 && ret <= 30.0);
            return ret;
        }
         
        public string ZodiacHouseOffsetShortStgring
        {
            get { return ToZodiacHouseOffsetShortStgrin(); }
        }

        public string ToZodiacHouseOffsetShortStgrin()
        {
            var lon = this;
            var rasi = lon.ToZodiacHouse().ToString();
            var offset = lon.ToZodiacHouseOffset();
            var minutes = Math.Floor(offset);
            offset = (offset - minutes) * 60.0;
            var seconds = Math.Floor(offset);
            offset = (offset - seconds) * 60.0;
            var subsecs = Math.Floor(offset);
            return $"{minutes}°{seconds}´ ";
        }

        public double PercentageOfZodiacHouse()
        {
            var offset = ToZodiacHouseOffset();
            var perc = offset / 30.0 * 100;
            Trace.Assert(perc >= 0 && perc <= 100);
            return perc;
        }

        public override string ToString()
        {
            var lon = this;
            var rasi = lon.ToZodiacHouse().ToString();
            var offset = lon.ToZodiacHouseOffset();
            var minutes = Math.Floor(offset);
            offset = (offset - minutes) * 60.0;
            var seconds = Math.Floor(offset);
            offset = (offset - seconds) * 60.0;
            var subsecs = Math.Floor(offset);
            return $"{rasi} {minutes}°{seconds}´{subsecs}˝ ";
        }
    }
        /// <summary>
        /// A package of longitude related functions. These are useful enough that
        /// I have justified using an object instead of a simple double value type
        /// </summary>
        /// 
        internal class LongitudeConverter : ExpandableObjectConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type t) => t == typeof(string) || base.CanConvertFrom(context, t);

            public override object ConvertFrom(
                ITypeDescriptorContext context,
                CultureInfo info,
                object value)
            {
                Trace.Assert(value is string, "LongitudeConverter::ConvertFrom 1");
                var s = (string)value;

                var arr = s.Split('.', ' ', ':');

                double lonValue = 0;
                if (arr.Length >= 1) lonValue = int.Parse(arr[0]);
                if (arr.Length >= 2)
                {
                    switch (arr[1].ToLower())
                    {
                        case "ari": lonValue += 0.0; break;
                        case "tau": lonValue += 30.0; break;
                        case "gem": lonValue += 60.0; break;
                        case "can": lonValue += 90.0; break;
                        case "leo": lonValue += 120.0; break;
                        case "vir": lonValue += 150.0; break;
                        case "lib": lonValue += 180.0; break;
                        case "sco": lonValue += 210.0; break;
                        case "sag": lonValue += 240.0; break;
                        case "cap": lonValue += 270.0; break;
                        case "aqu": lonValue += 300.0; break;
                        case "pis": lonValue += 330.0; break;
                    }
                }
                double divider = 60;
                for (var i = 2; i < arr.Length; i++)
                {
                    lonValue += (double.Parse(arr[i]) / divider);
                    divider *= 60.0;
                }

                return new LongitudeAstrolabium(lonValue);
            }
            public override object ConvertTo(
                ITypeDescriptorContext context,
                CultureInfo culture,
                object value,
                Type destType)
            {
                Trace.Assert(destType == typeof(string) && value is LongitudeAstrolabium, "Longitude::ConvertTo 1");
                var lon = (LongitudeAstrolabium)value;
                return lon.ToString();
            }
        }
    }
