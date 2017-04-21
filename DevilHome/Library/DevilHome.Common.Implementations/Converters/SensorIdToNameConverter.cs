using System;
using Windows.UI.Xaml.Data;
using DevilHome.Common.Interfaces.Enums;

namespace DevilHome.Common.Implementations.Converters
{
    public class SensorIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return "";
            }

            return (int)value == 1 ? SensorEnum.Temperatur.GetStringValue() : SensorEnum.Humidity.GetStringValue();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}