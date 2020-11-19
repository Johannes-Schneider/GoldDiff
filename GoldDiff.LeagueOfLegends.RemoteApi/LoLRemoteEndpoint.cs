using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldDiff.Shared.Http;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.Utility;

namespace GoldDiff.LeagueOfLegends.RemoteApi
{
    public sealed class LoLRemoteEndpoint : IDisposable
    {
        private static TimeSpan RequestTimeout { get; } = TimeSpan.FromSeconds(2);
        private const string VersionsUrl = "https://ddragon.leagueoflegends.com/api/versions.json";

        private static LoLRemoteEndpoint? _instance;

        public static LoLRemoteEndpoint Get
        {
            get => _instance ??= new LoLRemoteEndpoint();
        }

        private RestRequester Requester { get; }

        private LoLRemoteEndpoint()
        {
            Requester = new RestRequester(RequestTimeout);
        }

        public async Task<List<StringVersion>?> GetVersionsAsync()
        {
            var versionsAsString = await Requester.GetAsync<List<string>?>(VersionsUrl);
            if (versionsAsString == null)
            {
                return null;
            }

            var result = new List<StringVersion>();
            foreach (var versionAsString in versionsAsString)
            {
                if (StringVersion.TryParse(versionAsString, out var version))
                {
                    result.Add(version!);
                }
            }

            return result;
        }

        public async Task<StringVersion> GetLatestVersionAsync()
        {
            var availableVersions = await GetVersionsAsync();
            if (availableVersions == null)
            {
                throw new Exception($"Unable to determine the latest {nameof(StringVersion)}!");
            }

            return availableVersions.First();
        }

        public string GetStaticResourceUrl(StringVersion? version)
        {
            var versionAsString = version?.ToString();
            if (string.IsNullOrEmpty(versionAsString))
            {
                throw new ArgumentNullException(nameof(version));
            }

            var staticResourceFileExtension = ".tgz";
            if (versionAsString!.StartsWith("10.10", StringComparison.InvariantCultureIgnoreCase))
            {
                staticResourceFileExtension = ".zip";
            }

            return $"https://ddragon.leagueoflegends.com/cdn/dragontail-{versionAsString}{staticResourceFileExtension}";
        }

    #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~LoLRemoteEndpoint()
        {
            Dispose(false);
        }

        private bool _isDisposed;

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            _instance = null;
            _isDisposed = true;
            if (disposing)
            {
                Requester.Dispose();
            }
        }

    #endregion
    }
}