using System.ComponentModel;

namespace GoldDiff.LeagueOfLegends.Game
{
    public interface ILoLGoldOwner : INotifyPropertyChanged
    {
        int TotalGold { get; }
        
        int NonConsumableGold { get; }
    }
}