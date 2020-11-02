using GoldDiff.Shared.View.Model;
using Newtonsoft.Json;

namespace GoldDiff.View.Settings
{
    public class PlayerGoldDifferenceSettings : ViewModel
    {
        private bool _showStats = true;

        [JsonProperty]
        public bool ShowStats
        {
            get => _showStats;
            set => MutateVerbose(ref _showStats, value);
        }
    }
}