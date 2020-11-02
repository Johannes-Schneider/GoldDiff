using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private const int LatestImplementationVersion = 2;
        private static string RootDirectory { get; } = Path.Combine(Environment.CurrentDirectory, "LeagueResources");
        private static string StorageLocation { get; } = Path.Combine(RootDirectory, "LeagueResourceCache.json");

        public static LoLStaticResourceCache Load()
        {
            try
            {
                return JsonConvert.DeserializeObject<LoLStaticResourceCache>(File.ReadAllText(StorageLocation));
            }
            catch (Exception exception)
            {
                return new LoLStaticResourceCache
                       {
                           CurrentVersion = LoLVersion.Zero,
                       };
            }
        }

        [JsonProperty]
        public LoLVersion CurrentVersion { get; private set; }
        
        [JsonProperty]
        private ConcurrentDictionary<string, int> ChampionNameToIdIndex { get; } = new ConcurrentDictionary<string, int>();

        [JsonProperty]
        private ConcurrentDictionary<int, LoLStaticChampion> Champions { get; } = new ConcurrentDictionary<int, LoLStaticChampion>();

        [JsonProperty]
        private ConcurrentDictionary<int, LoLStaticItem> Items { get; } = new ConcurrentDictionary<int, LoLStaticItem>();

        [JsonIgnore]
        public IEnumerable<int> ChampionIds => Champions.Keys;

        [JsonIgnore]
        public IEnumerable<int> ItemIds => Items.Keys;

        [JsonProperty]
        private int ImplementationVersion { get; set; }

        public async Task UpdateAsync(ProgressViewController? progressViewController)
        {
            if (progressViewController == null)
            {
                throw new ArgumentNullException(nameof(progressViewController));
            }

            progressViewController.Model.Title = LoLStaticResourceCacheResources.UpdateProgressTitle;
            progressViewController.Model.TotalNumberOfSteps = 3;

            progressViewController.StartNextStep(LoLStaticResourceCacheResources.CheckForUpdatesProgressStepDescription);
            var latestGameVersion = await LoLRemoteEndpoint.Get.GetLatestVersionAsync();
            if (CurrentVersion >= latestGameVersion && ImplementationVersion >= LatestImplementationVersion)
            {
                progressViewController.Done();
                return;
            }
            
            if (CurrentVersion < latestGameVersion)
            {
                progressViewController.Model.TotalNumberOfSteps += 1;
                await DeleteOldStaticResourcesAsync(progressViewController, CurrentVersion);
            }

            var resourceRootDirectory = StaticResourceRootDirectory(latestGameVersion);
            if (!Directory.Exists(StaticResourceRootDirectory(latestGameVersion)))
            {
                progressViewController.Model.TotalNumberOfSteps += 3;
                var resourceArchiveFile = await DownloadStaticResourcesAsync(progressViewController, latestGameVersion).ConfigureAwait(false);
                await ExtractStaticResourcesAsync(progressViewController, latestGameVersion, resourceArchiveFile).ConfigureAwait(false);
                
                progressViewController.StartNextStep(LoLStaticResourceCacheResources.DeleteResourceArchiveProgressStepDescription);
                await Task.Run(() => File.Delete(resourceArchiveFile)).ConfigureAwait(false);
            }

            await Task.Run(() => CreateChampionIndex(progressViewController, latestGameVersion, resourceRootDirectory)).ConfigureAwait(false);
            await Task.Run(() => CreateItemIndex(progressViewController, latestGameVersion, resourceRootDirectory)).ConfigureAwait(false);

            progressViewController.Done();
            CurrentVersion = latestGameVersion;
            ImplementationVersion = LatestImplementationVersion;

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

        private async Task DeleteOldStaticResourcesAsync(ProgressViewController progressViewController, LoLVersion gameVersion)
        {
            progressViewController.StartNextStep(LoLStaticResourceCacheResources.DeleteOldStatisResourcesProgressStepDescription);

            var oldResourceRoot = Path.Combine(RootDirectory, gameVersion.ToString());
            if (Directory.Exists(oldResourceRoot))
            {
                await Task.Run(() => Directory.Delete(oldResourceRoot, true)).ConfigureAwait(false);
            }

            progressViewController.Model.CurrentStepProgress = 0.5d;
            if (File.Exists(Path.Combine(RootDirectory, $"{gameVersion}.tgz")))
            {
                await Task.Run(() => File.Delete(Path.Combine(RootDirectory, $"{gameVersion}.tgz"))).ConfigureAwait(false);
            }
            else if (File.Exists(Path.Combine(RootDirectory, $"{gameVersion}.zip")))
            {
                await Task.Run(() => File.Delete(Path.Combine(RootDirectory, $"{gameVersion}.zip"))).ConfigureAwait(false);
            }

            progressViewController.Model.CurrentStepProgress = 1.0d;
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

        private async Task ExtractStaticResourcesAsync(ProgressViewController progressViewController, LoLVersion gameVersion, string staticResourceArchiveFile)
        {
            progressViewController.StartNextStep(LoLStaticResourceCacheResources.ExtractStaticResourcesProgressStepDescription);

            var targetDirectory = StaticResourceRootDirectory(gameVersion);

            var archiveFileExtension = Path.GetExtension(staticResourceArchiveFile);
            if (archiveFileExtension.Equals(".zip", StringComparison.InvariantCultureIgnoreCase))
            {
                await Task.Run(() => ZipArchive.ExtractToDirectory(new FileInfo(staticResourceArchiveFile), new DirectoryInfo(targetDirectory), progressViewController.Model)).ConfigureAwait(false);
            }
            else
            {
                await Task.Run(() => TarGZipArchive.ExtractToDirectory(new FileInfo(staticResourceArchiveFile), new DirectoryInfo(targetDirectory), progressViewController.Model)).ConfigureAwait(false);
            }
        }

        private string StaticResourceRootDirectory(LoLVersion gameVersion)
        {
            return Path.Combine(RootDirectory, gameVersion.ToString());
        }

        public LoLStaticChampion GetChampion(string? name)
        {
            return GetChampion(ChampionNameToIdIndex[name ?? throw new ArgumentNullException(nameof(name))]);
        }

        public LoLStaticChampion GetChampion(int id)
        {
            return Champions[id];
        }

        public LoLStaticItem GetItem(int id)
        {
            return Items[id];
        }
    }
}