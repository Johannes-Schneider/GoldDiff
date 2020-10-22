using System.Collections.Generic;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using GoldDiff.LeagueOfLegends.ClientApi.Player;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi
{
    public class LoLClientGameData
    {
        [JsonProperty("allPlayers")]
        public List<LoLClientPlayer> Players { get; set; }
        
        [JsonProperty("gameData")]
        public LoLClientGameStats Stats { get; set; }
        
        [JsonProperty("events")]
        public LoLClientEventCollection EventCollection { get; set; }
    }
}