using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GoldDiff.View.ControlElement
{
    public class IconControl : Control
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(Geometry), MethodBase.GetCurrentMethod().DeclaringType);

        public Geometry? Source
        {
            get => GetValue(SourceProperty) as Geometry;
            set => SetValue(SourceProperty, value);
        }
    }
}