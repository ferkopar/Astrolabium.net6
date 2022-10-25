using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Astrolabium.Common;
using Astrolabium.Helpers;
using Astrolabium.Models;
using Astrolabium.Services;
using Astrolabium.ViewModels;
using System.IO;
using System.Linq;
using SweCore.Planets;
using System.Drawing.Imaging;
using System.Windows.Input;
using System.Numerics;
using Vector = System.Windows.Vector;
using static System.Net.Mime.MediaTypeNames;

namespace Astrolabium.CustomControls
{
    public class WheelPanelEventArgs : EventArgs
    {
        // public Horoscope horoscope;
        public WheelPanelEventArgs() : base() { }
        public Planet? Planet { get; set; }
        public PlanetValues Values { get; set; }
        public EphemerisResult Result { get; set; }
    }

    public delegate void MouseEnterPlanetHandler(object source, WheelPanelEventArgs args);
    public delegate void MouseExitPlanetHandler(object source, WheelPanelEventArgs args);
    public delegate void MouseClickPlanetHandler(object source, WheelPanelEventArgs args);

    internal class WheelPanel : Panel
    {

        private Planet? _planetUnderMouse;
        private readonly ICalcService _service = new CalcService();
        private readonly Rectangles _occupedRectangles = new Rectangles();

        // Dependency Property
        public static readonly DependencyProperty EphemerisDataProperty =
             DependencyProperty.Register("EphemerisData", typeof(EphemerisResult),
             typeof(WheelPanel));
        // .NET Property wrapper
        public EphemerisResult EphemerisData
        {
            get => (EphemerisResult)GetValue(EphemerisDataProperty);
            set
            {
                SetValue(EphemerisDataProperty, value);
                InvalidateVisual();
            }
        }

        public Native Native
        {
            get { return (Native)GetValue(NativeProperty); }
            set
            {
                if (value != null)
                {
                    SetValue(NativeProperty, value);
                    SetValue(EphemerisDataProperty, value.EphemerisData);
                    InvalidateVisual(); 
                }
            }
        }

        // Using a DependencyProperty as the backing store for Native.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NativeProperty =
            DependencyProperty.Register("Native", typeof(Native), typeof(WheelPanel), null);

        public CalculationResultViewModel CalculationResultViewModel { get; set; }
        public WheelPanel()
        {
             Native = Native.Default;
        }
        public WheelPanel(Native native)
        {
       
             Native = native;
            Children.Add(new Button());
            
        }

