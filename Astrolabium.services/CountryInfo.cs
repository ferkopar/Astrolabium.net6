using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrolabium.Sevices
{
    public class CountryInfoItem
    {
        /// <summary>
        /// ISO
        /// </summary>
        public String ISO { get; set; }
        /// <summary>
        /// ISO3
        /// </summary>
        public String ISO3 { get; set; }
        /// <summary>
        /// ISO-Numeric
        /// </summary>
        public int ISONumeric { get; set; }
        /// <summary>
        /// fips
        /// </summary>       
        public String Fips { get; set; }
        /// <summary>
        /// Country
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Capital	
        /// </summary>
        public String CapitalName { get; set; }
        /// <summary>
        /// Area(in sq km)
        /// </summary>
        public int Area { get; set; }
        /// <summary>
        /// Population
        /// </summary>
        public long Population { get; set; }
        /// <summary>
        /// Continent
        /// </summary>
        public String Continent { get; set; }
        /// <summary>
        /// Top level domain
        /// </summary>
        public String TLD { get; set; }
        /// <summary>
        /// CurrencyCode
        /// </summary>
        public String CurrencyCode { get; set; }
        /// <summary>
        /// CurrencyName
        /// </summary>
        public String CurrencyName { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        public String Phone { get; set; }
        /// <summary>
        /// Postal Code Format
        /// </summary>
        public String PostalCodeFormat { get; set; }
        /// <summary>
        /// Postal Code Regex
        /// </summary>
        public String PpstalCodeRegex { get; set; }
        /// <summary>
        /// Languages
        /// </summary>
        public String Languages { get; set; }
        /// <summary>
        /// geonameid
        /// </summary>
        public int GeonameId { get; set; }
        /// <summary>
        /// neighbours
        /// </summary>
        public String Neighbours { get; set; }
        /// <summary>
        /// EquivalentFipsCode
        /// </summary>
        public String EquivalentFipsCode { get; set; }

        public static int GetProprtyIndex(string propertyName)
        {
            var type = typeof(CountryInfoItem);
            var properties = type.GetProperties();
            int i;
            for (i = 0; i < properties.Length; i++)
            {
                if (properties[i].Name.Equals(propertyName))
                {
                    return i;
                }
            }
            throw new ArgumentException($"Nem létező tulajdonság:{propertyName}");
        }
    }
    
    public static class CountryInfo
    {
        private static List<CountryInfoItem> countryInfoItems;

        public static List<CountryInfoItem> List
        {
            get { return countryInfoItems; }
            set { countryInfoItems = value; }
        }

        public static CountryInfoItem Get(string s)
        {
            try
            {
              return countryInfoItems.Single(i => i.ISO == s);
            }
            catch (Exception)
            {
                return null;
            }
        }

        static CountryInfo()
        {
            string line;
            countryInfoItems = new List<CountryInfoItem>();
            using StreamReader file = new StreamReader(@"sources\CountryInfo.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (line[0] == '#') continue;
                string[] splitedLine = line.Split('\t');
                var x = splitedLine[CountryInfoItem.GetProprtyIndex(nameof(CountryInfoItem.Name))];
                countryInfoItems.Add
                (
                     new CountryInfoItem
                     {
                         ISO = splitedLine[CountryInfoItem.GetProprtyIndex(nameof(CountryInfoItem.ISO))],
                         Name = splitedLine[CountryInfoItem.GetProprtyIndex(nameof(CountryInfoItem.Name))],
                         GeonameId = int.Parse(splitedLine[CountryInfoItem.GetProprtyIndex(nameof(CountryInfoItem.GeonameId))])
                     }
                ) ;
            }
        }
    }
}
