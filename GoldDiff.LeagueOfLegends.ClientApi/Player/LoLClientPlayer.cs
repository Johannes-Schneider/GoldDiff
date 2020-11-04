using System;
using System.Collections.Generic;
using GoldDiff.LeagueOfLegends.ClientApi.Converter;
using GoldDiff.Shared.LeagueOfLegends;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Player
{
    public class LoLClientPlayer
    {
        [JsonProperty("summonerName")]
        public string SummonerName { get; set; } = string.Empty;

        [JsonProperty("championName")]
        public string ChampionName { get; set; } = string.Empty;

        [JsonProperty("isDead")]
        public bool IsDead { get; set; }
        
        [JsonProperty("respawnTimer")]
        [JsonConverter(typeof(LoLTimeConverter))]
        public TimeSpan RespawnTimeInSeconds { get; set; } = TimeSpan.Zero;

        [JsonProperty("position")]
        [JsonConverter(typeof(LoLPositionConverter))]
        public LoLPositionType Position { get; set; } = LoLPositionType.Undefined;

        [JsonProperty("team")]
        [JsonConverter(typeof(LoLTeamConverter))]
        public LoLTeamType Team { get; set; } = LoLTeamType.Undefined;
        
        [JsonProperty("items")]
        public List<LoLClientItem> Items { get; set; } = new List<LoLClientItem>();
        
        [JsonProperty("scores")]
        public LoLClientScore Score { get; set; } = new LoLClientScore();
    }
}