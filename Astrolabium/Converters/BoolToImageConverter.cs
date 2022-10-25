using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Astrolabium.Converters
{
    
    public class BoolToImageConverter : IValueConverter
    {
        public Image TrueImage { get; set; }
        public Image FalseImage { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //if (!(value is bool))
            //{
            //    return null;
            //}

            //bool b = (bool)value;
            //if (b)
            //{
            //    return this.TrueImage;
            //}
            //else
            //{
            //    return this.FalseImage;
            //}
            var uri = new Uri("pack://application:,,,/Astrolabium;component/Icons/icons8-checkmark-48.png");

            return uri;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
