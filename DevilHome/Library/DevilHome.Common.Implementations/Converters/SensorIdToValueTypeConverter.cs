using System;
using Windows.UI.Xaml.Data;

namespace DevilHome.Common.Implementations.Converters
{
    public class SensorIdToValueTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return "";
            }

            return (int)value == 1 ? " °C" : " %";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}