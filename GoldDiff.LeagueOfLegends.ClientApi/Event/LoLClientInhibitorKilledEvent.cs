using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientInhibitorKilledEvent : LoLClientKilledWithAssistersEvent
    {
        [JsonProperty("InhibKilled")]
        public string InhibitorName { get; set; }
    }
}