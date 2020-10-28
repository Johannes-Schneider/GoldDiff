using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GoldDiff.Shared.View.Converter
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullToVisibilityConverter : IValueConverter
    {
        public Visibility NullValue { get; set; } = Visibility.Collapsed;
        public Visibility NotNullValue { get; set; } = Visibility.Visible;
        
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, null))
            {
                return NullValue;
            }
            else
            {
                return NotNullValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}