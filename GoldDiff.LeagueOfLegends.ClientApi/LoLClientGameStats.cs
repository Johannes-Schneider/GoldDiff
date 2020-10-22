using System;
using GoldDiff.LeagueOfLegends.ClientApi.Converter;
using GoldDiff.Shared.LeagueOfLegends;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi
{
    public class LoLClientGameStats
    {
        [JsonProperty("gameTime")]
        [JsonConverter(typeof(LoLTimeConverter))]
        public TimeSpan GameTime { get; set; }
        
        [JsonProperty("gameMode")]
        [JsonConverter(typeof(LoLGameModeConverter))]
        public LoLGameModeType GameMode { get; set; }
        
        [JsonProperty("mapNumber")]
        [JsonConverter(typeof(LoLMapConverter))]
        public LoLMapType Map { get; set; }
        
        [JsonProperty("mapTerrain")]
        [JsonConverter(typeof(LoLMapTerrainConverter))]
        public LoLMapTerrainType MapTerrain { get; set; }
    }
}