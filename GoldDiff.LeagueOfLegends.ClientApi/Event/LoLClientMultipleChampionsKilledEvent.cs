using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientMultipleChampionsKilledEvent : LoLClientKilledEvent
    {
        [JsonProperty("KillStreak")]
        public int NumberOfKills { get; set; }
    }
}