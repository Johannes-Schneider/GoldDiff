using GoldDiff.LeagueOfLegends.ClientApi.Converter;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientTurretKilledEvent : LoLClientKilledWithAssistersEvent
    {
        [JsonProperty("TurretKilled")]
        [JsonConverter(typeof(LoLClientTurretConverter))]
        public LoLClientTurret Turret { get; set; }
    }
}