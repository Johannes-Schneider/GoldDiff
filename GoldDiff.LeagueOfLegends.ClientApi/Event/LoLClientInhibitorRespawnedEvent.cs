using GoldDiff.LeagueOfLegends.ClientApi.Converter;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientInhibitorRespawnedEvent : LoLClientEvent
    {
        [JsonProperty("InhibRespawned")]
        [JsonConverter(typeof(LoLClientInhibitorConverter))]
        public LoLClientInhibitor Inhibitor { get; set; }
    }
}