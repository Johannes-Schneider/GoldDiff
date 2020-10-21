using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi
{
    public class LoLClientEndpoint
    {
        private const string ClientCertificateThumbprint = "8259aafd8f71a809d2b154dd1cdb492981e448bd";
        private const string Host = "https://127.0.0.1:2999";

        private static string GameDataUrl { get; } = Host + "/liveclientdata";

        private static LoLClientEndpoint? _instance;

        public static LoLClientEndpoint Get
        {
            get { return _instance ??= new LoLClientEndpoint(); }
        }

        private HttpClient Client { get; }

        private LoLClientEndpoint()
        {
            Client = new HttpClient(new HttpClientHandler
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
            using var response = await Client.GetAsync(GameDataUrl).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<LoLClientGameData>(json);
        }
    }
}