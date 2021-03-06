﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using FlatXaml.View;
using GoldDiff.GitHub.RemoteApi;
using GoldDiff.LeagueOfLegends.ClientApi;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.LeagueOfLegends.StaticResource;
using GoldDiff.OperatingSystem;
using GoldDiff.Shared;
using GoldDiff.Shared.Utility;
using GoldDiff.Shared.View.SharedTheme;
using GoldDiff.View;
using GoldDiff.View.Dialog;
using GoldDiff.View.Settings;
using log4net;
using log4net.Config;

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

        private ILog Log { get; set; } = null!;
        private Process? _targetProcess;
        private LoLClientDataPollService? _clientDataPollService;
        private LoLGame? _game;
        private GoldDifferenceWindow? _goldDifferenceWindow;

        private async void App_OnStartup(object sender, StartupEventArgs e)
        {
            InitializeLogger();
            InitializeUserInterface();
            if (await UpdateApplication())
            {
                MainWindow?.Close();
                Shutdown();
            }

            await UpdateResourceCacheAsync();
            StartWaitingForTargetProcess();
        }
        
        private void InitializeLogger()
        {
            var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepo, new FileInfo("log4net.config"));

            Log = LogManager.GetLogger(typeof(App));
        }

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

        private async Task<bool> UpdateApplication()
        {
            var latestRelease = await GitHubRemoteEndpoint.Instance.GetLatestReleaseAsync(ApplicationConstants.RepositoryName);

            if (latestRelease == null)
            {
                return false;
            }

            if (!StringVersion.TryParse(latestRelease.Version, out var latestReleaseVersion))
            {
                return false;
            }

            Log.Info($"Checking for application updates... Latest release version {latestRelease.Version} vs. current version {ApplicationConstants.Version}.");
            if (latestReleaseVersion <= ApplicationConstants.Version)
            {
                return false;
            }

            if (!TryGetReleaseDownloadUrl(latestRelease, out var releaseDownloadUrl))
            {
                return false;
            }

            var dialog = new UpdateApplicationDialog(latestRelease)
                         {
                             Owner = MainWindow,
                         };

            if (dialog.ShowDialog() != true)
            {
                Log.Info($"User cancelled application update.");
                return false;
            }

            Log.Info($"User confirmed application update.");
            var temporaryPath = Environment.CurrentDirectory;
            var latestReleaseDownloadFile = Path.Combine(temporaryPath, Path.GetTempFileName());
            var unpackedDirectory = Path.Combine(temporaryPath, $"GoldDiff {latestRelease.Version}");

            var command = new StringBuilder().Append("/C \"")
                                             .Append($"cd \"{temporaryPath}\" && ")
                                             .Append($"curl -s -L \"{releaseDownloadUrl}\" > \"{latestReleaseDownloadFile}\" && ")
                                             .Append($"tar -xf \"{latestReleaseDownloadFile}\" > NUL && ")
                                             .Append($"xcopy \"{unpackedDirectory}\\*.*\" \"{Environment.CurrentDirectory}\" /y > NUL && ")
                                             .Append($"del \"{latestReleaseDownloadFile}\" > NUL && ")
                                             .Append($"rmdir /q /s \"{unpackedDirectory}\" > NUL && ")
                                             .Append($"cd /d \"{Environment.CurrentDirectory}\" && ")
                                             .Append("start GoldDiff.exe\"");

            try
            {
                Process.Start(new ProcessStartInfo
                              {
                                  FileName = "cmd.exe",
                                  Arguments = command.ToString(),
                              });
                return true;
            }
            catch
            {
                // TODO: implement error handling
            }

            return false;
        }

        private bool TryGetReleaseDownloadUrl(GitHubReleaseInfo latestRelease, out string url)
        {
            url = string.Empty;
            foreach (var asset in latestRelease.Assets)
            {
                if (!asset.ContentType.Equals("application/x-zip-compressed", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if (!asset.Name.Equals($"GoldDiff.{latestRelease.Version}.zip", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                url = asset.DownloadUrl;
                return true;
            }

            return false;
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
            Log.Info($"Target process ({TargetProcessName}) detected.");
            
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
            Current.Dispatcher.Invoke(OpenGoldDifferenceWindow);
        }

        private void OpenGoldDifferenceWindow()
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
        }

        private void GoldDifferenceWindow_OnClosed(object? sender, EventArgs e)
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