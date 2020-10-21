using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Player
{
    public class LoLClientItem
    {
        [JsonProperty("itemID")]
        public int Id { get; set; }
        
        [JsonProperty("displayName")]
        public string Name { get; set; }
        
        [JsonProperty("count")]
        public int Amount { get; set; }
        
        [JsonProperty("canUse")]
        public bool IsUsable { get; set; }
        
        [JsonProperty("consumable")]
        public bool IsConsumable { get; set; }
    }
}