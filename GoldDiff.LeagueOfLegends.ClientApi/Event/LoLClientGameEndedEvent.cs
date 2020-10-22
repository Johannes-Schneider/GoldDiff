using GoldDiff.LeagueOfLegends.ClientApi.Converter;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientGameEndedEvent : LoLClientEvent
    {
        [JsonProperty("Result")]
        [JsonConverter(typeof(LoLClientGameResultConverter))]
        public LoLClientGameResult Result { get; set; }
    }
}