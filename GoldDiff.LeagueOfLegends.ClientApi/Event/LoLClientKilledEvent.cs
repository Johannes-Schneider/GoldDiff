using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public abstract class LoLClientKilledEvent : LoLClientEvent
    {
        /// <summary>
        /// Get or sets the name of the killer (could be a summoner's name, a minion's name, or a neutral objective's name).
        /// </summary>
        [JsonProperty("KillerName")]
        public string KillerName { get; set; }
    }
}