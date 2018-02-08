using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Smallrobots.Ev3RemoteController.Converters
{
    public class SpeedToRotationConverter : IValueConverter
    { 
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int inValue;
            if (int.TryParse(value.ToString(), out inValue))
            {
                return (int)(90.0 + inValue / 100.0 * 135.0);
            }
            else
                return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
