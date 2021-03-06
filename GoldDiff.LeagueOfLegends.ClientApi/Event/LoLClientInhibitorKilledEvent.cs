﻿using GoldDiff.LeagueOfLegends.ClientApi.Converter;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientInhibitorKilledEvent : LoLClientKilledWithAssistersEvent
    {
        [JsonProperty("InhibKilled")]
        [JsonConverter(typeof(LoLClientInhibitorConverter))]
        public LoLClientInhibitor Inhibitor { get; set; }
    }
}