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
using Astrolabium.UserControls.ViewModels;
using Astrolabium.Windows;

namespace Astrolabium.UserControls.Wiews
{
    public class NativeComboEventArgs : EventArgs
    {
        public Native SelectedNative { get; }
        public NativeComboEventArgs(Native selectedNative)
        {
            SelectedNative = selectedNative;
        }
    }

    public delegate void SelectionChangedEventHandler(object sender, NativeComboEventArgs e);

    /// <summary>
    /// Interaction logic for NativeComboAndButtonsUserControl1xaml.xaml
    /// </summary>
    public partial class NativeComboAndButtonsUserControl : UserControl
    {
        public event EventHandler<NativeComboEventArgs> SelectionChanged;

        private NativeListViewModell viewModell;

        public NativeComboAndButtonsUserControl()
        {
            InitializeComponent();
            ViewModell = NativeListViewModell.ReadFromDatabase();
            NativeComboBox.ItemsSource=ViewModell.NativeList;
            DataContext = this;
        }

        internal NativeListViewModell ViewModell { get => viewModell;  set => viewModell = value; }

        private void AddNative(object sender, RoutedEventArgs e)
        {
            var w = new ModalViewWindow();
            w.NewNative = true;
            w.RaiseEndEdit += W_RaiseEndEdit;
            w.ShowDialog();
        }

        private void W_RaiseEndEdit(object sender, NativeEditorEventArgs e)
        {
            if (!e.IsCanceled)
            {
                if ((sender as ModalViewWindow).NewNative)
                {
                    viewModell.NativeList.Add(e.Native);
                }
                //OnRaiseNativelistButtonClicked(new NativeListEventArgs(e.Native));
            }
            (sender as Window).Close();
        }

        private void NativeSelectionChanged(object sender, SelectionChangedEventArgs e) => OnRaiseSelectionChanged(new NativeComboEventArgs((sender as ComboBox).SelectedItem as Native));
        protected virtual void OnRaiseSelectionChanged(NativeComboEventArgs e) => SelectionChanged?.Invoke(this, e);

        private void EditNative(object sender, RoutedEventArgs e)
        {
           
            var w = new ModalViewWindow();
            w.NewNative = false;
            w.Native = NativeComboBox.SelectedItem as Native;
            w.RaiseEndEdit += W_RaiseEndEdit;
            w.ShowDialog();
        }

        private void DeleteNative(object sender, RoutedEventArgs e)
        {
            var native = NativeComboBox.SelectedItem as Native;  
            viewModell.NativeList.Remove(native);
            native.DeleteFromDatabase();
         }
    }
}