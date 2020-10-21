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
        public string SummonerName { get; set; }
        
        [JsonProperty("championName")]
        public string ChampionName { get; set; }

        [JsonProperty("isDead")]
        public bool IsDead { get; set; }
        
        [JsonProperty("respawnTimer")]
        [JsonConverter(typeof(LoLTimeConverter))]
        public TimeSpan RespawnTimeInSeconds { get; set; }
        
        [JsonProperty("position")]
        [JsonConverter(typeof(LoLPositionConverter))]
        public LoLPosition Position { get; set; }
        
        [JsonProperty("team")]
        [JsonConverter(typeof(LoLTeamConverter))]
        public LoLTeam Team { get; set; }
        
        [JsonProperty("items")]
        public List<LoLClientItem> Items { get; set; }
        
        [JsonProperty("scores")]
        public LoLClientScore Score { get; set; }
    }
}