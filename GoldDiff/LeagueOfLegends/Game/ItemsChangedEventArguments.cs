using System.Collections.Generic;
using System.Linq;

namespace GoldDiff.LeagueOfLegends.Game
{
    public class ItemsChangedEventArguments
    {
        public IEnumerable<LoLItem> AddedItems { get; }

        public IEnumerable<LoLItem> RemovedItems { get; }

        public ItemsChangedEventArguments(IEnumerable<LoLItem>? addedItems, IEnumerable<LoLItem>? removedItems)
        {
            AddedItems = addedItems?.ToList() ?? new List<LoLItem>();
            RemovedItems = removedItems?.ToList() ?? new List<LoLItem>();
        }
    }
}