using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public class LoLClientTurret
    {
        public LoLTeamType Team { get; set; }
        
        public LoLClientTurretTier Tier { get; set; }
    }
}