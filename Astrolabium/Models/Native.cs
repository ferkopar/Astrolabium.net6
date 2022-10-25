using Astrolabium.Services;
using Astrolabium.Database.Modell;
using Astrolabium.Database;
using System;

namespace Astrolabium.Models
{
    
    public enum SexEnum{Female,Male}
    
    public delegate void NativeSavedEventHandler(object source, EventArgs args);
    public partial class Native 
    {
        private readonly ICalcService _service = new CalcService();
        public EphemerisResult EphemerisData { get; private set; }
        private void CalculateEphemerisData()
        {
            if (BirthDate == DateTime.MinValue || BirthLocation == null) return;
            var inputCalculation = new InputCalculation
            {
                DateLocal = BirthDate,
                Position = BirthLocation.GeoPos
            };
            EphemerisData = _service.Calculate(new Configuration(), inputCalculation);
            EphemerisData.CalculateAspects();
        }
        public void CalculateEphemerisData(InputCalculation inputCalculation)
        {
            EphemerisData = _service.Calculate(new Configuration(), inputCalculation);
            EphemerisData.CalculateAspects();
        }
        private DateTime birthDate;
        private Location actualLocation;
        public InputCalculation InputCalculation { get; set; } = new InputCalculation();
        public Guid ID { get; set; }
        public DateTime BirthDate
        {
            get { return birthDate; }
            set { birthDate = value;
                CalculateEphemerisData();
            }
        }
        public bool BirthDateIsCorrect { get; set; } = false;
        public Location ActualLocation
        {
            get { return actualLocation; }
            set { actualLocation = value;
                  CalculateEphemerisData();
            }
        }
        public Location BirthLocation{ get; set; }
        public string Name { get; set; }
        public SexEnum Sex { get; set; } = SexEnum.Female;
        public static Native Default => new Native("Éppen most", DateTime.Now, Location.Default);
        public void DeleteFromDatabase()
        {
            var person = new Person
            {
                ID = ID == Guid.Empty ? Guid.NewGuid() : ID,
                Name = Name,
                Sex = (int)Sex,
                BirthDate = BirthDate,
                BirthDateIsCorret = BirthDateIsCorrect,
                BirthLocationID = BirthLocation?.GeoNameId,
                ActualLocationID = ActualLocation?.GeoNameId
            };
            using var db = new Context();
            var p = db.Persons.Find(person.ID);
            if (p != null) {
                db.Persons.Remove(p);
                db.SaveChanges();
            }
        }       
        public void WriteToDatabase()
        {
            var person = new Person
            {
                ID = ID == Guid.Empty?Guid.NewGuid():ID,
                Name = Name,
                Sex = (int)Sex,
                BirthDate = BirthDate,
                BirthDateIsCorret = BirthDateIsCorrect,
                BirthLocationID = BirthLocation?.GeoNameId,
                ActualLocationID = ActualLocation?.GeoNameId
            };
            using var db = new Context();
            Person p;
            if((p = db.Persons.Find(person.ID)) == null)
            {
                db.Persons.Add(person);
            }
            else
            {
                p.Name = person.Name;
                p.Sex = person.Sex;
                p.BirthDate = person.BirthDate;
                p.BirthDateIsCorret = person.BirthDateIsCorret;
                p.BirthLocationID = person.BirthLocationID;
                p.ActualLocationID = person.BirthLocationID;
            }
            db.SaveChanges();
        }
        public Native() { } 
        public Native(string name, DateTime birthDate, Location location)
        {
            BirthDate = birthDate;
            Name = name;
            BirthLocation = location;
            CalculateEphemerisData();
        }
        public override string ToString()
        {
            return Name;
        }
    }
}

