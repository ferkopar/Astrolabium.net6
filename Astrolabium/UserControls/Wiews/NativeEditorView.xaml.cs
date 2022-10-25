using Astrolabium.UserControls.ViewModels;
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
    public class NativeEditorEventArgs : EventArgs
    {
        public Native Native { get; }
        public bool IsCanceled {get;}
        public NativeEditorEventArgs(Native native, bool isCanceled = false)
        {
            Native = native;
            IsCanceled = isCanceled;
        }
    }
    /// <summary>
    /// Interaction logic for NativeEditorView.xaml
    /// </summary>
    public partial class NativeEditorView : UserControl
    {
        public event EventHandler<NativeEditorEventArgs> RaiseEndEdit;
        private NativeEditorViewModell viewModell;
        public NativeEditorViewModell ViewModell
        {
            get
            {
                return viewModell;
            }
            set
            {
                viewModell = value;
                
            }
        }
        //public DateTime TesztDatum { get; } = new DateTime(1956, 11, 4);
        public DateTime TesztDatum
        {
            get { return viewModell.TesztDatum; }
        }
        public NativeEditorView()
        {
            viewModell = new NativeEditorViewModell();
            InitializeComponent();
            DataContext = viewModell;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var n = viewModell.Native;
            n.WriteToDatabase();
            OnRaiseEndEdit(new NativeEditorEventArgs(n));
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var n = viewModell.Native;
            OnRaiseEndEdit(new NativeEditorEventArgs(n,true));
        }
        protected virtual void OnRaiseEndEdit(NativeEditorEventArgs e) => RaiseEndEdit?.Invoke(this, e);
        private void SexButton_Click(object sender, RoutedEventArgs e)
        {
            if(viewModell.Native.Sex == SexEnum.Female)
            {
                viewModell.Native.Sex = SexEnum.Male;
            }
            else
            {
                viewModell.Native.Sex = SexEnum.Female;
            }
            viewModell.SexUri =new Uri($"pack://application:,,,/Astrolabium;component/Icons/Male-icon.png");
        }

        private void DateOkButton_Click(object sender, RoutedEventArgs e)
        {
            viewModell.Native.BirthDateIsCorrect = !viewModell.Native.BirthDateIsCorrect;
            viewModell.BirthDateOKUri = new Uri($"pack://application:,,,/Astrolabium;component/Icons/Male-icon.png");
        }
    }
}
