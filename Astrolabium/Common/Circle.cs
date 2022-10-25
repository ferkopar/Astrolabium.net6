using System;
using System.Windows;
using System.Windows.Media;

namespace Astrolabium.Common
{
    class Circle
    {
        private Point _origo;
        private readonly double _radius;
        public Circle(double radius, Point origo)
        {
            _radius = radius;
            _origo = origo;
        }
        public Circle(double radius, double x, double y) : this(radius, new Point(x, y)) { }
        public double Radius => _radius;
        public double X => _origo.X;
        public double Y => _origo.Y;
        public Point GetPointAtAngle(double angle)
        {
            const double degRad = Math.PI / 180;
            var retVal = new Point
            {
                Y = _origo.Y - (Math.Sin((angle) * degRad) * _radius),
                X = Math.Cos((angle) * degRad) * _radius + _origo.X
            };
            return retVal;
        }
        public void Draw(DrawingContext dc, Brush brush, Pen strongBluePen)
        {
            dc.DrawEllipse(brush, strongBluePen,_origo,_radius,_radius);
        }
        public void DrawRadial(DrawingContext dc, Pen pen,double angle)
        {
            dc.DrawLine(pen,_origo,GetPointAtAngle(angle));
        }
        public void DrawRadialSection(DrawingContext dc, Pen pen,double angle,Circle other)
        {
            if(_origo == other._origo)
            {
                dc.DrawLine(pen, other.GetPointAtAngle(angle), GetPointAtAngle(angle));
            }
            else
            {
                throw new ArgumentException("Csak koncentrikus körökkel lehet sugárdarabot rajzolni");
            }
        }
    }
}
