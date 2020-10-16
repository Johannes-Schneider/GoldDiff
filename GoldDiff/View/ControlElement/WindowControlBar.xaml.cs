using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace GoldDiff.View.ControlElement
{
    public partial class WindowControlBar : UserControl
    {
        public static readonly DependencyProperty CanMinimizeProperty = DependencyProperty.Register(nameof(CanMinimize), typeof(bool), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty CanMaximizeProperty = DependencyProperty.Register(nameof(CanMaximize), typeof(bool), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty CanCloseProperty = DependencyProperty.Register(nameof(CanClose), typeof(bool), MethodBase.GetCurrentMethod().DeclaringType);
        
        public bool CanMinimize
        {
            get => (bool) GetValue(CanMinimizeProperty);
            set => SetValue(CanMinimizeProperty, value);
        }

        public bool CanMaximize
        {
            get => (bool) GetValue(CanMaximizeProperty);
            set => SetValue(CanMaximizeProperty, value);
        }

        public bool CanClose
        {
            get => (bool) GetValue(CanCloseProperty);
            set => SetValue(CanCloseProperty, value);
        }
        
        public WindowControlBar()
        {
            InitializeComponent();
        }
    }
}