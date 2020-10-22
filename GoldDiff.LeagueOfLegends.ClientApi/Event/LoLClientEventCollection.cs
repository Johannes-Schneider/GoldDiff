using System.Collections.Generic;
using GoldDiff.LeagueOfLegends.ClientApi.Converter;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientEventCollection
    {
        [JsonProperty("Events", ItemConverterType = typeof(LoLClientEventConverter))]
        public List<LoLClientEvent> Events { get; set; }
    }
}