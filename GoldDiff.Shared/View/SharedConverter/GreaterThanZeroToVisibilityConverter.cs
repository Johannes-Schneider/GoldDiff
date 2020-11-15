using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GoldDiff.Shared.View.SharedConverter
{
    [ValueConversion(typeof(int), typeof(Visibility))]
    [ValueConversion(typeof(double), typeof(Visibility))]
    public class GreaterThanZeroToVisibilityConverter : IValueConverter
    {
        public Visibility TrueValue { get; set; } = Visibility.Visible;
        public Visibility FalseValue { get; set; } = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return intValue > 0 ? TrueValue : FalseValue;
            }

            if (value is double doubleValue)
            {
                return doubleValue > 0.0d ? TrueValue : FalseValue;
            }

            throw new ArgumentException($"Unable to convert {nameof(value)}!");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}