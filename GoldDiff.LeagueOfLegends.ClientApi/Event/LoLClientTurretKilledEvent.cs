using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientTurretKilledEvent : LoLClientKilledWithAssistersEvent
    {
        [JsonProperty("TurretKilled")]
        public string TurretName { get; set; }
    }
}