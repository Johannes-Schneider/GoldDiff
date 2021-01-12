using System;
using System.Threading;
using System.Threading.Tasks;
using GoldDiff.LeagueOfLegends.ClientApi;
using log4net;

namespace GoldDiff.LeagueOfLegends.Game
{
    public sealed class LoLClientDataPollService : IDisposable
    {
        private static ILog Log { get; } = LogManager.GetLogger(typeof(LoLClientDataPollService));
        
        public event EventHandler<LoLClientGameData>? GameDataReceived;

        public TimeSpan PollInterval { get; set; }

        private CancellationTokenSource CancellationTokenSource { get; } = new();
        private Task? _pollTask;
        private bool _isDisposed;

        public LoLClientDataPollService(TimeSpan pollInterval)
        {
            PollInterval = pollInterval;
        }

        public void Start()
        {
            if (_pollTask != null)
            {
                return;
            }

            _pollTask = Task.Run(PollGameData);
        }

        private async Task PollGameData()
        {
            while (!_isDisposed)
            {
                try
                {
                    var gameData = await LoLClientEndpoint.Get.GetGameDataAsync();
                    if (gameData != null && !_isDisposed)
                    {
                        GameDataReceived?.Invoke(this, gameData);
                    }

                    await Task.Delay(PollInterval, CancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    // ignored
                }
                catch (Exception exception)
                {
                    Log.Warn($"Exception while polling {nameof(LoLClientGameData)}.", exception);
                }
            }
        }

    #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~LoLClientDataPollService()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            if (disposing)
            {
                CancellationTokenSource.Cancel();
                _pollTask?.Wait();
            }
        }

    #endregion
    }
}