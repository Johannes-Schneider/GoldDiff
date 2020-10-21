using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GoldDiff.Shared.View.Converter
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public Visibility TrueValue { get; set; } = Visibility.Visible;
        public Visibility FalseValue { get; set; } = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool boolValue))
            {
                throw new ArgumentException($"{nameof(value)} must be a {nameof(Boolean)}!");
            }

            return boolValue ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility visibilityValue))
            {
                throw new ArgumentException($"{nameof(value)} must be a {nameof(Visibility)}!");
            }

            if (visibilityValue == TrueValue)
            {
                return true;
            }

            if (visibilityValue == FalseValue)
            {
                return false;
            }
            
            throw new ArgumentException($"{nameof(value)} can not be converted to a {nameof(Boolean)}!");
        }
    }
}