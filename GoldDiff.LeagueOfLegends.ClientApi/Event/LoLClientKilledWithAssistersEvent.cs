using System.Collections.Generic;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public abstract class LoLClientKilledWithAssistersEvent : LoLClientKilledEvent
    {
        [JsonProperty("Assisters")]
        public List<string> AssistersNames { get; set; }
    }
}