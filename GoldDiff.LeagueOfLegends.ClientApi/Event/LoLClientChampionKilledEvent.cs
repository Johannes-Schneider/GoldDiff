using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientChampionKilledEvent : LoLClientKilledWithAssistersEvent
    {
        [JsonProperty("VictimName")]
        public string VictimSummonerName { get; set; }
    }
}