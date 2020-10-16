using System;
using System.Windows;
using GoldDiff.View;

namespace GoldDiff
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            LoadTheme();
            
            MainWindow = new MainWindow();
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            
            MainWindow.Show();
        }

        private static void LoadTheme()
        {
            // TODO: load theme based on user settings
            var theme = new ResourceDictionary
                        {
                            Source = new Uri("pack://application:,,,/GoldDiff;component/View/Theme/Default.xaml"),
                        };
            Current.Resources.MergedDictionaries.Add(theme);
        }
    }
}