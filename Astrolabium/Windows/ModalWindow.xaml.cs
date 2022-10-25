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
using System.Windows.Shapes;
using SweCore;
using SweCore.Planets;
using Astrolabium.Models;
using Astrolabium.ViewModels;

namespace Astrolabium.Windows
{
    /// <summary>
    /// Interaction logic for ModalWindow.xaml
    /// </summary>
    public partial class ModalWindow : Window
    {
        // Dependency Property
        public static readonly DependencyProperty PlanetNameProperty =
            DependencyProperty.Register("PlanetName", typeof(string),
                typeof(ModalWindow));
        // .NET Property wrapper
        public string PlanetName
        {
            get => (string)GetValue(PlanetNameProperty);
            set
            {
                SetValue(PlanetNameProperty, value);
                InvalidateVisual();
            }
        }
        // Dependency Property
        public static readonly DependencyProperty EphemerisDataProperty =
            DependencyProperty.Register("EphemerisData", typeof(EphemerisResult),
                typeof(ModalWindow));
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

        public static readonly DependencyProperty PlanetDataProperty =
            DependencyProperty.Register("PlanetData", typeof(Planet),
                typeof(ModalWindow));
        // .NET Property wrapper
        public Planet PlanetData
        {
            get => (Planet)GetValue(PlanetDataProperty);
            set
            {
                SetValue(PlanetDataProperty, value);
                InvalidateVisual();
            }
        }
        public ModalWindow()
        {
            InitializeComponent();

        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            var texts = new ModalWindowsViewModell();
            txtSomeBox.Text = $" {PlanetName}  {EphemerisData.GetPlanetInSign(PlanetData)}-ban a {EphemerisData.GetPlanetInHouse(PlanetData)}-ban.";
            var signName = EphemerisData.GetPlanetInSign(PlanetData).SignName;
            var signTxt = texts.GetPlantSignText(PlanetName, signName);
            RichTextBox.Document.Blocks.Add(new Paragraph(new Run(signTxt.Text)));
        }

        private void btnSaveData_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
