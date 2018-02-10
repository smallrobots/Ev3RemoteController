using System;
using Windows.UI.Xaml.Data;

namespace Smallrobots.Ev3RemoteController.Converters
{
    public class PolarGuage_AngleToRotationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int inValue;
            if (int.TryParse(value.ToString(), out inValue))
            {
                return (int)(- 90.0 + inValue);
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
