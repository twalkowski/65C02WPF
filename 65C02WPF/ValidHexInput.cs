using System.Globalization;
using System.Windows.Controls;

namespace _65C02WPF
{
    class ValidHexInput : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse((string)value, NumberStyles.HexNumber, cultureInfo, out int NewValue))
            {
                if (NewValue >= 0 & NewValue <= 0xff)
                {
                    return ValidationResult.ValidResult;   /// Input is valid Hex
                }
                else
                {
                    return new ValidationResult(false, "Value is out of range");
                }
            }
            else
            {
                return new ValidationResult(false, "Value is not a hex number");  /// invalid hex input
            }
        }
    }
}

