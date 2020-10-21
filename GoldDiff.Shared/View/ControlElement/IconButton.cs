using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GoldDiff.Shared.View.ControlElement
{
    public class IconButton : Button
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(Geometry), MethodBase.GetCurrentMethod().DeclaringType!);

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(double), MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly DependencyProperty BackgroundWhenHoveredProperty = DependencyProperty.Register(nameof(BackgroundWhenHovered), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType);
        
        public static readonly DependencyProperty BackgroundWhenPressedProperty = DependencyProperty.Register(nameof(BackgroundWhenPressed), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType);
        
        public static readonly DependencyProperty BackgroundWhenDisabledProperty = DependencyProperty.Register(nameof(BackgroundWhenDisabled), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType);
        
        public static readonly DependencyProperty ForegroundWhenHoveredProperty = DependencyProperty.Register(nameof(ForegroundWhenHovered), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly DependencyProperty ForegroundWhenPressedProperty = DependencyProperty.Register(nameof(ForegroundWhenPressed), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType);
        
        public static readonly DependencyProperty ForegroundWhenDisabledProperty = DependencyProperty.Register(nameof(ForegroundWhenDisabled), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType);

        public Geometry? Icon
        {
            get => GetValue(IconProperty) as Geometry;
            set => SetValue(IconProperty, value);
        }

        public double IconSize
        {
            get => (double) GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public Brush? BackgroundWhenHovered
        {
            get => GetValue(BackgroundWhenHoveredProperty) as Brush;
            set => SetValue(BackgroundWhenHoveredProperty, value);
        }

        public Brush? BackgroundWhenPressed
        {
            get => GetValue(BackgroundWhenPressedProperty) as Brush;
            set => SetValue(BackgroundWhenPressedProperty, value);
        }
        
        public Brush? BackgroundWhenDisabled
        {
            get => GetValue(BackgroundWhenDisabledProperty) as Brush;
            set => SetValue(BackgroundWhenDisabledProperty, value);
        }
        
        public Brush? ForegroundWhenHovered
        {
            get => GetValue(ForegroundWhenHoveredProperty) as Brush;
            set => SetValue(ForegroundWhenHoveredProperty, value);
        }

        public Brush? ForegroundWhenPressed
        {
            get => GetValue(ForegroundWhenPressedProperty) as Brush;
            set => SetValue(ForegroundWhenPressedProperty, value);
        }

        public Brush? ForegroundWhenDisabled
        {
            get => GetValue(ForegroundWhenDisabledProperty) as Brush;
            set => SetValue(ForegroundWhenDisabledProperty, value);
        }

        public IconButton()
        {
            Style = Application.Current?.Resources[GoldDiffResourceKey.DefaultIconButtonStyle] as Style ?? null;
        }
    }
}