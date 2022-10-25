using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Astrolabium.Models;
using SweCore.Date;

namespace Astrolabium.ViewModels
{

    /// <summary>
    /// Calculation result
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class CalculationResultViewModel : ViewModelBase
    {
        /// <summary>
        /// New result
        /// </summary>
        public CalculationResultViewModel() {
            Planets = new ObservableCollection<PlanetValues>();
            Houses = new ObservableCollection<HouseValues>();
            ASMCs = new ObservableCollection<HouseValues>();
        }

        /// <summary>
        /// Reset the result
        /// </summary>
        public void Reset() {
            DateUTC = new DateUt();
            JulianDay = new JulianDay();
            EphemerisTime = new EphemerisTime();
            SideralTime = 0;
            MeanEclipticObliquity = 0;
            TrueEclipticObliquity = 0;
            NutationLongitude = 0;
            NutationObliquity = 0;
            Planets.Clear();
            Houses.Clear();
            ASMCs.Clear();
        }

        public void Apply(EphemerisResult ephemerisResult) {
            Result = ephemerisResult;
            DateUTC = ephemerisResult.DateUTC;
            JulianDay = ephemerisResult.JulianDay;
            EphemerisTime = ephemerisResult.EphemerisTime;
            SideralTime = ephemerisResult.SideralTime;
            MeanEclipticObliquity = ephemerisResult.MeanEclipticObliquity;
            TrueEclipticObliquity = ephemerisResult.TrueEclipticObliquity;
            NutationLongitude = ephemerisResult.NutationLongitude;
            NutationObliquity = ephemerisResult.NutationObliquity;
            Planets.Clear();
            foreach (var p in ephemerisResult.Planets) Planets.Add(p);
            Houses.Clear();
            foreach (var h in ephemerisResult.Houses) Houses.Add(h);
            ASMCs.Clear();
            foreach (var h in ephemerisResult.AsmCs) ASMCs.Add(h);
        }

        public EphemerisResult Result {
            get => _Result;
            private set {
                _Result = value;
                OnPropertyChanged("Result");
            }
        }
        private EphemerisResult _Result;

        /// <summary>
        /// Date UTC
        /// </summary>
        public DateUt DateUTC {
            get => _DateUTC;
            set {
                _DateUTC = value;
                OnPropertyChanged("DateUTC");
            }
        }
        private DateUt _DateUTC;

        /// <summary>
        /// Julian Day
        /// </summary>
        public JulianDay JulianDay {
            get => _JulianDay;
            set {
                _JulianDay = value;
                OnPropertyChanged("JulianDay");
            }
        }
        private JulianDay _JulianDay;

        /// <summary>
        /// Ephemeris time
        /// </summary>
        public EphemerisTime EphemerisTime {
            get => _EphemerisTime;
            set {
                _EphemerisTime = value;
                OnPropertyChanged("EphemerisTime");
                OnPropertyChanged("DeltaTSec");
            }
        }
        private EphemerisTime _EphemerisTime;

        /// <summary>
        /// Delat T in seconds
        /// </summary>
        public double DeltaTSec => EphemerisTime.DeltaT * 86400.0;

        /// <summary>
        /// Sideral time
        /// </summary>
        public double SideralTime {
            get => _SideralTime;
            set {
                _SideralTime = value;
                OnPropertyChanged("SideralTime");
                OnPropertyChanged("SideralTimeInDegrees");
                OnPropertyChanged("ARMC");
            }
        }
        private double _SideralTime;

        /// <summary>
        /// Sideral time in degrees
        /// </summary>
        public double SideralTimeInDegrees => SideralTime * 15;

        /// <summary>
        /// ARMC : Sideral time in degrees
        /// </summary>
        public double ARMC => SideralTime * 15;

        /// <summary>
        /// Mean ecliptic obliquity
        /// </summary>
        public double MeanEclipticObliquity {
            get => _MeanEclipticObliquity;
            set {
                _MeanEclipticObliquity = value;
                OnPropertyChanged("MeanEclipticObliquity");
            }
        }
        private double _MeanEclipticObliquity;

        /// <summary>
        /// True ecliptic obliquity
        /// </summary>
        public double TrueEclipticObliquity {
            get => _TrueEclipticObliquity;
            set {
                _TrueEclipticObliquity = value;
                OnPropertyChanged("TrueEclipticObliquity");
            }
        }
        private double _TrueEclipticObliquity;

        /// <summary>
        /// Nutation in longitude
        /// </summary>
        public double NutationLongitude {
            get => _NutationLongitude;
            set {
                _NutationLongitude = value;
                OnPropertyChanged("NutationLongitude");
            }
        }
        private double _NutationLongitude;

        /// <summary>
        /// Nutation in obliquity
        /// </summary>
        public double NutationObliquity {
            get => _NutationObliquity;
            set {
                _NutationObliquity = value;
                OnPropertyChanged("NutationObliquity");
            }
        }
        private double _NutationObliquity;

        /// <summary>
        /// Planets calculation result
        /// </summary>
        public ObservableCollection<PlanetValues> Planets { get; }

        /// <summary>
        /// Houses
        /// </summary>
        public ObservableCollection<HouseValues> Houses { get; }

        /// <summary>
        /// Ascendants, MC etc.
        /// </summary>
        public ObservableCollection<HouseValues> ASMCs { get; }

    }

}
