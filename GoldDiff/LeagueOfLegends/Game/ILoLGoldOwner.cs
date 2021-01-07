using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GoldDiff.LeagueOfLegends.Game
{
    public interface ILoLGoldOwner : INotifyPropertyChanged
    {
        event EventHandler<LoLGoldSnapshot> GoldSnapshotAdded; 
        
        int TotalGold { get; }
        
        int NonConsumableGold { get; }
        
        IEnumerable<LoLGoldSnapshot> GoldSnapshots { get; }
    }
}