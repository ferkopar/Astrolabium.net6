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

namespace Astrolabium.UserControls.Wiews
{
    /// <summary>
    /// Interaction logic for RadixDetailsUserControlView.xaml
    /// </summary>
    public partial class RadixDetailsUserControlView : UserControl
    {
        public Native Native
        {
            get { return (Astrolabium.Models.Native)GetValue(NativeProperty); }
            set { SetValue(NativeProperty, value);
                PlanetListBox.ItemsSource = value.EphemerisData.Planets;
            }
        }

        // Using a DependencyProperty as the backing store for Native.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NativeProperty =
            DependencyProperty.Register("Native", typeof(Astrolabium.Models.Native), typeof(RadixDetailsUserControlView), new PropertyMetadata(null));

        public RadixDetailsUserControlView()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
