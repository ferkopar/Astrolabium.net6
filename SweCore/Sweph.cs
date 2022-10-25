
using System;
using System.IO;
using System.Text;
using SweCore.Date;
using SweCore.Houses;
using SweCore.Planets;
using SwissEphCore;
using SwissEphCore.Tools;

namespace SweCore
{
    /// <summary>
    /// Swiss Ephemeris engine
    /// </summary>
    public class Sweph : IDisposable
    {
        private bool _initialized;
        private SwissEph _swissEph;
        private SweDate _date;
        private Persit.IDataProvider _dataProvider;
        private SwePlanet _planets;
        #region Public constants
        /// <summary>
        /// Current Swiss Ephemeris version
        /// </summary>
        public const String Version = "2.00.00";
        #endregion

        #region Ctors & Dest

        /// <summary>
        /// Create a new context
        /// </summary>
        public Sweph(SweConfig config = null)
        {
            _initialized = false;
            Config = config == null ? new SweConfig() : config.Clone();
        }

        /// <summary>
        /// Internal release resources
        /// </summary>
        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                Close();
                _initialized = true;
            }
        }

        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose() {
            Dispose(true);
        }

        #endregion

        #region General methods

        /// <summary>
        /// Check if engine is initialized
        /// </summary>
        protected void CheckInitialized() {
            if (!_initialized) {
                _initialized = true;
                Initialize();
            }
        }

        /// <summary>
        /// Initialize
        /// </summary>
        protected virtual void Initialize(){
            _date = CreateDateEngine();
            _dataProvider = CreateDataProvider();
            _planets = CreatePlanetsEngine();
            _swissEph = new SwissEph();
            //_swissEph.swe_set_ephe_path(@"C:\Work\Astrolabium.net\Astrolabium.net\bin\Debug\sweph\ephe");
            _swissEph.OnLoadFile += (s, e) => {
                e.File = LoadFile(e.FileName);
            };
            _swissEph.OnTrace += (s, e) => {
                Trace(e.Message);
            };
            RecalcSwissEphState();
        }

        /// <summary>
        /// Internal close
        /// </summary>
        private void Close() {
            _swissEph?.Dispose();
            _swissEph = null;
            _date = null;
            _dataProvider = null;
        }

        /// <summary>
        /// Check default encoding for Swiss Ephemeris
        /// </summary>
        public static Encoding CheckEncoding(Encoding encoding) {
            return encoding ?? Encoding.GetEncoding("Windows-1252");
        }

        #endregion

        #region Swiss Ephemeris proxies

        private int _swissFlag;
        private char _hsys = 'P';

        /// <summary>
        /// Recalculate the swisseph flag and parameters
        /// </summary>
        protected void RecalcSwissEphState() {
            _swissFlag = SwissEph.SEFLG_SPEED;
            // Ephemeris type
            switch (Ephemeris) {
                case EphemerisMode.Moshier:
                    _swissFlag |= SwissEph.SEFLG_MOSEPH;
                    break;
                case EphemerisMode.JPL:
                    _swissFlag |= SwissEph.SEFLG_JPLEPH;
                    break;
                default:
                    _swissFlag |= SwissEph.SEFLG_SWIEPH;
                    break;
            }
            // Position center
            var sidmode = SwissEph.SE_SIDM_FAGAN_BRADLEY;
            switch (PositionCenter) {
                case PositionCenter.Topocentric:
                    _swissFlag |= SwissEph.SEFLG_TOPOCTR;
                    break;
                case PositionCenter.Heliocentric:
                    _swissFlag |= SwissEph.SEFLG_HELCTR;
                    break;
                case PositionCenter.Barycentric:
                    _swissFlag |= SwissEph.SEFLG_BARYCTR;
                    break;
                case PositionCenter.SiderealFagan:
                    _swissFlag |= SwissEph.SEFLG_SIDEREAL;
                    sidmode = SwissEph.SE_SIDM_FAGAN_BRADLEY;
                    break;
                case PositionCenter.SiderealLahiri:
                    _swissFlag |= SwissEph.SEFLG_SIDEREAL;
                    sidmode = SwissEph.SE_SIDM_LAHIRI;
                    break;
            }
            SwissEph.swe_set_sid_mode(sidmode, 0, 0);
            // House system
            _hsys = SweHouse.HouseSystemToChar(HouseSystem);
        }

        /// <summary>
        /// swe_set_jpl_file()
        /// </summary>
        /// <param name="file"></param>
        public void swe_set_jpl_file(string file) {
            _swissEph.swe_set_jpl_file(file);
        }

        /// <summary>
        /// swe_set_topo()
        /// </summary>
        public void swe_set_topo(double geolon, double geolat, double height) {
            SwissEph.swe_set_topo(geolon, geolat, height);
        }

        /// <summary>
        /// swe_calc()
        /// </summary>
        public Int32 swe_calc(double tjd, int ipl, double[] xx, ref string serr) {
            CheckInitialized();
            System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);
            return SwissEph.swe_calc(tjd, ipl, _swissFlag, xx, ref serr);
        }
        public Int32 swe_calc(double tjd, int ipl, Int32 iflag, double[] xx, ref string serr) {
            CheckInitialized();
            return SwissEph.swe_calc(tjd, ipl, iflag, xx, ref serr);
        }

        /// <summary>
        /// swe_sidtime()
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public double swe_sidtime(double tjd_ut) { return SwissEph.swe_sidtime(tjd_ut); }

        /// <summary>
        /// swe_fixstar()
        /// </summary>
        public Int32 swe_fixstar(string star, double tjd, double[] xx, ref string serr) {
            CheckInitialized();
            return SwissEph.swe_fixstar(star, tjd, _swissFlag, xx, ref serr);
        }

        /// <summary>
        /// swe_get_planet_name()
        /// </summary>
        public string swe_get_planet_name(int ipl) { return SwissEph.swe_get_planet_name(ipl); }

        /// <summary>
        /// swe_houses()
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int swe_houses(double tjd_ut, double geolat, double geolon, double[] cusps, double[] ascmc) {
            CheckInitialized();
            return SwissEph.swe_houses(tjd_ut, geolat, geolon, _hsys, cusps, ascmc);
        }

        /// <summary>
        /// swe_houses_ex()
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int swe_houses_ex(double tjd_ut, double geolat, double geolon, CPointer<double> hcusps, CPointer<double> ascmc) {
            CheckInitialized();
            return SwissEph.swe_houses_ex(tjd_ut, _swissFlag, geolat, geolon, _hsys, hcusps, ascmc);
        }

        /// <summary>
        /// swe_house_pos()
        /// </summary>
        public double swe_house_pos(double armc, double geolon, double eps, double[] xpin, ref string serr) {
            CheckInitialized();
            return SwissEph.swe_house_pos(armc, geolon, eps, _hsys, xpin, ref serr);
        }

        #endregion

        #region Data management

        /// <summary>
        /// Create a new data provider
        /// </summary>
        /// <returns></returns>
        protected virtual Persit.IDataProvider CreateDataProvider() {
            return new Persit.EmptyDataProvider();
        }

        #endregion

        #region Date management

        /// <summary>
        /// Create a new date engine
        /// </summary>
        protected virtual SweDate CreateDateEngine() {
            return new SweDate(this, Config.UseEspenakMeeusDeltaT);
        }

        /// <summary>
        /// Create a Julian Day
        /// </summary>
        public JulianDay JulianDay(int year, int month, int day, double hour, DateCalendar? calendar = null) {
            return JulianDay(new DateUt(year, month, day, hour), calendar);
        }

        /// <summary>
        /// Create a Julian Day
        /// </summary>
        public JulianDay JulianDay(int year, int month, int day, int hour, int minute, int second, DateCalendar? calendar = null) {
            return JulianDay(new DateUt(year, month, day, hour, minute, second), calendar);
        }

        /// <summary>
        /// Create a Julian Day
        /// </summary>
        public JulianDay JulianDay(DateUt date, DateCalendar? calendar = null) {
            return new JulianDay(date, calendar);
        }

        /// <summary>
        /// Create an Ephemeris Time
        /// </summary>
        public EphemerisTime EphemerisTime(JulianDay day) {
            return new EphemerisTime(day, Date.DeltaT(day));
        }

        /// <summary>
        /// Get Date UT from Julian Day
        /// </summary>
        public DateUt DateUt(JulianDay jd) {
            return SweDate.JulianDayToDate(jd.Value, jd.Calendar);
        }

        /// <summary>
        /// Get Date UT from Ephemeris Time
        /// </summary>
        public DateUt DateUt(EphemerisTime et) {
            return SweDate.JulianDayToDate(et.JulianDay.Value, et.JulianDay.Calendar);
        }

        #endregion

        #region Planets management

        /// <summary>
        /// Create a new planets envgine
        /// </summary>
        protected virtual SwePlanet CreatePlanetsEngine(){
            return new SwePlanet(this);
        }

        /// <summary>
        /// Get a planet name
        /// </summary>
        /// <param name="planetId">Id of planet</param>
        /// <returns>Name</returns>
        public String PlanetName(Planet planetId) {
            return Planets.GetPlanetName(planetId);
        }

        #endregion

        #region File management

        /// <summary>
        /// Load a file
        /// </summary>
        /// <param name="filename">File name</param>
        /// <returns>File loaded or null if file not found</returns>
        protected internal Stream LoadFile(string filename) {
            var h = OnLoadFile;
            if (h != null) {
                var e = new LoadFileEventArgs(filename);
                h(this, e);
                return e.File;
            }
            return null;
        }

        #endregion

        #region Trace

        /// <summary>
        /// Trace information
        /// </summary>
        public void Trace(String format, params object[] args) {
            var h = OnTrace;
            if (h != null) {
                String message = args != null ? String.Format(format, args) : format;
                h(this, new TraceEventArgs(message));
            }
        }

        #endregion

        #region Protected properties

        /// <summary>
        /// Current configuration
        /// </summary>
        protected SweConfig Config { get; }

        /// <summary>
        /// Swiss Ephemeris
        /// </summary>
        protected SwissEph SwissEph { get { CheckInitialized(); return _swissEph; } }

        #endregion

        #region Current engines

        /// <summary>
        /// Current date engine
        /// </summary>
        public SweDate Date { get { CheckInitialized(); return _date; } }

        /// <summary>
        /// Current data provider
        /// </summary>
        public Persit.IDataProvider DataProvider { get { CheckInitialized(); return _dataProvider; } }

        /// <summary>
        /// Current planets engine
        /// </summary>
        public SwePlanet Planets { get { CheckInitialized(); return _planets; } }

        #endregion

        #region Configuration properties

        /// <summary>
        /// Ephemeris use
        /// </summary>
        public EphemerisMode Ephemeris
        {
            get => _ephemeris;
            set
            {
                if (_ephemeris != value)
                {
                    _ephemeris = value;
                    RecalcSwissEphState();
                }
            }
        }
        private EphemerisMode _ephemeris;

        /// <summary>
        /// Current position center
        /// </summary>
        public PositionCenter PositionCenter
        {
            get => _positionCenter;
            set
            {
                if (_positionCenter != value)
                {
                    _positionCenter = value;
                    RecalcSwissEphState();
                }
            }
        }
        private PositionCenter _positionCenter;

        /// <summary>
        /// Current house system
        /// </summary>
        public HouseSystem HouseSystem
        {
            get => _houseSystem;
            set
            {
                if (_houseSystem != value)
                {
                    _houseSystem = value;
                    RecalcSwissEphState();
                }
            }
        }
        private HouseSystem _houseSystem;

        #endregion

        #region Events

        /// <summary>
        /// Event raised when a new trace message is invoked
        /// </summary>
        public event EventHandler<TraceEventArgs> OnTrace;

        /// <summary>
        /// Event raised when loading a file is required
        /// </summary>
        public event EventHandler<LoadFileEventArgs> OnLoadFile;

        #endregion

    }

}
