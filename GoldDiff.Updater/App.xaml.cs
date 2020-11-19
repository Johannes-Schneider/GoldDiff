using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using FlatXaml.Model;
using GoldDiff.GitHub.RemoteApi;
using GoldDiff.Shared;
using GoldDiff.Shared.Http;
using GoldDiff.Shared.Utility;
using GoldDiff.Updater.Properties;

namespace GoldDiff.Updater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string DownloadDirectory { get; } = Path.Combine(Environment.CurrentDirectory, ".temp");
        private Progression Progress { get; set; }

        private async void App_OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

                Progress = new Progression
                           {
                               Headline = MainWindowResources.ProgressTitle,
                               TotalNumberOfSteps = 1,
                           };

                var mainWindow = new MainWindow
                                 {
                                     Progression = Progress,
                                 };
                mainWindow.Show();

                var downloadUrl = await GetLatestReleaseDownloadUrl().ConfigureAwait(false);
                if (string.IsNullOrEmpty(downloadUrl))
                {
                    return;
                }

                Progress.TotalNumberOfSteps += 1;
                var latestReleaseAsset = await DownloadFile(downloadUrl).ConfigureAwait(false);
            }
            finally
            {
                Shutdown();
            }
        }

        private async Task<string?> GetLatestReleaseDownloadUrl()
        {
            Progress.StartNextStep(MainWindowResources.ProgressCheckForUpdates);

            var latestRelease = await GitHubRemoteEndpoint.Instance.GetLatestReleaseAsync(ApplicationConstants.RepositoryName);
            if (latestRelease == null)
            {
                return null;
            }

            if (!StringVersion.TryParse(latestRelease.Version, out var latestReleaseVersion))
            {
                return null;
            }

            if (latestReleaseVersion <= ApplicationConstants.Version)
            {
                return null;
            }

            foreach (var asset in latestRelease.Assets)
            {
                if (!asset.ContentType.Equals("application/x-zip-compressed", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if (!asset.Name.Equals($"GoldDiff.{latestReleaseVersion}.zip", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                return asset.DownloadUrl;
            }

            return null;
        }

        private async Task<string> DownloadFile(string url)
        {
            if (!Directory.Exists(DownloadDirectory))
            {
                Directory.CreateDirectory(DownloadDirectory);
            }

            var filePath = Path.Combine(DownloadDirectory, ".download");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            Progress.StartNextStep(MainWindowResources.ProgressDownloadRelease);

            var downloader = new FileDownloader();
            downloader.DownloadProgressChanged += (_, args) =>
                                                  {
                                                      Progress.CurrentStepProgress = args.Progress;
                                                      Progress.CurrentStepDescription = $"{MainWindowResources.ProgressDownloadRelease} " +
                                                                                        $"({args.AverageDownloadSpeedInMBs:F1} MiB/s, " +
                                                                                        $"{MainWindowResources.ProgressDownloadReleaseRemainingTime}: {args.EstimatedRemainingTime.TotalSeconds:F0} s)";
                                                  };
            await downloader.DownloadAsync(url, filePath);

            return filePath;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            try
            {
                Process.Start(Path.Combine(Environment.CurrentDirectory, "GoldDiff.exe"));
            }
            catch
            {
                // ignore
            }
        }
    }
}