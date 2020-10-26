using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GoldDiff.Shared.View.Converter
{
    [ValueConversion(typeof(double), typeof(double))]
    [ValueConversion(typeof(double), typeof(CornerRadius))]
    public class MultiplyDoubleConverter : IValueConverter
    {
        public double Factor { get; set; } = 1.0d;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double doubleValue))
            {
                throw new ArgumentException($"{nameof(value)} must be of type {nameof(Double)}!");
            }

            if (targetType == typeof(double))
            {
                return doubleValue * Factor;
            }

            if (targetType == typeof(CornerRadius))
            {
                return new CornerRadius(doubleValue * Factor);
            }

            throw new ArgumentException($"Invalid {nameof(targetType)} {targetType}!");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}