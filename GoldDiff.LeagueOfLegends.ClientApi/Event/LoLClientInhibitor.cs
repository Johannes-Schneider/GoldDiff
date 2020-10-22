using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientInhibitor
    {
        public LoLTeamType Team { get; set; }
        
        public LoLClientInhibitorTier Tier { get; set; }
    }
}