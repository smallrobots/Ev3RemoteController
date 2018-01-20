using System;
using Windows.UI.Xaml.Data;
using Smallrobots.Ev3RemoteController.ViewModels;

namespace Smallrobots.Ev3RemoteController.Converters
{
    public class ConnectionStatusToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((ConnectionState)value == ConnectionState.CanConnect_Or_Ping)
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