        //declaring the event
        public event MouseEnterPlanetHandler MouseEnteredPlanet;
        private void OnMouseEnteredPlanet(Planet? planet)
        {
            //Invoking all the event handlers
            if (MouseEnteredPlanet != null)
            {
                var args = new WheelPanelEventArgs { Planet = planet };
                MouseEnteredPlanet(this, args);
            }
        }
        public event MouseExitPlanetHandler MouseExitPlanet;
        private void OnMouseExitPlanet(Planet? planet)
        {
            //Invoking all the event handlers
            if (MouseExitPlanet != null)
            {
                var args = new WheelPanelEventArgs { Planet = planet };
                MouseExitPlanet(this, args);
            }
        }
        public event MouseClickPlanetHandler MouseClickPlanet;
        private void OnMouseClickPlanet(Planet? planet)
        {
            //Invoking all the event handlers
            if (MouseClickPlanet != null)
            {
                var args = new WheelPanelEventArgs { Planet = planet, Values = EphemerisData[planet], Result = EphemerisData };
                MouseClickPlanet(this, args);
            }
        }
        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var p = _occupedRectangles.PlanetAtPoint(e.GetPosition(this));
            if (p != null)
            {
                if (_planetUnderMouse != p)
                {
                    OnMouseEnteredPlanet(p);
                    _planetUnderMouse = p;
                }
            }
            else
            {
                if (_planetUnderMouse != null)
                {
                    OnMouseExitPlanet(_planetUnderMouse);
                    _planetUnderMouse = null;
                }

            }

        }
        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            var p = _occupedRectangles.PlanetAtPoint(e.GetPosition(this));
            if (p != null)
            {
                OnMouseClickPlanet(p);
            }
        }
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (EphemerisData == null) return; 

            string GetHouseString(int houseNo) => ((char)(0x2160 + houseNo)).ToString();
           
            const double magicNumber = 50;
            const double mainWheelDiffRatio = 2 / magicNumber;
            const double halferWheelDiffRatio = 4 / magicNumber;
            const double rulerWheelDiffRatio = 9 / magicNumber;
            const double smallRulerWheelDiffRatio = 9.99 / magicNumber;

            const double innerWheelDiffRatio = 10 / magicNumber;
            const double planetStickDiffRatio = 4/magicNumber;
            const double planetWheelDiffRatio = 5 / magicNumber;
            const double aspectWheelDifRatio = 12 / magicNumber;
            const double houseWheelDiffRatio = 10 / magicNumber;
            const double signCharacterDiffRatio = 5 / magicNumber;
            const double bodyCharacterDiffRatio = 5 / magicNumber;
            const double houseCharacterDiffRatio = 4 / magicNumber;


            base.OnRender(dc);
            _occupedRectangles.Clear();

            Brush drawingBrush = Brushes.AliceBlue;
            Brush selectedDrawingBrush = Brushes.LightGoldenrodYellow;
            var drawingPen = new Pen(Brushes.SteelBlue, 3);
            var linePen = new Pen(Brushes.RoyalBlue, 1);
            var blackLinePen = new Pen(Brushes.Black, .5);
            var strongLinePen = new Pen(Brushes.Tomato, 2);
            var strongBluePen = new Pen(Brushes.RoyalBlue, 2);

            if (ActualWidth < 10 || ActualHeight < 10) return;
            var squareSize = new Size(ActualWidth - 10, ActualHeight - 10);
            var wheelSize = new Size(squareSize.Width - 10, squareSize.Height - 10);
            var topLeftPoint = new Point(5, 5);
            var wheelTopPoint = new Point(topLeftPoint.X + 5, topLeftPoint.Y + 5);
            var wheelCenter = new Point(wheelTopPoint.X + wheelSize.Width / 2
                                          , wheelTopPoint.Y + wheelSize.Height / 2);


            var outerWheel = new Circle(wheelSize.Width < wheelSize.Height
                                          ? wheelSize.Width / 2
                                          : wheelSize.Height / 2
                                            , wheelCenter);
            var outerMainWheel = new Circle(outerWheel.Radius - mainWheelDiffRatio * outerWheel.Radius, wheelCenter);

            var wheel = new Circle(outerWheel.Radius, wheelCenter);
            var mainWheel = new Circle(wheel.Radius - mainWheelDiffRatio * wheel.Radius, wheelCenter);
            var rulerWheel = new Circle(wheel.Radius - rulerWheelDiffRatio * wheel.Radius, wheelCenter);

            var SmallRulerWheel = new Circle(wheel.Radius - smallRulerWheelDiffRatio * wheel.Radius, wheelCenter);

            var halferWheel = new Circle(mainWheel.Radius - halferWheelDiffRatio * wheel.Radius, wheelCenter);
            var innerWheel = new Circle(mainWheel.Radius - innerWheelDiffRatio * wheel.Radius, wheelCenter);
            var planetStickWhell = new Circle(innerWheel.Radius - planetStickDiffRatio * wheel.Radius, wheelCenter);
            var planetWheel = new Circle(innerWheel.Radius - planetWheelDiffRatio * wheel.Radius, wheelCenter);
            var aspectWheel = new Circle(planetWheel.Radius - aspectWheelDifRatio * wheel.Radius, wheelCenter);
            var houseNrVheel = new Circle(houseWheelDiffRatio * wheel.Radius, wheelCenter);
            var brush = drawingBrush;
            const string fontName = "Esoterik";
            var fontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#HamburgSymbols");
            var  typeFace = fontFamily.GetTypefaces().FirstOrDefault();
           
            var borderRectangle = new Rect(topLeftPoint, squareSize);
            dc.DrawLine(strongBluePen, new Point(wheelCenter.X - wheel.Radius, wheelCenter.Y), new Point(wheelCenter.X + wheel.Radius, wheelCenter.Y));
            dc.DrawRoundedRectangle(brush, drawingPen, borderRectangle,
                new CornerRadius { BottomLeft = 5, BottomRight=5, TopLeft=5, TopRight=5});
            wheel.Draw(dc, brush, linePen);
            mainWheel.Draw(dc, brush, strongLinePen);
            innerWheel.Draw(dc, brush, strongBluePen);
            aspectWheel.Draw(dc, brush, linePen); 
            
            var springPointOffset = new LongitudeAstrolabium(180 - EphemerisData.Ascendant.Cusp);

            #region fokbeosztás kirajzolása
            for (var i = springPointOffset.Value; i < 360 + springPointOffset.Value; i++)
            {
                if ((int) i % 5 == 0)
                {
                    innerWheel.DrawRadialSection(dc, linePen, i, rulerWheel);
                }
                else
                {
                    innerWheel.DrawRadialSection(dc, linePen, i, SmallRulerWheel);
                }
            }
            #endregion

            #region jelek kirajzolása

            var plan = 1;
            for (var i = springPointOffset.Value; i < 360 + springPointOffset.Value; i += 30, plan++)
            {
                mainWheel.DrawRadialSection(dc, linePen, i, innerWheel);
                var house = new ZodiacHouse((ZodiacHouse.Name)plan);
                var middlePoint = halferWheel.GetPointAtAngle(i + 15);
                
               
                var txt = new FormattedText(house.GetSignString()
                    , CultureInfo.InvariantCulture
                    , FlowDirection.LeftToRight
                    , typeFace
                    //, new Typeface(fontName)
                    , (int)Math.Round(signCharacterDiffRatio * wheel.Radius)
                    , Brushes.Black
                    , 1);
                middlePoint.X -= txt.Width / 2;
                middlePoint.Y -= txt.Height / 2;
                dc.DrawText(txt, middlePoint);
            }
            #endregion

            #region házak kirajzolása

            for (var i = 0; i < 12; i++)
            {

                if (i == 3 ||  i == 9  )
                    innerWheel.DrawRadial(dc, strongBluePen, springPointOffset.Value + EphemerisData.Houses[i].Cusp);
                else
                    innerWheel.DrawRadial(dc, blackLinePen, springPointOffset.Value + EphemerisData.Houses[i].Cusp);
                var txt = new FormattedText(GetHouseString(i)
                                          , CultureInfo.InvariantCulture
                                          , FlowDirection.LeftToRight
                                          , new Typeface(fontName)
                                          , (int)Math.Round(houseCharacterDiffRatio * wheel.Radius)
                                          , Brushes.Black
                                          , 1);

                LongitudeAstrolabium houseCusp;
                LongitudeAstrolabium nextHouseCusp;

                if (i != 11)
                {
                    houseCusp = new LongitudeAstrolabium(EphemerisData.Houses[i].Cusp);
                    nextHouseCusp = new LongitudeAstrolabium(EphemerisData.Houses[i + 1].Cusp);

                }
                else
                {
                    houseCusp = new LongitudeAstrolabium(EphemerisData.Houses[i].Cusp);
                    nextHouseCusp = new LongitudeAstrolabium(EphemerisData.Houses[0].Cusp);

                }
                var houseMidle = houseCusp % nextHouseCusp;
                var middlePoint = houseNrVheel.GetPointAtAngle(houseMidle.Value - 180 + springPointOffset.Value);
                middlePoint.X -= txt.Width / 2;
                middlePoint.Y -= txt.Height / 2;
                dc.DrawText(txt, middlePoint);
            }
            #endregion

            #region Bolygók kirajzolása

            var orderedPlanets = EphemerisData.Planets.OrderByDescending(p=>p.Longitude).ToList();
            foreach (var body in orderedPlanets)
            {
                var txt = new FormattedText(body.PlanetString
                                                      , CultureInfo.InvariantCulture
                                                      , FlowDirection.LeftToRight
                                                      , typeFace
                                                      , (int)Math.Round(bodyCharacterDiffRatio * wheel.Radius)
                                                      , Brushes.Black
                                                      , 1);

                var planetPoint = planetWheel.GetPointAtAngle((springPointOffset + body.Longitude).Value);
                var bodyRect = new Rect(new Size(Math.Max(txt.Width, txt.Height), Math.Max(txt.Width, txt.Height)));
                bodyRect.Location = planetPoint;
                //eltolás balra fel (a planéta pont a négyzet közepén legyen)
                bodyRect.Offset(new Vector(-bodyRect.Width/2,-bodyRect.Height/2));               
                var runningLongitude =  body.Longitude;
                Point runningPlanetPoint = planetPoint;
                Rect runningPlanetRect = bodyRect;                
                while (_occupedRectangles.IntersectsWith(runningPlanetRect))
                {
                    var conflictedPlanet = _occupedRectangles.GetIntersect(runningPlanetRect);
                    double direction;
                   if( body.Latitude < conflictedPlanet.Item1.Longitude)
                    {
                        direction = -1;
                    }
                    else { 
                        direction = 1; 
                    }
                    runningPlanetPoint = planetWheel.GetPointAtAngle((springPointOffset + (runningLongitude+=direction)).Value);
                    runningPlanetRect.Location = runningPlanetPoint;
                    runningPlanetRect.Offset(new Vector(-runningPlanetRect.Width / 2, -runningPlanetRect.Height / 2));
                }
                dc.DrawLine(linePen,
                    innerWheel.GetPointAtAngle((springPointOffset + body.Longitude).Value),
                     runningPlanetPoint);
                _occupedRectangles.Add(body, runningPlanetRect);
                dc.DrawTextintoCenterOfRect(runningPlanetRect,txt);
            }

            #endregion

            #region Fényszögek kirajzolása
            foreach (var aspect in EphemerisData.AspectsDict)
            {
                if (aspect.Value.AspectType != AspectType.Conjunction 
                    && aspect.Value.AspectType != AspectType.Opposition
                    && aspect.Value.AspectType != AspectType.Noaspect)
                {
                    var point1 = aspectWheel.GetPointAtAngle((springPointOffset + aspect.Value.Body1.Longitude).Value);
                    var point2 = aspectWheel.GetPointAtAngle((springPointOffset + aspect.Value.Body2.Longitude).Value);
                    dc.DrawLine(linePen, point1, point2);
                }
            }
            #endregion
        }
    }

    internal class Rectangles : Dictionary<PlanetValues, Rect>
    {
        public bool IntersectsWith(Rect other) => this.Any(ra => ra.Value.IntersectsWith(other));

        public (PlanetValues,Rect) GetIntersect(Rect other)
        {
            foreach (var rect in this.Where(rect => !Equals(rect.Value, other))
                                     .Where(rect => rect.Value.IntersectsWith(other)))
            {
                return (rect.Key, rect.Value);
            }

            throw new InvalidOperationException("Nincs ütközés");
        }

        public Planet? PlanetAtPoint(Point p)
        {
            var aRect = this.Where(kvp => kvp.Value.Contains(p)).Select(kvp => kvp.Key).FirstOrDefault();
            if (aRect != null)
            return new Planet( aRect.Planet.Id);
            return null;
        }

    }

    static class DrawingContextExtension
    {
        /// <summary>
        /// Draws a rounded rectangle with four individual corner radius
        /// </summary>
        public static void DrawRoundedRectangle(this DrawingContext dc, Brush brush,
            Pen pen, Rect rect, CornerRadius cornerRadius)
        {
            var geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                bool isStroked = pen != null;
                const bool isSmoothJoin = true;

                context.BeginFigure(rect.TopLeft + new Vector(0, cornerRadius.TopLeft), brush != null, true);
                context.ArcTo(new Point(rect.TopLeft.X + cornerRadius.TopLeft, rect.TopLeft.Y),
                    new Size(cornerRadius.TopLeft, cornerRadius.TopLeft),
                    90, false, SweepDirection.Clockwise, isStroked, isSmoothJoin);
                context.LineTo(rect.TopRight - new Vector(cornerRadius.TopRight, 0), isStroked, isSmoothJoin);
                context.ArcTo(new Point(rect.TopRight.X, rect.TopRight.Y + cornerRadius.TopRight),
                    new Size(cornerRadius.TopRight, cornerRadius.TopRight),
                    90, false, SweepDirection.Clockwise, isStroked, isSmoothJoin);
                context.LineTo(rect.BottomRight - new Vector(0, cornerRadius.BottomRight), isStroked, isSmoothJoin);
                context.ArcTo(new Point(rect.BottomRight.X - cornerRadius.BottomRight, rect.BottomRight.Y),
                    new Size(cornerRadius.BottomRight, cornerRadius.BottomRight),
                    90, false, SweepDirection.Clockwise, isStroked, isSmoothJoin);
                context.LineTo(rect.BottomLeft + new Vector(cornerRadius.BottomLeft, 0), isStroked, isSmoothJoin);
                context.ArcTo(new Point(rect.BottomLeft.X, rect.BottomLeft.Y - cornerRadius.BottomLeft),
                    new Size(cornerRadius.BottomLeft, cornerRadius.BottomLeft),
                    90, false, SweepDirection.Clockwise, isStroked, isSmoothJoin);

                context.Close();
            }
            dc.DrawGeometry(brush, pen, geometry);
        }

        public static void DrawTextintoCenterOfRect(this DrawingContext dc, Rect rect, FormattedText txt)
        {
            dc.DrawText(txt,
                   new Point(rect.TopLeft.X + (rect.Width - txt.Width) / 2,
                   rect.TopLeft.Y + (rect.Height - txt.Height) / 2));
        }
    }

    public static class VectorExtensions
    {
        public static Vector Rotate(this Vector v, double degrees) => new Vector(
                (v.X * Math.Cos(degrees) - v.Y * Math.Sin(degrees)),
                (v.X * Math.Sin(degrees) + v.Y * Math.Cos(degrees))
            );

        public static Vector Rotate90Degress(this Vector v) => v.Rotate(Math.PI / 90);

    }

    public static class RectExtensions

    {
        public static Point GetMiddle(this Rect rect) => new Point(rect.Left - rect.Width / 2, rect.Top - rect.Height / 2);
    }

}
