using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using GoldDiff.Shared.View;
using GoldDiff.Shared.View.Theme;

namespace GoldDiff.View.Settings
{
    public partial class ViewSettingsDialog : GoldDiffDialog
    {
        private static readonly DependencyProperty ThemeHasChangedProperty = DependencyProperty.Register(nameof(ThemeHasBeenChanged), typeof(bool), MethodBase.GetCurrentMethod().DeclaringType);

        private bool ThemeHasBeenChanged
        {
            get => (bool) GetValue(ThemeHasChangedProperty);
            set => SetValue(ThemeHasChangedProperty, value);
        }

        private ThemeType InitialThemeValue { get; }

        public ViewSettingsDialog()
        {
            InitialThemeValue = ViewSettings.Instance.Theme;

            InitializeComponent();
        }

        private void SelectTheme_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ThemeHasBeenChanged = ViewSettings.Instance.Theme != InitialThemeValue;
        }
    }
}