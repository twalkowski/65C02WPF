using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace _65C02WPF
{
    /// <summary>
    /// Value converter (string to int) for textbox target bound to an integer source property 
    /// </summary>
    public class HexConverter : IValueConverter
    {
        /// <summary>
        /// Convert is called when the source is updating the target 
        /// 
        /// See the IValueConverter interface for more information
        /// </summary>
        /// <param name="value">The source value to convert - integer for this case</param>
        /// <param name="targetType">the type of the target property - string for the textbox</param>
        /// <param name="parameter">a user-defined parameter</param>
        /// <param name="culture">the culture info</param>
        /// <returns> the converted value - string</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;   /// no custom conversion needed when updating the target
        }

        /// <summary>
        /// ConvertBack is called when the target is updating the source 
        /// 
        /// See the IValueConverter interface for more information
        /// </summary>
        /// <param name="value">The value to convert - string from the textbox</param>
        /// <param name="targetType">the type of the source property - integer in this case</param>
        /// <param name="parameter">a user-defined parameter</param>
        /// <param name="culture">the culture info</param>
        /// <returns> the converted value - integer</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse((string)value, NumberStyles.HexNumber, culture, out int HexValue))
            {
                return HexValue;   /// return the hex value of the input string
            }
            else
            {
                return null;
            }
        }
    }
}
