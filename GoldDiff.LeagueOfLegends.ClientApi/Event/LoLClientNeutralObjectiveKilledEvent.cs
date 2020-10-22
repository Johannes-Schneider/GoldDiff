using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public abstract class LoLClientNeutralObjectiveKilledEvent : LoLClientKilledWithAssistersEvent
    {
        [JsonProperty("Stolen")]
        public bool HasBeenStolen { get; set; }
    }
}