using System;
using System.Collections.Generic;
using System.Text;

namespace Astrolabium.Database.Modell
{
    public class Person
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public bool BirthDateIsCorret { get; set; }
        public int? BirthLocationID { get; set; }
        public int? ActualLocationID { get; set; }
        public int Sex { get; set; }
    }
}
