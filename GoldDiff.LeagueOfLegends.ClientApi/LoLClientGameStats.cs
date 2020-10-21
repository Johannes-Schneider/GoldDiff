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
        public LoLGameMode GameMode { get; set; }
        
        [JsonProperty("mapNumber")]
        [JsonConverter(typeof(LoLMapConverter))]
        public LoLMap Map { get; set; }
        
        [JsonProperty("mapTerrain")]
        [JsonConverter(typeof(LoLMapTerrainConverter))]
        public LoLMapTerrain MapTerrain { get; set; }
    }
}