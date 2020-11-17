using System;
using System.Globalization;
using System.Windows.Data;

namespace GoldDiff.View.Converter
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TimeSpan timeSpanValue))
            {
                return string.Empty;
            }

            var minutes = Math.Floor(timeSpanValue.TotalSeconds / 60.0d);
            var seconds = Math.Round(timeSpanValue.TotalSeconds - minutes * 60.0d, 0);

            return $"{minutes:00}:{seconds:00}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}