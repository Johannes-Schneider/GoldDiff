using System;
using System.IO;

namespace GoldDiff.LeagueOfLegends.StaticResource
{
    public class LoLStaticItem
    {
        public int Id { get; }
        
        public string Name { get; }
        
        public string SmallTileImage { get; }
        
        public int TotalCosts { get; }
        
        public int RecipeCosts { get; }
        
        public int SellWorth { get; }

        public LoLStaticItem(int id, string? name, string? smallTileImage, int totalCosts, int recipeCosts, int sellWorth)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!File.Exists(smallTileImage))
            {
                throw new ArgumentException($"{smallTileImage} cannot be found!");
            }

            if (totalCosts < 0 || totalCosts < recipeCosts)
            {
                throw new ArgumentOutOfRangeException(nameof(totalCosts));
            }

            if (recipeCosts < 0 || recipeCosts > totalCosts)
            {
                throw new ArgumentOutOfRangeException(nameof(recipeCosts));
            }

            if (sellWorth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sellWorth));
            }

            Id = id;
            Name = name!;
            SmallTileImage = smallTileImage!;
            TotalCosts = totalCosts;
            RecipeCosts = recipeCosts;
            SellWorth = sellWorth;
        }
    }
}