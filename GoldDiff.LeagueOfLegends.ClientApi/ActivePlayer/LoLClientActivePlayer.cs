using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.ActivePlayer
{
    public class LoLClientActivePlayer
    {
        [JsonProperty("summonerName")]
        public string SummonerName { get; set; } = string.Empty;
    }
}