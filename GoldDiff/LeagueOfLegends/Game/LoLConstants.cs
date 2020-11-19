using System;

namespace GoldDiff.LeagueOfLegends.Game
{
    public static class LoLConstants
    {
        public const int MinorPlayerGoldDifference = 500;
        public const int MediocrePlayerGoldDifference = 1000;
        public const int LargePlayerGoldDifference = 2000;

        public const int MinorTeamGoldDifference = 2000;
        public const int MediocreTeamGoldDifference = 4000;
        public const int LargeTeamGoldDifference = 6000;

        public const int MinorTeamPowerPlayGoldDifference = 1000;
        public const int MediocreTeamPowerPlayGoldDifference = 1500;
        public const int LargeTeamPowerPlayGoldDifference = 3000;
        
        public static TimeSpan InhibitorRespawnTime { get; } = TimeSpan.FromMinutes(5);

        public static TimeSpan BaronBuffDuration { get; } = TimeSpan.FromMinutes(3);
        public static TimeSpan ElderDragonBuffDuration { get; } = TimeSpan.FromMinutes(2);
    }
}