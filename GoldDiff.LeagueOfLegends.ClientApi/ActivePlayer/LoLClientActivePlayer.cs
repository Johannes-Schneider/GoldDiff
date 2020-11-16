using System.Text.Json.Serialization;

namespace GoldDiff.LeagueOfLegends.ClientApi.ActivePlayer
{
    public class LoLClientActivePlayer
    {
        [JsonPropertyName("summonerName")]
        public string SummonerName { get; set; } = string.Empty;
    }
}