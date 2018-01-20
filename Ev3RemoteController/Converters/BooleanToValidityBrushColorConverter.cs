using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Smallrobots.Ev3RemoteController.Converters
{
    public class BooleanToValidityBrushColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool) value)
            {
                return new SolidColorBrush(Windows.UI.Color.FromArgb(80, 0, 0, 0));
            }
            else
            {
                return new SolidColorBrush(Windows.UI.Color.FromArgb(80, 255, 0, 0));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
