using Astrolabium.Windows;
using Astrolabium.Sevices;
using System;
using System.Collections.Generic;
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
using Astrolabium.UserControls.Wiews;
using Astrolabium.CustomControls;

namespace Astrolabium
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Bigyo.GeoNames = MainGeoName.GeoNames;
        }
        
        private void Wheel_MouseClickPlanet(object source, CustomControls.WheelPanelEventArgs args)
        {
            //var modalWindow = new ModalWindow();
            var modalWindow = new ModalWindow {  EphemerisData = args.Result, PlanetName = args.Values.PlanetName };
            if (args.Planet != null) modalWindow.PlanetData = args.Planet.Value.Id;
            modalWindow.ShowDialog();
        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetPlanetShow_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new WheelSettingsView();
        }

        private void NativeChanged(object sender, ChartSettingsEventArgs e)
        {
            if (e.ChartType == ChartTypes.Radix)
            {
                var vheelPanel = new CustomControls.WheelPanel(e.SelectedNative);
                vheelPanel.MouseClickPlanet += Wheel_MouseClickPlanet;
                MainContent.Content = vheelPanel;
            }
            else
            {
                var aspectViewer = new AspectViewer { Native = e.SelectedNative };
                MainContent.Content = aspectViewer;
            }
            RadixDetails.Native = e.SelectedNative;
        }

        private void Wheel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //MainContent.Width = (sender as WheelPanel).ActualWidth;
            //MainContent.Height = (sender as WheelPanel).ActualHeight; 
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //RadixDetails.Width = ActualWidth - Wheel.Height;
        }
    }
}
