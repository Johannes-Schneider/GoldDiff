using GoldDiff.LeagueOfLegends.ClientApi.Converter;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientInhibitorRespawningSoonEvent : LoLClientEvent
    {
        [JsonProperty("InhibRespawningSoon")]
        [JsonConverter(typeof(LoLClientInhibitorConverter))]
        public LoLClientInhibitor Inhibitor { get; set; }
    }
}