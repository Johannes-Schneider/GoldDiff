using System;
using System.Globalization;
using System.Windows.Data;

namespace GoldDiff.Shared.View.SharedConverter
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value is TimeSpan timeSpan ? timeSpan : null);
        }

        public static string Convert(TimeSpan? timeSpan)
        {
            return timeSpan?.ToString(@"mm\:ss") ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TryConvertBack(value as string) ?? TimeSpan.Zero;
        }

        public static TimeSpan? TryConvertBack(string? value)
        {
            var tokens = value?.Split(new[] {':'}) ?? Array.Empty<string>();
            if (tokens.Length != 2)
            {
                return null;
            }

            if (!int.TryParse(tokens[0], out var minutes) ||
                !int.TryParse(tokens[1], out var seconds))
            {
                return null;
            }

            return new TimeSpan(0, minutes, seconds);
        }
    }
}