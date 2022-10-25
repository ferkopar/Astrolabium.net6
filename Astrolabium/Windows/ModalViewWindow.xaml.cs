using Astrolabium.Models;
using Astrolabium.UserControls.Wiews;
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

namespace Astrolabium.Windows
{
    /// <summary>
    /// Interaction logic for ModalViewWindow.xaml
    /// </summary>
    public partial class ModalViewWindow : Window
    {
        public event EventHandler<NativeEditorEventArgs> RaiseEndEdit;
        private Native native;
        public bool NewNative { get; set; }
        public Native Native
        {
            get
            {
                return native;
            }
            set
            {
                native = value;
                EditorControl.ViewModell.Native = value;
            }
        }
        public ModalViewWindow()
        {
            InitializeComponent();
        }
        private void NativeEditorView_RaiseEndEdit(object sender, UserControls.Wiews.NativeEditorEventArgs e)
        {
            OnRaiseEndEdit(e);
        }
        protected virtual void OnRaiseEndEdit(NativeEditorEventArgs e) => RaiseEndEdit?.Invoke(this, e);
    }
}
