using System;
using System.Globalization;
using System.Threading;
using System.Windows.Data;

namespace GoldDiff.View.Converter
{
    [ValueConversion(typeof(int), typeof(string))]
    public class IntToLoLGoldValueConverter : IValueConverter
    {
        public int PostDecimalPlaces { get; set; } = 1;
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int intValue))
            {
                throw new ArgumentException($"{nameof(value)} must be of type {nameof(Int32)}!");
            }

            return Convert(intValue, PostDecimalPlaces);
        }

        public static string Convert(int intValue, int postDecimalPlaces = 1)
        {
            var sign = intValue >= 0 ? 1 : -1;
            var absValue = Math.Abs(intValue);
            if (absValue < 500)
            {
                return (absValue * sign).ToString();
            }

            return (Math.Round(absValue / 1000.0d, postDecimalPlaces) * sign).ToString(Thread.CurrentThread.CurrentCulture) + "K";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}