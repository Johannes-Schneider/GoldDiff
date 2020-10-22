using System;
using GoldDiff.LeagueOfLegends.StaticResource;

namespace GoldDiff.LeagueOfLegends.Game
{
    public class LoLItem
    {
        public LoLStaticItem StaticProperties { get; }

        private int _amount = 1;

        public int Amount
        {
            get => _amount;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _amount = value;
            }
        }

        public LoLItem(LoLStaticItem? staticItem, int amount = 1)
        {
            StaticProperties = staticItem ?? throw new ArgumentNullException(nameof(staticItem));
            Amount = amount;
        }
    }
}