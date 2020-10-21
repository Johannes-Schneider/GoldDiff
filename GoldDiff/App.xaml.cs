using System;
using System.Threading.Tasks;
using System.Windows;
using GoldDiff.LeagueOfLegends.StaticResource;
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
        private MainWindow? _mainWindow;
        private LoLStaticResourceCache? _lolResourceCache;
        
        private async void App_OnStartup(object sender, StartupEventArgs e)
        {
            var applicationSettings = ApplicationSettings.Load();
            LoadTheme(applicationSettings);
            
            _mainWindow = new MainWindow();
            MainWindow = _mainWindow;
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            MainWindow.Show();
            
            _lolResourceCache = LoLStaticResourceCache.Load();
            await UpdateResourceCacheAsync();
        }

        private static void LoadTheme(ApplicationSettings settings)
        {
            var theme = new ResourceDictionary
                        {
                            Source = new Uri(settings.ThemeLocation),
                        };
            Current.Resources.MergedDictionaries.Add(theme);
        }

        private async Task UpdateResourceCacheAsync()
        {
            var progressView = new ProgressView();
            _mainWindow!.Model.Content = progressView;
            
            await _lolResourceCache!.UpdateAsync(progressView.Controller);

            _mainWindow!.Model.Content = null;
        }
    }
}