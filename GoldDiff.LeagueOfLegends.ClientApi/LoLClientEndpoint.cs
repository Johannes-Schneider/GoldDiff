using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GoldDiff.Shared.Http;

namespace GoldDiff.LeagueOfLegends.ClientApi
{
    public sealed class LoLClientEndpoint : IDisposable
    {
        private const string ClientCertificateThumbprint = "8259aafd8f71a809d2b154dd1cdb492981e448bd";
        private const string Host = "https://127.0.0.1:2999/liveclientdata";

        private static string GameDataUrl { get; } = Host + "/allgamedata";

        private static LoLClientEndpoint? _instance;

        public static LoLClientEndpoint Get
        {
            get { return _instance ??= new LoLClientEndpoint(); }
        }

        private RestRequester Requester { get; }

        private LoLClientEndpoint()
        {
            Requester = new RestRequester(new HttpClientHandler
                                          {
                                              ServerCertificateCustomValidationCallback = ValidateServerCertificate,
                                          });
        }

        private static bool ValidateServerCertificate(HttpRequestMessage message, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
            {
                return true;
            }

            if (certificate?.Thumbprint?.Equals(ClientCertificateThumbprint, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            return false;
        }

        public async Task<LoLClientGameData?> GetGameDataAsync()
        {
            return await Requester.GetAsync<LoLClientGameData?>(GameDataUrl);
        }

    #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~LoLClientEndpoint()
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