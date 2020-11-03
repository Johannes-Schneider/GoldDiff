using System;
using System.Globalization;
using System.Resources;
using System.Windows.Data;
using GoldDiff.Shared.View.Theme;

namespace GoldDiff.Shared.View.Converter
{
    public class EnumerationToStringConverter : IValueConverter
    {
        public Type EnumerationType { get; set; }
        
        public ResourceManager Localization { get; set; }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Localization.GetString(value.ToString()) ?? throw new Exception($"Unknown {EnumerationType.Name} {value}!");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string stringValue))
            {
                throw new ArgumentException($"{nameof(value)} must be of type {nameof(String)}!");
            }

            foreach (var member in Enum.GetValues(EnumerationType))
            {
                if (stringValue.Equals(Localization.GetString(member.ToString())))
                {
                    return member;
                }
            }
            
            throw new ArgumentException($"Unable to convert {stringValue} to {EnumerationType.Name}!");
        }
    }
}