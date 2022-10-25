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
using Astrolabium.Models;
//using Astrolabium.net.Models;
using Astrolabium.UserControls.ViewModels;
using Astrolabium.UserControls.Wiews;
using Astrolabium.Windows;
using SweCore;

namespace Astrolabium.UserControls.Wiews
{
        public delegate void NativeChangedEventHandler(object sender, ChartSettingsEventArgs e);

    public class ChartSettingsEventArgs : EventArgs
    {
        public Native SelectedNative { get; }
        public ChartTypes  ChartType { get; }
        public ChartSettingsEventArgs(Native selectedNative, ChartTypes chartType )
        {
            SelectedNative = selectedNative;
            ChartType = chartType;
        }
    }
    /// <summary>
    /// Interaction logic for ChartSettingsUserControl1Viewx.xaml
    /// </summary>
    public partial class ChartSettingsUserControl1View : UserControl
    {
        public event EventHandler<ChartSettingsEventArgs> NativeChanged;

        private ChartSettingsUserControl1ViewModell viewModell = new();
        private InputCalculation  inputCalculation = new InputCalculation();
        private Native selectedNative;
        private HouseSystem houseSystem = HouseSystem.Placidus;

        public Native Native
        {
            get { return (Native)GetValue(NativeProperty); }
            set { SetValue(NativeProperty, value); }
        }

        internal ChartSettingsUserControl1ViewModell ViewModell 
        { 
            get => ViewModell; 
            set => ViewModell = value; 
        }

        // Using a DependencyProperty as the backing store for Native.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NativeProperty =
            DependencyProperty.Register("Native", typeof(Native),
                typeof(ChartSettingsUserControl1View),
                new PropertyMetadata(Native.Default));
        
        public ChartSettingsUserControl1View()
        {
            InitializeComponent();
            DataContext = this;
            BodyListBox.ItemsSource = viewModell.ShowBody;

            inputCalculation.Planets.Clear();
            foreach (var body in viewModell.ShowBody.Where(b => b.Showed))
            {
                inputCalculation.Planets.Add(body.Body);
            }
            HouseSystems.AddRange(Enum.GetNames(typeof(HouseSystem)));
            ChartTypes.AddRange(Enum.GetNames(typeof(ChartTypes)));
        }
        public List<string> HouseSystems { get; } = new List<string>();
        public HouseSystem HouseSystem { get => houseSystem; set => houseSystem = value; }
        public List<string> ChartTypes { get; } = new List<string>();
        public ChartTypes ChartType { get; set; }
        private void NativeComboSelectionChanged(object sender, NativeComboEventArgs e)
        {
            if (e == null) return;
            var native = e.SelectedNative;
            if (native != null)
            {
                selectedNative = native;
                RecalculateChart(native);
            }
        }

        private void RecalculateChart(Native native)
        {
            inputCalculation.DateLocal = native.BirthDate;
            inputCalculation.Latitude = native.BirthLocation.GeoPos.Latitude;
            inputCalculation.Longitude = native.BirthLocation.GeoPos.Longitude;
            inputCalculation.HouseSystem = houseSystem;
            native.CalculateEphemerisData(inputCalculation);
            OnRaiseSelectionChanged(new ChartSettingsEventArgs(native,ChartType));
        }
        protected virtual void OnRaiseSelectionChanged(ChartSettingsEventArgs e) => NativeChanged?.Invoke(this, e);

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var showBody=(ShowBody)((sender as CheckBox).Tag);

            var showBodyInViewModell = viewModell.ShowBody.First(s=>s.Name == showBody.Name);
            var isChecked = (sender as CheckBox).IsChecked;

            if(isChecked == true) 
            {
                if (!inputCalculation.Planets.Contains(showBody.Body))
                {
                    inputCalculation.Planets.Add(showBody.Body);
                }
            }
            else
            {
                if (inputCalculation.Planets.Contains(showBody.Body)) 
                {
                    inputCalculation.Planets.Remove(showBody.Body);
                }
            }
            if(selectedNative != null) 
            {
                RecalculateChart(selectedNative);
            }
            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = sender as ComboBox;
            var selectedHouseSystem = combo.SelectedItem;
            var ize = houseSystem;
            if (selectedNative != null)
            {
                RecalculateChart(selectedNative);
            }
        }
    }

    public enum ChartTypes{ Radix,Aspectarian}
}
