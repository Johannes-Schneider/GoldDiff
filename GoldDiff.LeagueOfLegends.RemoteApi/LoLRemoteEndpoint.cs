using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldDiff.Shared.Http;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.LeagueOfLegends.Api
{
    public class LoLRemoteEndpoint : IDisposable
    {
        private const string VersionsUrl = "https://ddragon.leagueoflegends.com/api/versions.json";

        private static LoLRemoteEndpoint? _instance;

        public static LoLRemoteEndpoint Get
        {
            get { return _instance ??= new LoLRemoteEndpoint(); }
        }

        private RestRequester Requester { get; }

        private LoLRemoteEndpoint()
        {
            Requester = new RestRequester();
        }

        public async Task<List<LoLVersion>?> GetVersionsAsync()
        {
            var versionsAsString = await Requester.GetAsync<List<string>?>(VersionsUrl);
            if (versionsAsString == null)
            {
                return null;
            }

            var result = new List<LoLVersion>();
            foreach (var versionAsString in versionsAsString)
            {
                if (LoLVersion.TryParse(versionAsString, out var version))
                {
                    result.Add(version!);
                }
            }

            return result;
        }

        public async Task<LoLVersion> GetLatestVersionAsync()
        {
            var availableVersions = await GetVersionsAsync();
            if (availableVersions == null)
            {
                throw new Exception($"Unable to determine the latest {nameof(LoLVersion)}!");
            }

            return availableVersions.First();
        }

        public string GetStaticResourceUrl(LoLVersion? version)
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