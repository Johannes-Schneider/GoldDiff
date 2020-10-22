using System;
using System.Collections.Generic;

namespace GoldDiff.LeagueOfLegends.Game
{
    public interface ILoLItemOwner
    {
        event EventHandler<ItemsChangedEventArguments> ItemsChanged;

        IEnumerable<LoLItem> Items { get; }
    }
}