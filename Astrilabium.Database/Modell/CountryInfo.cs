using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrolabium.Database.Modell
{
    public class CountryInfo
    {
        /// <summary>
        /// ISO
        /// </summary>
        [Key]
        public string ISO { get; set; }
        /// <summary>
        /// ISO3
        /// </summary>
        public string ISO3 { get; set; }
        /// <summary>
        /// ISO-Numeric
        /// </summary>
        public int ISONumeric { get; set; }
        /// <summary>
        /// fips
        /// </summary>       
        public string Fips { get; set; }
        /// <summary>
        /// Country
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Capital	
        /// </summary>
        public string CapitalName { get; set; }
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
        public string Continent { get; set; }
        /// <summary>
        /// Top level domain
        /// </summary>
        public string TLD { get; set; }
        /// <summary>
        /// CurrencyCode
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// CurrencyName
        /// </summary>
        public string CurrencyName { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Postal Code Format
        /// </summary>
        public string PostalCodeFormat { get; set; }
        /// <summary>
        /// Postal Code Regex
        /// </summary>
        public string PpstalCodeRegex { get; set; }
        /// <summary>
        /// Languages
        /// </summary>
        public string Languages { get; set; }
        /// <summary>
        /// geonameid
        /// </summary>
        public int GeonameId { get; set; }
        /// <summary>
        /// neighbours
        /// </summary>
        public string Neighbours { get; set; }
        /// <summary>
        /// EquivalentFipsCode
        /// </summary>
        public string EquivalentFipsCode { get; set; }

        public static int GetProprtyIndex(string propertyName)
        {
            var type = typeof(CountryInfo);
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

}
