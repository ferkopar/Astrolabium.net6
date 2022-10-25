using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Astrolabium.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Astrolabium.CustomControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Astrolabium.CustomControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Astrolabium.CustomControls;assembly=Astrolabium.CustomControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class AspectViewer : Control
    {


        public Native Native
        {
            get { return (Native)GetValue(NativeProperty); }
            set { SetValue(NativeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Native.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NativeProperty =
            DependencyProperty.Register("Native", typeof(Native), typeof(AspectViewer), new PropertyMetadata(Native.Default));


        static AspectViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AspectViewer), new FrameworkPropertyMetadata(typeof(AspectViewer)));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            var fontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/#HamburgSymbols");
            var typeFace = fontFamily.GetTypefaces().FirstOrDefault();

            var drawingPen = new Pen(Brushes.SteelBlue, 3);
            var thinkPen = new Pen(Brushes.SteelBlue, 1);

            Brush drawingBrush = Brushes.AliceBlue;
            Brush selectedDrawingBrush = Brushes.LightGoldenrodYellow;

            var squareSize = new Size(ActualWidth - 10, ActualHeight - 10);
            var topLeftPoint = new Point(5, 5);
            var borderRectangle = new Rect(topLeftPoint, squareSize);

            drawingContext.DrawRoundedRectangle(drawingBrush, drawingPen, borderRectangle,
                new CornerRadius { BottomLeft = 5, BottomRight = 5, TopLeft = 5, TopRight = 5 });

            var bodies = Native.EphemerisData.Planets;
            var bodiesNo = bodies.Count;
            var maxSideLength = borderRectangle.BottomLeft.Y - borderRectangle.TopLeft.Y;
            var bodySquareSide = maxSideLength / (bodiesNo + 1);
            var bodySquare = new Rect(new Size(bodySquareSide, bodySquareSide));

            var runningSquare = Rect.Offset(bodySquare, new Vector(10, maxSideLength - bodySquare.Height));


            for (int i = bodiesNo - 1; i >= 0; i--)
            {
                drawingContext.DrawRectangle(drawingBrush, thinkPen, runningSquare);
                FormattedText txt = getFormattedText(bodies[i].PlanetString, typeFace, bodies, ref runningSquare, i);
                drawingContext.DrawTextintoCenterOfRect(runningSquare, txt);
                runningSquare.Offset(new Vector(0, -bodySquare.Height - 2));
            }

            runningSquare = Rect.Offset(bodySquare, new Vector(10 + bodySquare.Width, maxSideLength - bodySquare.Height));

            for (int j = bodiesNo; j >= 0; j--)
            {
                for (int i = bodiesNo; i >= j + 1; i--)
                {
                    drawingContext.DrawRectangle(drawingBrush, thinkPen, runningSquare);
                    if ((i >= 0) && (i == j + 1))
                    {
                        FormattedText txt = getFormattedText(bodies[i - 1].PlanetString, typeFace, bodies, ref runningSquare, i);
                        drawingContext.DrawTextintoCenterOfRect(runningSquare, txt);
                    }
                    else
                    {
                        var aspect = new Aspect(bodies[i-1], bodies[j]);
                        if (aspect != null && aspect.AspectType != AspectType.Noaspect)
                        {
                            FormattedText txt = getFormattedText(aspect.AspectDisplayChar, typeFace, bodies, ref runningSquare, i);
                            drawingContext.DrawTextintoCenterOfRect(runningSquare, txt);
                        }

                    }              
                    runningSquare.Offset(new Vector(0, -bodySquare.Height - 2));
                }
                runningSquare = Rect.Offset(bodySquare, new Vector(10 + j * bodySquare.Width, maxSideLength - bodySquare.Height));

            }

            static FormattedText getFormattedText(string txt, Typeface typeFace, List<PlanetValues> bodies, ref Rect runningSquare, int i)
            {
                return new FormattedText(txt
                                                                      , CultureInfo.InvariantCulture
                                                                      , FlowDirection.LeftToRight
                                                                      , typeFace
                                                                      , runningSquare.Width / 2
                                                                      , Brushes.Black
                                                                      , 1);
            }
        }
    }
}
