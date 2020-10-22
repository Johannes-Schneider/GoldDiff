using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientFirstChampionKilledEvent : LoLClientEvent
    {
        [JsonProperty("Recipient")]
        public string RecipientSummonerName { get; set; }
    }
}