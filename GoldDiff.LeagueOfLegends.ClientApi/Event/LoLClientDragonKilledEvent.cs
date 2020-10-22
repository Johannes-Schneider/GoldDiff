using GoldDiff.LeagueOfLegends.ClientApi.Converter;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientDragonKilledEvent : LoLClientNeutralObjectiveKilledEvent
    {
        [JsonProperty("DragonType")]
        [JsonConverter(typeof(LoLClientDragonTypeConverter))]
        public LoLClientDragonType DragonType { get; set; }
    }
}