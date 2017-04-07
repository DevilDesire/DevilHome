using System;
using Windows.UI.Xaml;
using DevilDesireDevLib.Interfaces;

namespace DevilDesireDevLib.Implementation.Converter
{
    public class BoolToVisibilityConverter : IConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}