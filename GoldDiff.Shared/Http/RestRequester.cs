using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoldDiff.Shared.Http
{
    public class RestRequester : IDisposable
    {
        public static TimeSpan DefaultRequestTimeout { get; } = TimeSpan.FromMilliseconds(500);
        
        private HttpClient Client { get; }

        public RestRequester(TimeSpan requestTimeout)
        {
            Client = new HttpClient
                     {
                         Timeout = requestTimeout,
                     };
        }

        public RestRequester(HttpClientHandler clientHandler, TimeSpan requestTimeout)
        {
            Client = new HttpClient(clientHandler ?? throw new ArgumentNullException(nameof(clientHandler)))
                     {
                         Timeout = requestTimeout,
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