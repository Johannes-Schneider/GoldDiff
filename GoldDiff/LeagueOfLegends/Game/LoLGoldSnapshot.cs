using System;

namespace GoldDiff.LeagueOfLegends.Game
{
    public class LoLGoldSnapshot
    {
        public TimeSpan GameTime { get; }
        
        public int TotalGold { get; }
        
        public int NonConsumableGold { get; }

        public LoLGoldSnapshot(TimeSpan gameTime, int totalGold, int nonConsumableGold)
        {
            GameTime = gameTime;
            TotalGold = totalGold;
            NonConsumableGold = nonConsumableGold;
        }
    }
}