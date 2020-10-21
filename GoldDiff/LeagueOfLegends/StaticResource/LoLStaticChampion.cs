using System;
using System.IO;

namespace GoldDiff.LeagueOfLegends.StaticResource
{
    public class LoLStaticChampion
    {
        public int Id { get; }
        
        public string Name { get; }
        
        public string SmallTileImage { get; }
        
        public string LargeTileImage { get; }

        public LoLStaticChampion(int id, string? name, string? smallTileImage, string? largeTileImage)
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

            if (!File.Exists(largeTileImage))
            {
                throw new ArgumentException($"{largeTileImage} cannot be found!");
            }

            Id = id;
            Name = name!;
            SmallTileImage = smallTileImage!;
            LargeTileImage = largeTileImage!;
        }
    }
}