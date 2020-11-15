using System.Windows;
using System.Windows.Controls;
using FlatXaml.View;
using GoldDiff.Shared.View.SharedTheme;

namespace GoldDiff.View.Settings
{
    public partial class ViewSettingsDialog : FlatDialog
    {
        private static readonly DependencyProperty ThemeHasChangedProperty = DependencyProperty.Register(nameof(ThemeHasBeenChanged), typeof(bool), typeof(ViewSettingsDialog));

        private bool ThemeHasBeenChanged
        {
            get => (bool) GetValue(ThemeHasChangedProperty);
            set => SetValue(ThemeHasChangedProperty, value);
        }

        private ThemeType InitialThemeValue { get; }

        public ViewSettingsDialog()
        {
            InitialThemeValue = ThemeSettings.Instance.Theme;

            InitializeComponent();
        }

        private void SelectTheme_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ThemeHasBeenChanged = ThemeSettings.Instance.Theme != InitialThemeValue;
        }
    }
}