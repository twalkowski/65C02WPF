using System;
using System.Globalization;
using System.Windows.Data;

namespace _65C02WPF
{
    [ValueConversion(typeof(int), typeof(String))]
    public class IntToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value).ToString("x2");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
