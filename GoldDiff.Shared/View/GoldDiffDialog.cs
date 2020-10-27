using System.Windows;

namespace GoldDiff.Shared.View
{
    public class GoldDiffDialog : Window
    {
        public GoldDiffDialog()
        {
            Style = Application.Current?.Resources[GoldDiffSharedResourceKeys.DefaultDialogStyle] as Style;
            WindowState = WindowState.Minimized;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Owner == null)
            {
                return;
            }

            Width = Owner.Width;
            Height = Owner.Height;
            Top = Owner.Top;
            Left = Owner.Left;
            WindowState = WindowState.Normal;
        }
    }
}