using System;
using System.Windows;
using GoldDiff.Shared;
using GoldDiff.Shared.View.ControlElement;
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
            var applicationSettings = ApplicationSettings.Load();
            LoadTheme(applicationSettings);
            
            var mainWindow = new MainWindow();

            var progressView = new ProgressView();
            progressView.Model.Title = "Updating ...";
            mainWindow.Model.Content = progressView;

            MainWindow = mainWindow;
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            MainWindow.Show();
        }

        private static void LoadTheme(ApplicationSettings settings)
        {
            var theme = new ResourceDictionary
                        {
                            Source = new Uri(settings.ThemeLocation),
                        };
            Current.Resources.MergedDictionaries.Add(theme);
        }
    }
}