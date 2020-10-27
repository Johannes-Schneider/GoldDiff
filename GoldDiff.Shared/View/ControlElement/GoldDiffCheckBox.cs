using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GoldDiff.Shared.View.ControlElement
{
    public class GoldDiffCheckBox : CheckBox
    {
        public static readonly DependencyProperty NotCheckedIconProperty = DependencyProperty.Register(nameof(NotCheckedIcon), typeof(Geometry), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty CheckedIconProperty = DependencyProperty.Register(nameof(CheckedIcon), typeof(Geometry), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty NotCheckedBackgroundProperty = DependencyProperty.Register(nameof(NotCheckedBackground), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty CheckedBackgroundProperty = DependencyProperty.Register(nameof(CheckedBackground), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty NotCheckedIconForegroundProperty = DependencyProperty.Register(nameof(NotCheckedIconForeground), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty CheckedIconForegroundProperty = DependencyProperty.Register(nameof(CheckedIconForeground), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty NotCheckedIconBackgroundProperty = DependencyProperty.Register(nameof(NotCheckedIconBackground), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty CheckedIconBackgroundProperty = DependencyProperty.Register(nameof(CheckedIconBackground), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType);

        public Geometry? NotCheckedIcon
        {
            get => GetValue(NotCheckedIconProperty) as Geometry;
            set => SetValue(NotCheckedIconProperty, value);
        }
        
        public Geometry? CheckedIcon
        {
            get => GetValue(CheckedIconProperty) as Geometry;
            set => SetValue(CheckedIconProperty, value);
        }

        public Brush? NotCheckedBackground
        {
            get => GetValue(NotCheckedBackgroundProperty) as Brush;
            set => SetValue(NotCheckedBackgroundProperty, value);
        }
        
        public Brush? CheckedBackground
        {
            get => GetValue(CheckedBackgroundProperty) as Brush;
            set => SetValue(CheckedBackgroundProperty, value);
        }

        public Brush? NotCheckedIconForeground
        {
            get => GetValue(NotCheckedIconForegroundProperty) as Brush;
            set => SetValue(NotCheckedIconForegroundProperty, value);
        }
        
        public Brush? CheckedIconForeground
        {
            get => GetValue(CheckedIconForegroundProperty) as Brush;
            set => SetValue(CheckedIconForegroundProperty, value);
        }

        public Brush? NotCheckedIconBackground
        {
            get => GetValue(NotCheckedIconBackgroundProperty) as Brush;
            set => SetValue(NotCheckedIconBackgroundProperty, value);
        }
        
        public Brush? CheckedIconBackground
        {
            get => GetValue(CheckedIconBackgroundProperty) as Brush;
            set => SetValue(CheckedIconBackgroundProperty, value);
        }

        public GoldDiffCheckBox()
        {
            Style = Application.Current?.Resources[GoldDiffSharedResourceKeys.DefaultGoldDiffCheckBoxStyle] as Style;
        }
    }
}