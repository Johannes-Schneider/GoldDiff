using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using GoldDiff.LeagueOfLegends.ClientApi;
using GoldDiff.LeagueOfLegends.Game;
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
        private const string TargetProcessName = "League of Legends";

        // private const string TargetProcessName = "notepad";
        private static TimeSpan ClientDataPollInterval { get; } = TimeSpan.FromMilliseconds(500);

        private ApplicationSettings ApplicationSettings { get; } = ApplicationSettings.Load();
        private MainWindow MyMainWindow { get; set; } = null!;
        private LoLStaticResourceCache LoLResourceCache { get; } = LoLStaticResourceCache.Load();
        private ProcessEventWatcher ProcessEventWatcher { get; } = new ProcessEventWatcher();

        private Process? _targetProcess;
        private LoLClientDataPollService? _clientDataPollService;
        private LoLGame? _game;
        private GoldDifferenceWindow? _goldDifferenceWindow;

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

            var processes = Process.GetProcessesByName(TargetProcessName);
            if (processes.Length > 0)
            {
                _targetProcess = processes[0];
                TargetProcessStarted();
            }
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
            _clientDataPollService?.Dispose();
            _clientDataPollService = new LoLClientDataPollService(ClientDataPollInterval);
            _clientDataPollService.GameDataReceived += ClientDataPollService_OnGameDataReceived;

            _game = new LoLGame(LoLResourceCache);
            _game.Initialized += Game_OnInitialized;
            _clientDataPollService.Start();
        }

        private void ClientDataPollService_OnGameDataReceived(object sender, LoLClientGameData e)
        {
            _game?.Consume(e);
        }

        private void Game_OnInitialized(object sender, EventArgs e)
        {
            Current.Dispatcher.Invoke(() =>
                                      {
                                          _goldDifferenceWindow?.Close();

                                          _goldDifferenceWindow = new GoldDifferenceWindow(_game);
                                          _goldDifferenceWindow.Show();
                                      });
        }

        private void TargetProcessStopped()
        {
            _clientDataPollService?.Dispose();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ProcessEventWatcher.Dispose();
            _clientDataPollService?.Dispose();
            base.OnExit(e);
        }
    }
}