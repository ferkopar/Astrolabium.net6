
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace FerkopaUtils
{
    public static class StringExtender
    {
        /// <summary>
        /// A megadott string első karakterét nagybetűssé, a többi karakter kisbetűssé alakítja.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string StringToCapital(this string s)
        {
            if (String.IsNullOrEmpty(s)) return String.Empty;
            var sb = new StringBuilder(s.ToLower());
            sb[0] = Char.ToUpper(sb[0]);
            return sb.ToString();
        }

        public static bool IsEmpty(this string s)
        {
            return string.IsNullOrWhiteSpace(s);           
        }

        public static bool IsBeginWith(this String s, String b)
        {
            if (s.Length >= b.Length &&
                s.Substring(0, b.Length) == b) return true;
            return false;
        }

        public static String DropLastCharIfMatch(this String s, char c)
        {
            if (s[s.Length - 1] == c) return s.DropLastChar();
            return s;
        }

        public static String DropLastChar(this String s)
        {
            return s.Substring(0, s.Length - 1);
        }

        public static String DropFirstChar(this String s)
        {
            return s.Substring(1, s.Length - 1);
        }

        /// <summary>
        /// String átalakítása hexa formátumra, csak 8 bites vagy alacsonyabb stringek esetén működik.
        /// </summary>
        /// <param name="s">Az átalakítandó string.</param>
        /// <returns>A hexa string</returns>
        public static string ToHex(this string s)
        {
            return s.Select(c => (byte)c).Select(b => String.Format("{0:X}", b)).Aggregate(String.Empty, (current, hex) => current + ((hex.Length == 1) ? "0" + hex : hex));
        }


        public static string NormalizeWhiteSpace(this string s)
        {
            if (s == null) throw new ArgumentNullException("s");
            var ss = s.Trim();
            var iswhite = false;
            var sLength = ss.Length;
            var sb = new StringBuilder(sLength);
            foreach (var c in ss.ToCharArray())
            {
                if (Char.IsWhiteSpace(c))
                {
                    if (iswhite)
                    {
                        //Continuing whitespace ignore it.
                    }
                    else
                    {
                        //New WhiteSpace

                        //Replace whitespace with a single space.
                        sb.Append(" ");
                        //Set iswhite to True and any following whitespace will be ignored
                        iswhite = true;
                    }
                }
                else
                {
                    sb.Append(c.ToString(Thread.CurrentThread.CurrentCulture));
                    //reset iswhitespace to false
                    iswhite = false;
                }
            }
            return sb.ToString();
        }

        public static string RemoveHtmlTags(this string htmlText)
        {
            var reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            return reg.Replace(htmlText, "");
        }

        public static bool IsFilePath(this string text)
        {
            try
            {
                Path.GetFullPath(text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static byte[] ToUtf8Bytes(this string str)
        {
            var x = Encoding.UTF8.GetBytes(str);
            return x;
        }

        public static string GetUtf8StringFromByteArray(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static T As<T>(this string value)
        {
            object v = value;

            // If a string, just return it
            if (typeof(T) == typeof(string))
            {
                return (T)v;
            }
            // Use a TypeConverter if one is available
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null && converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    return (T)converter.ConvertFromString(value);
                }
                catch (Exception ex)
                {
                    string failureMessage
                        = string.Format(
                            CultureInfo.CurrentCulture,
                            "Failed to convert \"{0}\" to {1}: {2}",
                            value,
                            typeof(T).Name,
                            ex.Message);
                    throw new InvalidOperationException(failureMessage);
                }
            }

            // Look for a constructor that takes a string
            var constructor
                = typeof(T).GetConstructor(new[] { typeof(string) });
            if (constructor != null)
            {
                return (T)constructor.Invoke(new[] { value });
            }

            var message
                = string.Format(
                    CultureInfo.CurrentCulture,
                    "Cannot convert {0} into {1}",
                    value,
                    typeof(T).Name);
            throw new InvalidOperationException(message);
        }

        /// <summary>
        /// A megadott stringet aposztrófok közé teszi.
        /// A string belsejében lévő aposztrófokat megduplázza.
        /// </summary>
        public static string ToSqlQuoted(this string str)
        {
            return (str == null || str == string.Empty) ? "\'\'" : "'" + str.Replace("'", "''") + "'";
        }

        /// <summary>
        /// A megadott karaktert aposztrófok közé teszi.
        /// </summary>
        public static string ToSqlQuoted(this char chr)
        {
            return (chr == '\'') ? "''''" : "'" + chr + "'";
        }

        /// <summary>
        /// A megadott stringet aposztrófok közé teszi.
        /// A string belsejében lévő aposztrófokat megduplázza.
        /// Kérésre nem teszi aposztrófot a string külső részére.
        /// </summary>
        public static string ToSqlQuoted(this string str, bool withoutApostrophe)
        {
            if (withoutApostrophe) return str.Replace("'", "''");
            return ToSqlQuoted(str);
        }

        /// <summary>
        /// Converts an Olson time zone ID to a Windows time zone ID.
        /// </summary>
        /// <param name="olsonTimeZoneId">An Olson time zone ID. See http://unicode.org/repos/cldr-tmp/trunk/diff/supplemental/zone_tzid.html. </param>
        /// <returns>
        /// The TimeZoneInfo corresponding to the Olson time zone ID, 
        /// or null if you passed in an invalid Olson time zone ID.
        /// </returns>
        /// <remarks>
        /// See http://unicode.org/repos/cldr-tmp/trunk/diff/supplemental/zone_tzid.html
        /// </remarks>
        public static TimeZoneInfo OlsonTimeZoneToTimeZoneInfo(string olsonTimeZoneId)
        {
            var olsonWindowsTimes = new Dictionary<string, string>()
    {
        { "Africa/Bangui", "W. Central Africa Standard Time" },
        { "Africa/Cairo", "Egypt Standard Time" },
        { "Africa/Casablanca", "Morocco Standard Time" },
        { "Africa/Harare", "South Africa Standard Time" },
        { "Africa/Johannesburg", "South Africa Standard Time" },
        { "Africa/Lagos", "W. Central Africa Standard Time" },
        { "Africa/Monrovia", "Greenwich Standard Time" },
        { "Africa/Nairobi", "E. Africa Standard Time" },
        { "Africa/Windhoek", "Namibia Standard Time" },
        { "America/Anchorage", "Alaskan Standard Time" },
        { "America/Argentina/San_Juan", "Argentina Standard Time" },
        { "America/Asuncion", "Paraguay Standard Time" },
        { "America/Bahia", "Bahia Standard Time" },
        { "America/Bogota", "SA Pacific Standard Time" },
        { "America/Buenos_Aires", "Argentina Standard Time" },
        { "America/Caracas", "Venezuela Standard Time" },
        { "America/Cayenne", "SA Eastern Standard Time" },
        { "America/Chicago", "Central Standard Time" },
        { "America/Chihuahua", "Mountain Standard Time (Mexico)" },
        { "America/Cuiaba", "Central Brazilian Standard Time" },
        { "America/Denver", "Mountain Standard Time" },
        { "America/Fortaleza", "SA Eastern Standard Time" },
        { "America/Godthab", "Greenland Standard Time" },
        { "America/Guatemala", "Central America Standard Time" },
        { "America/Halifax", "Atlantic Standard Time" },
        { "America/Indianapolis", "US Eastern Standard Time" },
        { "America/Indiana/Indianapolis", "US Eastern Standard Time" },
        { "America/La_Paz", "SA Western Standard Time" },
        { "America/Los_Angeles", "Pacific Standard Time" },
        { "America/Mexico_City", "Mexico Standard Time" },
        { "America/Montevideo", "Montevideo Standard Time" },
        { "America/New_York", "Eastern Standard Time" },
        { "America/Noronha", "UTC-02" },
        { "America/Phoenix", "US Mountain Standard Time" },
        { "America/Regina", "Canada Central Standard Time" },
        { "America/Santa_Isabel", "Pacific Standard Time (Mexico)" },
        { "America/Santiago", "Pacific SA Standard Time" },
        { "America/Sao_Paulo", "E. South America Standard Time" },
        { "America/St_Johns", "Newfoundland Standard Time" },
        { "America/Tijuana", "Pacific Standard Time" },
        { "Antarctica/McMurdo", "New Zealand Standard Time" },
        { "Atlantic/South_Georgia", "UTC-02" },
        { "Asia/Almaty", "Central Asia Standard Time" },
        { "Asia/Amman", "Jordan Standard Time" },
        { "Asia/Baghdad", "Arabic Standard Time" },
        { "Asia/Baku", "Azerbaijan Standard Time" },
        { "Asia/Bangkok", "SE Asia Standard Time" },
        { "Asia/Beirut", "Middle East Standard Time" },
        { "Asia/Calcutta", "India Standard Time" },
        { "Asia/Colombo", "Sri Lanka Standard Time" },
        { "Asia/Damascus", "Syria Standard Time" },
        { "Asia/Dhaka", "Bangladesh Standard Time" },
        { "Asia/Dubai", "Arabian Standard Time" },
        { "Asia/Irkutsk", "North Asia East Standard Time" },
        { "Asia/Jerusalem", "Israel Standard Time" },
        { "Asia/Kabul", "Afghanistan Standard Time" },
        { "Asia/Kamchatka", "Kamchatka Standard Time" },
        { "Asia/Karachi", "Pakistan Standard Time" },
        { "Asia/Katmandu", "Nepal Standard Time" },
        { "Asia/Kolkata", "India Standard Time" },
        { "Asia/Krasnoyarsk", "North Asia Standard Time" },
        { "Asia/Kuala_Lumpur", "Singapore Standard Time" },
        { "Asia/Kuwait", "Arab Standard Time" },
        { "Asia/Magadan", "Magadan Standard Time" },
        { "Asia/Muscat", "Arabian Standard Time" },
        { "Asia/Novosibirsk", "N. Central Asia Standard Time" },
        { "Asia/Oral", "West Asia Standard Time" },
        { "Asia/Rangoon", "Myanmar Standard Time" },
        { "Asia/Riyadh", "Arab Standard Time" },
        { "Asia/Seoul", "Korea Standard Time" },
        { "Asia/Shanghai", "China Standard Time" },
        { "Asia/Singapore", "Singapore Standard Time" },
        { "Asia/Taipei", "Taipei Standard Time" },
        { "Asia/Tashkent", "West Asia Standard Time" },
        { "Asia/Tbilisi", "Georgian Standard Time" },
        { "Asia/Tehran", "Iran Standard Time" },
        { "Asia/Tokyo", "Tokyo Standard Time" },
        { "Asia/Ulaanbaatar", "Ulaanbaatar Standard Time" },
        { "Asia/Vladivostok", "Vladivostok Standard Time" },
        { "Asia/Yakutsk", "Yakutsk Standard Time" },
        { "Asia/Yekaterinburg", "Ekaterinburg Standard Time" },
        { "Asia/Yerevan", "Armenian Standard Time" },
        { "Atlantic/Azores", "Azores Standard Time" },
        { "Atlantic/Cape_Verde", "Cape Verde Standard Time" },
        { "Atlantic/Reykjavik", "Greenwich Standard Time" },
        { "Australia/Adelaide", "Cen. Australia Standard Time" },
        { "Australia/Brisbane", "E. Australia Standard Time" },
        { "Australia/Darwin", "AUS Central Standard Time" },
        { "Australia/Hobart", "Tasmania Standard Time" },
        { "Australia/Perth", "W. Australia Standard Time" },
        { "Australia/Sydney", "AUS Eastern Standard Time" },
        { "Etc/GMT", "UTC" },
        { "Etc/GMT+11", "UTC-11" },
        { "Etc/GMT+12", "Dateline Standard Time" },
        { "Etc/GMT+2", "UTC-02" },
        { "Etc/GMT-12", "UTC+12" },
        { "Europe/Amsterdam", "W. Europe Standard Time" },
        { "Europe/Athens", "GTB Standard Time" },
        { "Europe/Belgrade", "Central Europe Standard Time" },
        { "Europe/Berlin", "W. Europe Standard Time" },
        { "Europe/Brussels", "Romance Standard Time" },
        { "Europe/Budapest", "Central Europe Standard Time" },
        { "Europe/Dublin", "GMT Standard Time" },
        { "Europe/Helsinki", "FLE Standard Time" },
        { "Europe/Istanbul", "GTB Standard Time" },
        { "Europe/Kiev", "FLE Standard Time" },
        { "Europe/London", "GMT Standard Time" },
        { "Europe/Minsk", "E. Europe Standard Time" },
        { "Europe/Moscow", "Russian Standard Time" },
        { "Europe/Paris", "Romance Standard Time" },
        { "Europe/Sarajevo", "Central European Standard Time" },
        { "Europe/Warsaw", "Central European Standard Time" },
        { "Indian/Mauritius", "Mauritius Standard Time" },
        { "Pacific/Apia", "Samoa Standard Time" },
        { "Pacific/Auckland", "New Zealand Standard Time" },
        { "Pacific/Fiji", "Fiji Standard Time" },
        { "Pacific/Guadalcanal", "Central Pacific Standard Time" },
        { "Pacific/Guam", "West Pacific Standard Time" },
        { "Pacific/Honolulu", "Hawaiian Standard Time" },
        { "Pacific/Pago_Pago", "UTC-11" },
        { "Pacific/Port_Moresby", "West Pacific Standard Time" },
        { "Pacific/Tongatapu", "Tonga Standard Time" }
    };

            var windowsTimeZone = default(TimeZoneInfo);
            if (olsonWindowsTimes.TryGetValue(olsonTimeZoneId, out var windowsTimeZoneId))
            {
                try { windowsTimeZone = TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZoneId); }
                catch (TimeZoneNotFoundException) { }
                catch (InvalidTimeZoneException) { }
            }
            return windowsTimeZone;
        }
    }
}
