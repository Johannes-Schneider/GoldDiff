using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoldDiff.Shared.View.ControlElement
{
    public partial class WindowControlBar : UserControl
    {
        public static readonly DependencyProperty CanMinimizeProperty = DependencyProperty.Register(nameof(CanMinimize), typeof(bool), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty CanMaximizeProperty = DependencyProperty.Register(nameof(CanMaximize), typeof(bool), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty CanCloseProperty = DependencyProperty.Register(nameof(CanClose), typeof(bool), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty CanOpenSettingsProperty = DependencyProperty.Register(nameof(CanOpenSettings), typeof(bool), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty OpenSettingsCommandProperty = DependencyProperty.Register(nameof(OpenSettingsCommand), typeof(ICommand), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty OpenSettingsCommandParameterProperty = DependencyProperty.Register(nameof(OpenSettingsCommandParameter), typeof(object), MethodBase.GetCurrentMethod().DeclaringType);
        
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

        public bool CanOpenSettings
        {
            get => (bool) GetValue(CanOpenSettingsProperty);
            set => SetValue(CanOpenSettingsProperty, value);
        }

        public ICommand? OpenSettingsCommand
        {
            get => GetValue(OpenSettingsCommandProperty) as ICommand;
            set => SetValue(OpenSettingsCommandProperty, value);
        }

        public object? OpenSettingsCommandParameter
        {
            get => GetValue(OpenSettingsCommandParameterProperty);
            set => SetValue(OpenSettingsCommandParameterProperty, value);
        }
        
        public WindowControlBar()
        {
            InitializeComponent();
        }
    }
}