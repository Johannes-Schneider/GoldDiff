using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoldDiff.LeagueOfLegends.Api;
using GoldDiff.Properties;
using GoldDiff.Shared.Archive;
using GoldDiff.Shared.Http;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.View.Controller;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.StaticResource
{
    public sealed partial class LoLStaticResourceCache
    {
        private static string RootDirectory { get; } = Path.Combine(Environment.CurrentDirectory, nameof(LoLStaticResourceCache));
        private static string StorageLocation { get; } = Path.Combine(RootDirectory, $"{nameof(LoLStaticResourceCache)}.json");

        public static LoLStaticResourceCache Load()
        {
            try
            {
                return JsonConvert.DeserializeObject<LoLStaticResourceCache>(File.ReadAllText(StorageLocation));
            }
            catch
            {
                return new LoLStaticResourceCache();
            }
        }

        [JsonProperty]
        public LoLVersion CurrentVersion { get; private set; } = LoLVersion.Zero;
        
        [JsonProperty]
        private ConcurrentDictionary<int, string> ChampionIdToNameIndex { get; } = new ConcurrentDictionary<int, string>();
        
        [JsonProperty]
        private ConcurrentDictionary<string, LoLStaticChampion> Champions { get; } = new ConcurrentDictionary<string, LoLStaticChampion>();
        
        [JsonProperty]
        private ConcurrentDictionary<int, string> ItemIdToNameIndex { get; } = new ConcurrentDictionary<int, string>();
        
        [JsonProperty]
        private ConcurrentDictionary<string, LoLStaticItem> Items { get; } = new ConcurrentDictionary<string, LoLStaticItem>();

        public async Task UpdateAsync(ProgressViewController? progressViewController)
        {
            if (progressViewController == null)
            {
                throw new ArgumentNullException(nameof(progressViewController));
            }

            progressViewController.Model.Title = LoLStaticResourceCacheResources.UpdateProgressTitle;
            progressViewController.Model.TotalNumberOfSteps = 5;

            progressViewController.StartNextStep(LoLStaticResourceCacheResources.CheckForUpdatesProgressStepDescription);
            var latestGameVersion = await LoLRemoteEndpoint.Get.GetLatestVersionAsync();
            if (CurrentVersion >= latestGameVersion)
            {
                progressViewController.Done();
                return;
            }

            var resourceArchiveFile = await DownloadStaticResourcesAsync(progressViewController, latestGameVersion).ConfigureAwait(false);
            var resourceRootDirectory = await ExtractStaticResourcesAsync(progressViewController, latestGameVersion, resourceArchiveFile).ConfigureAwait(false);

            await Task.Run(() => CreateChampionIndex(progressViewController, latestGameVersion, resourceRootDirectory)).ConfigureAwait(false);
            await Task.Run(() => CreateItemIndex(progressViewController, latestGameVersion, resourceRootDirectory)).ConfigureAwait(false);

            progressViewController.Done();
            CurrentVersion = latestGameVersion;

            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(StorageLocation)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(StorageLocation)!);
                }

                File.WriteAllText(StorageLocation, JsonConvert.SerializeObject(this, Formatting.Indented));
            }
            catch (Exception exception)
            {
                // TODO: implement error handling
            }
        }

        private async Task<string> DownloadStaticResourcesAsync(ProgressViewController progressViewController, LoLVersion gameVersion)
        {
            progressViewController.StartNextStep(LoLStaticResourceCacheResources.DownloadStaticResourcesProgressStepDescription);

            var downloadUrl = LoLRemoteEndpoint.Get.GetStaticResourceUrl(gameVersion);
            var fileExtension = downloadUrl.Split(new[] {"."}, StringSplitOptions.None).Last();
            var targetFile = Path.Combine(RootDirectory, $"{gameVersion}.{fileExtension}");

            if (File.Exists(targetFile))
            {
                return targetFile;
            }

            var fileDownloader = new FileDownloader();
            fileDownloader.DownloadProgressChanged += (_, args) =>
                                                      {
                                                          progressViewController.Model.CurrentStepProgress = args.Progress;
                                                          progressViewController.Model.CurrentStepDescription = $"{LoLStaticResourceCacheResources.DownloadStaticResourcesProgressStepDescription} " +
                                                                                                                $"({args.AverageDownloadSpeedInMBs:F1} MiB/s, " +
                                                                                                                $"{LoLStaticResourceCacheResources.DownloadStaticResourcesRemainingTime}: {args.EstimatedRemainingTime.TotalSeconds:F0} s)";
                                                      };
            await fileDownloader.DownloadAsync(downloadUrl, targetFile);
            return targetFile;
        }

        private async Task<string> ExtractStaticResourcesAsync(ProgressViewController progressViewController, LoLVersion gameVersion, string staticResourceArchiveFile)
        {
            progressViewController.StartNextStep(LoLStaticResourceCacheResources.ExtractStaticResourcesProgressStepDescription);

            var targetDirectory = Path.Combine(RootDirectory, gameVersion.ToString());
            if (Directory.Exists(targetDirectory))
            {
                return targetDirectory;
            }

            var archiveFileExtension = Path.GetExtension(staticResourceArchiveFile);
            if (archiveFileExtension.Equals(".zip", StringComparison.InvariantCultureIgnoreCase))
            {
                await Task.Run(() => ZipArchive.ExtractToDirectory(new FileInfo(staticResourceArchiveFile), new DirectoryInfo(targetDirectory), progressViewController.Model)).ConfigureAwait(false);
            }
            else
            {
                await Task.Run(() => TarGZipArchive.ExtractToDirectory(new FileInfo(staticResourceArchiveFile), new DirectoryInfo(targetDirectory), progressViewController.Model)).ConfigureAwait(false);
            }

            return targetDirectory;
        }

        public LoLStaticChampion GetChampion(int id)
        {
            return GetChampion(ChampionIdToNameIndex[id]);
        }

        public LoLStaticChampion GetChampion(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return Champions[name!];
        }

        public LoLStaticItem GetItem(int id)
        {
            return GetItem(ItemIdToNameIndex[id]);
        }

        public LoLStaticItem GetItem(string? name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return Items[name!];
        }
    }
}