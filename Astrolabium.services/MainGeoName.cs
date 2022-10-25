using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using FerkopaUtils;
using NGeoNames;
using NGeoNames.Entities;
using NGeoNames.Parsers;



namespace Astrolabium.Sevices
{
    public static class MainGeoName
    {
        private static List<ExtendedGeoName>_geoNames = new List<ExtendedGeoName>();
        //private static List<CountryInfo> _countries = new List<CountryInfo>();

        public static List<ExtendedGeoName> GeoNames
        {
            get { return _geoNames; }
        }

        public static ExtendedGeoName GetGeoName(int? id)
        {
            if (id == null) return null;
            try
            {
                return _geoNames.Single(n => n.Id == id);
            }
            catch
            {
                return _geoNames.First();
            }
        }

        static MainGeoName()
        {
            var geoNames = new GeoFileReader().ReadRecords<ExtendedGeoName>(@"sources\HU.txt", new ExtendedGeoNameParser());
            _geoNames.AddRange(geoNames
                .Where(p => p.Population > 10000 && p.FeatureClass.Equals("P"))
                .OrderBy(p => p.Name)   
                );
        }
    }
}