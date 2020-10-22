using GoldDiff.LeagueOfLegends.ClientApi.Converter;
using GoldDiff.Shared.LeagueOfLegends;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientEntireTeamKilledEvent : LoLClientEvent
    {
        [JsonProperty("Acer")]
        public string KillerSummonerName { get; set; }
        
        [JsonProperty("AcingTeam")]
        [JsonConverter(typeof(LoLTeamConverter))]
        public LoLTeamType KillerSummonerTeam { get; set; }
    }
}