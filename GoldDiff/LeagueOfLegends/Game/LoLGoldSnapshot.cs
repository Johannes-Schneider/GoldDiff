using System;
using GoldDiff.View.Settings;

namespace GoldDiff.LeagueOfLegends.Game
{
    public class LoLGoldSnapshot
    {
        public TimeSpan GameTime { get; }

        public int TotalGold { get; }

        public int NonConsumableGold { get; }

        public int Gold => ViewSettings.Instance.DisplayGoldType switch
                           {
                               DisplayGoldType.Total => TotalGold,
                               DisplayGoldType.NonConsumable => NonConsumableGold,
                               _ => throw new Exception($"Unknown {nameof(DisplayGoldType)} {ViewSettings.Instance.DisplayGoldType}!"),
                           };

        public LoLGoldSnapshot(TimeSpan gameTime, int totalGold, int nonConsumableGold)
        {
            GameTime = gameTime;
            TotalGold = totalGold;
            NonConsumableGold = nonConsumableGold;
        }
    }
}