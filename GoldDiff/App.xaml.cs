using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using FlatXaml.View;
using GoldDiff.LeagueOfLegends.ClientApi;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.LeagueOfLegends.StaticResource;
using GoldDiff.OperatingSystem;
using GoldDiff.Shared.View.SharedTheme;
using GoldDiff.View;
using GoldDiff.View.Settings;

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
        private MainWindow MyMainWindow { get; set; } = null!;
        private LoLStaticResourceCache LoLResourceCache { get; } = LoLStaticResourceCache.Load();
        private ProcessEventWatcher ProcessEventWatcher { get; } = new();

        private Process? _targetProcess;
        private LoLClientDataPollService? _clientDataPollService;
        private LoLGame? _game;
        private GoldDifferenceWindow? _goldDifferenceWindow;

        private async void App_OnStartup(object sender, StartupEventArgs e)
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;

            InitializeUserInterface();
            await UpdateResourceCacheAsync();
            StartWaitingForTargetProcess();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) { }

        private void InitializeUserInterface()
        {
            foreach (var themePart in ThemeSettings.Instance.LoadTheme())
            {
                Current.Resources.MergedDictionaries.Add(themePart);
            }

            MyMainWindow = new MainWindow();
            MyMainWindow.Model.LeagueVersion = LoLResourceCache.CurrentVersion;

            MainWindow = MyMainWindow;
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            MainWindow.Show();
        }

        private async Task UpdateResourceCacheAsync()
        {
            var progressView = new FlatProgression();
            MyMainWindow.Model.Content = progressView;

            await LoLResourceCache.UpdateAsync(progressView.Progression);

            MyMainWindow.Model.Content = null;
            MyMainWindow.Model.LeagueVersion = LoLResourceCache.CurrentVersion;
        }

        private void StartWaitingForTargetProcess()
        {
            ProcessEventWatcher.ProcessStarted += ProcessEventWatcher_OnProcessStarted;
            ProcessEventWatcher.ProcessStopped += ProcessEventWatcher_OnProcessStopped;

            var processes = Process.GetProcessesByName(TargetProcessName);
            if (processes.Length > 0)
            {
                _targetProcess = processes[0];
                TargetProcessStarted();
            }
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
                                          if (_goldDifferenceWindow != null)
                                          {
                                              _goldDifferenceWindow.Model.Game = _game;
                                              if (_goldDifferenceWindow.WindowState == WindowState.Minimized)
                                              {
                                                  _goldDifferenceWindow.WindowState = WindowState.Normal;
                                              }
                                          }
                                          else
                                          {
                                              _goldDifferenceWindow = new GoldDifferenceWindow(_game);
                                              _goldDifferenceWindow.Closed += GoldDifferenceWindow_OnClosed;
                                              _goldDifferenceWindow.Show();
                                          }
                                      });
        }

        private void GoldDifferenceWindow_OnClosed(object sender, EventArgs e)
        {
            _goldDifferenceWindow = null;
        }

        private void TargetProcessStopped()
        {
            _game?.GameClientClosed();
            _clientDataPollService?.Dispose();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ProcessEventWatcher.Dispose();
            _clientDataPollService?.Dispose();
            ViewSettings.Instance.Save();
            ThemeSettings.Instance.Save();
            base.OnExit(e);
        }
    }
}