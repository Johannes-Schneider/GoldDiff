using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using GoldDiff.LeagueOfLegends.StaticResource;
using GoldDiff.OperatingSystem;
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
        private const string TargetProcessName = "League of Legends.exe";
        
        private ApplicationSettings ApplicationSettings { get; } = ApplicationSettings.Load();
        private MainWindow MyMainWindow { get; set; } = null!;
        private LoLStaticResourceCache LoLResourceCache { get; } = LoLStaticResourceCache.Load();
        private ProcessEventWatcher ProcessEventWatcher { get; } = new ProcessEventWatcher();

        private Process? _targetProcess;
        
        private async void App_OnStartup(object sender, StartupEventArgs e)
        {
            InitializeUserInterface();
            await UpdateResourceCacheAsync();
            
            ProcessEventWatcher.ProcessStarted += ProcessEventWatcher_OnProcessStarted;
            ProcessEventWatcher.ProcessStopped += ProcessEventWatcher_OnProcessStopped;
        }

        private void InitializeUserInterface()
        {
            var theme = new ResourceDictionary
                        {
                            Source = new Uri(ApplicationSettings.ThemeLocation),
                        };
            Current.Resources.MergedDictionaries.Add(theme);
            
            MyMainWindow = new MainWindow();
            MyMainWindow.Model.LeagueVersion = LoLResourceCache.CurrentVersion;
            
            MainWindow = MyMainWindow;
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            MainWindow.Show();
        }

        private async Task UpdateResourceCacheAsync()
        {
            var progressView = new ProgressView();
            MyMainWindow.Model.Content = progressView;
            
            await LoLResourceCache!.UpdateAsync(progressView.Controller);

            MyMainWindow.Model.Content = null;
            MyMainWindow.Model.LeagueVersion = LoLResourceCache.CurrentVersion;
        }
        
        private void ProcessEventWatcher_OnProcessStarted(object sender, ProcessEventEventArguments e)
        {
            if (_targetProcess != null)
            {
                return;
            }

            try
            {
                var newProcess = Process.GetProcessById(e.ProcessId);
                if (!newProcess.ProcessName.Equals(TargetProcessName))
                {
                    return;
                }

                _targetProcess = newProcess;
                TargetProcessStarted();
            }
            catch
            {
                // ignored
            }
        }

        private void ProcessEventWatcher_OnProcessStopped(object sender, ProcessEventEventArguments e)
        {
            if (_targetProcess == null || _targetProcess.Id != e.ProcessId)
            {
                return;
            }

            _targetProcess = null;
            TargetProcessStopped();
        }

        private void TargetProcessStarted()
        {
            // TODO: Start game
        }

        private void TargetProcessStopped()
        {
            // TODO: Stop game
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ProcessEventWatcher.Dispose();
            base.OnExit(e);
        }
    }
}