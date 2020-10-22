using System.ComponentModel;

namespace GoldDiff.LeagueOfLegends.Game
{
    public interface ILoLScoreOwner : INotifyPropertyChanged
    {
        int Kills { get; }
        
        int Deaths { get; }
        
        int Assists { get; }
        
        double Vision { get; }
    }
}