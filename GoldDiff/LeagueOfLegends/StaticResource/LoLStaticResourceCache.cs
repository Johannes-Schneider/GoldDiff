using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FlatXaml.Model;
using GoldDiff.View.Resource;
using GoldDiff.LeagueOfLegends.RemoteApi;
using GoldDiff.Shared.Archive;
using GoldDiff.Shared.Http;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.Utility;
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
                           CurrentVersion = StringVersion.Zero,
                       };
            }
        }

        [JsonProperty]
        public StringVersion CurrentVersion { get; private set; }

        [JsonProperty]
        private ConcurrentDictionary<string, int> ChampionNameToIdIndex { get; } = new();

        [JsonProperty]
        private ConcurrentDictionary<int, LoLStaticChampion> Champions { get; } = new();

        [JsonProperty]
        private ConcurrentDictionary<int, LoLStaticItem> Items { get; } = new();

        [JsonIgnore]
        public IEnumerable<int> ChampionIds => Champions.Keys;

        [JsonIgnore]
        public IEnumerable<int> ItemIds => Items.Keys;

        [JsonProperty]
        private int ImplementationVersion { get; set; }

        public async Task UpdateAsync(Progression? progression)
        {
            if (progression == null)
            {
                throw new ArgumentNullException(nameof(progression));
            }

            progression.TotalNumberOfSteps = 3;

            progression.Headline = LoLStaticResourceCacheResources.UpdateProgressTitle;
            progression.StartNextStep(LoLStaticResourceCacheResources.CheckForUpdatesProgressStepDescription);
            var latestGameVersion = await LoLRemoteEndpoint.Get.GetLatestVersionAsync();
            if (CurrentVersion >= latestGameVersion && ImplementationVersion >= LatestImplementationVersion)
            {
                progression.Done();
                return;
            }

            if (CurrentVersion < latestGameVersion)
            {
                progression.TotalNumberOfSteps += 1;
                await DeleteOldStaticResourcesAsync(progression, CurrentVersion);
            }

            var resourceRootDirectory = StaticResourceRootDirectory(latestGameVersion);
            if (!Directory.Exists(StaticResourceRootDirectory(latestGameVersion)))
            {
                progression.TotalNumberOfSteps += 3;
                var resourceArchiveFile = await DownloadStaticResourcesAsync(progression, latestGameVersion).ConfigureAwait(false);
                await ExtractStaticResourcesAsync(progression, latestGameVersion, resourceArchiveFile).ConfigureAwait(false);

                progression.StartNextStep(LoLStaticResourceCacheResources.DeleteResourceArchiveProgressStepDescription);
                await Task.Run(() => File.Delete(resourceArchiveFile)).ConfigureAwait(false);
            }

            await Task.Run(() => CreateChampionIndex(progression, latestGameVersion, resourceRootDirectory)).ConfigureAwait(false);
            await Task.Run(() => CreateItemIndex(progression, latestGameVersion, resourceRootDirectory)).ConfigureAwait(false);

            progression.Done();
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

        private async Task DeleteOldStaticResourcesAsync(Progression progression, StringVersion gameVersion)
        {
            progression.StartNextStep(LoLStaticResourceCacheResources.DeleteOldStatisResourcesProgressStepDescription);

            var oldResourceRoot = Path.Combine(RootDirectory, gameVersion.ToString());
            if (Directory.Exists(oldResourceRoot))
            {
                await Task.Run(() => Directory.Delete(oldResourceRoot, true)).ConfigureAwait(false);
            }

            progression.CurrentStepProgress = 0.5d;
            if (File.Exists(Path.Combine(RootDirectory, $"{gameVersion}.tgz")))
            {
                await Task.Run(() => File.Delete(Path.Combine(RootDirectory, $"{gameVersion}.tgz"))).ConfigureAwait(false);
            }
            else if (File.Exists(Path.Combine(RootDirectory, $"{gameVersion}.zip")))
            {
                await Task.Run(() => File.Delete(Path.Combine(RootDirectory, $"{gameVersion}.zip"))).ConfigureAwait(false);
            }

            progression.CurrentStepProgress = 1.0d;
        }

        private async Task<string> DownloadStaticResourcesAsync(Progression progression, StringVersion gameVersion)
        {
            progression.StartNextStep(LoLStaticResourceCacheResources.DownloadStaticResourcesProgressStepDescription);

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
                                                          progression.CurrentStepProgress = args.Progress;
                                                          progression.CurrentStepDescription = $"{LoLStaticResourceCacheResources.DownloadStaticResourcesProgressStepDescription} " +
                                                                                               $"({args.AverageDownloadSpeedInMBs:F1} MiB/s, " +
                                                                                               $"{LoLStaticResourceCacheResources.DownloadStaticResourcesRemainingTime}: {args.EstimatedRemainingTime.TotalSeconds:F0} s)";
                                                      };
            await fileDownloader.DownloadAsync(downloadUrl, targetFile);
            return targetFile;
        }

        private async Task ExtractStaticResourcesAsync(Progression progression, StringVersion gameVersion, string staticResourceArchiveFile)
        {
            progression.StartNextStep(LoLStaticResourceCacheResources.ExtractStaticResourcesProgressStepDescription);

            var targetDirectory = StaticResourceRootDirectory(gameVersion);

            var archiveFileExtension = Path.GetExtension(staticResourceArchiveFile);
            if (archiveFileExtension.Equals(".zip", StringComparison.InvariantCultureIgnoreCase))
            {
                await Task.Run(() => ZipArchive.ExtractToDirectory(new FileInfo(staticResourceArchiveFile), new DirectoryInfo(targetDirectory), progression)).ConfigureAwait(false);
            }
            else
            {
                await Task.Run(() => TarGZipArchive.ExtractToDirectory(new FileInfo(staticResourceArchiveFile), new DirectoryInfo(targetDirectory), progression)).ConfigureAwait(false);
            }
        }

        private string StaticResourceRootDirectory(StringVersion gameVersion)
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