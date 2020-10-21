using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GoldDiff.Shared.View.ControlElement
{
    public class IconControl : Control
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(Geometry), MethodBase.GetCurrentMethod().DeclaringType);

        public Geometry? Source
        {
            get => GetValue(SourceProperty) as Geometry;
            set => SetValue(SourceProperty, value);
        }

        public IconControl()
        {
            Style = Application.Current?.Resources[GoldDiffResourceKey.DefaultIconControlStyle] as Style ?? null;
        }
    }
}