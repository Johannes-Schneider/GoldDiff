using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Player
{
    public class LoLClientScore
    {
        [JsonProperty("kills")]
        public int Kills { get; set; }
        
        [JsonProperty("deaths")]
        public int Deaths { get; set; }
        
        [JsonProperty("assists")]
        public int Assists { get; set; }
        
        [JsonProperty("creepScore")]
        public int MinionKills { get; set; }
        
        [JsonProperty("wardScore")]
        public int Vision { get; set; }
    }
}