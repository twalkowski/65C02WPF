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

    [ValueConversion(typeof(byte), typeof(String))]
    public class ByteToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((byte)value).ToString("x2");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
