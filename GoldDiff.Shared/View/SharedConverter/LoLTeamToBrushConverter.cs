using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.View.SharedTheme;

namespace GoldDiff.Shared.View.SharedConverter
{
    [ValueConversion(typeof(LoLTeamType), typeof(Brush))]
    public class LoLTeamToBrushConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
                   {
                       LoLTeamType.BlueSide => Application.Current?.Resources[GoldDiffSharedThemeKeys.LoLTeamBlueSideRegular] as Brush,
                       LoLTeamType.RedSide => Application.Current?.Resources[GoldDiffSharedThemeKeys.LoLTeamRedSideRegular] as Brush,
                       _ => null,
                   };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}