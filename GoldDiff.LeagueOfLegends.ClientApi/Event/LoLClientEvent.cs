using System;
using GoldDiff.LeagueOfLegends.ClientApi.Converter;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public abstract class LoLClientEvent
    {
        [JsonProperty("EventID")]
        public int Id { get; set; }

        [JsonProperty("EventName")]
        [JsonConverter(typeof(LoLClientEventTypeConverter))]
        public LoLClientEventType EventType { get; set; }
        
        [JsonProperty("EventTime")]
        [JsonConverter(typeof(LoLTimeConverter))]
        public TimeSpan GameTime { get; set; }
    }
}