using System;
using DevilDesireDevLib.Interfaces;

namespace DevilDesireDevLib.Implementation.Converter
{
    public class UnixTimestampToStringConverter : IConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                DateTime toConvert = new DateTime(1970, 1, 1).AddSeconds(System.Convert.ToDouble(value)).ToLocalTime();
                return toConvert.ToString("dd.MM.yyyy HH:mm:ss");
            }
            catch (Exception)
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}