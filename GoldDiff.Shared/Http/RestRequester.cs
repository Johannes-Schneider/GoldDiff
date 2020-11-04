using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoldDiff.Shared.Http
{
    public class RestRequester : IDisposable
    {
        private HttpClient Client { get; }

        public RestRequester()
        {
            Client = new HttpClient
                     {
                         Timeout = TimeSpan.FromMilliseconds(500),
                     };
        }

        public RestRequester(HttpClientHandler clientHandler)
        {
            Client = new HttpClient(clientHandler ?? throw new ArgumentNullException(nameof(clientHandler)))
                     {
                         Timeout = TimeSpan.FromMilliseconds(500),
                     };
        }

        public async Task<TResultType> GetAsync<TResultType>(string url)
        {
            try
            {
                var response = await Client.GetAsync(url).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    return default!;
                }

                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<TResultType>(json);
            }
            catch (TaskCanceledException)
            {
                return default!;
            }
        }

    #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RestRequester()
        {
            Dispose(false);
        }

        protected bool _isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            if (disposing)
            {
                Client.Dispose();
            }
        }

    #endregion
    }
}