using Astrolabium.Models;
using Astrolabium.UserControls.ViewModels;
using Astrolabium.Windows;
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

namespace Astrolabium.UserControls.Wiews
{
    public class NativeListEventArgs : EventArgs
    {
      public Native SelectedNative { get; }
      public NativeListEventArgs(Native selectedNative)
        {
            SelectedNative = selectedNative;
        }
    }
    public delegate void NativelistButtonClickedEventHandler(object sender, NativeListEventArgs e);
   
    /// <summary>
    /// Interaction logic for NativeListView.xaml
    /// </summary>
    public partial class NativeListView : UserControl
    {
        public event EventHandler<NativeListEventArgs> RaiseNativelistButtonClicked;

        private NativeListViewModell viewModell;
        public NativeListView()
        {

            InitializeComponent();
            //viewModell = NativeListViewModell.GenerateTestModell();
            viewModell = NativeListViewModell.ReadFromDatabase();
            DataContext = viewModell;
            //NativeListViewControl.ItemsSource = viewModell.NativeList;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var ize = button.DataContext as Native;
            OnRaiseNativelistButtonClicked(new NativeListEventArgs(button.DataContext as Native));
        }
        protected virtual void OnRaiseNativelistButtonClicked(NativeListEventArgs e) => RaiseNativelistButtonClicked?.Invoke(this, e);

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
                OnRaiseNativelistButtonClicked(new NativeListEventArgs(e.Native));
            }
            (sender as Window).Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var x = sender as Button;
            var nativ = x.Tag as Native;
            var w = new ModalViewWindow();
            w.NewNative = false;
            w.Native = nativ;
            w.RaiseEndEdit += W_RaiseEndEdit;
            w.ShowDialog();
        }
    }  
}

