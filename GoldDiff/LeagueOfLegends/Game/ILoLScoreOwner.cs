using System.ComponentModel;

namespace GoldDiff.LeagueOfLegends.Game
{
    public interface ILoLScoreOwner : INotifyPropertyChanged
    {
        int Kills { get; }

        int KillsAtLastItemAcquisition { get; }
        
        int KillsSinceLastItemAcquisition { get; }

        int Deaths { get; }
        
        int DeathsAtLastItemAcquisition { get; }
        
        int DeathsSinceLastItemAcquisition { get; }
        
        int Assists { get; }
        
        int AssistsAtLastItemAcquisition { get; }
        
        int AssistsSinceLastItemAcquisition { get; }
        
        double Vision { get; }
    }
}