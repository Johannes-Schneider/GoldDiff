using GoldDiff.LeagueOfLegends.ClientApi;

namespace GoldDiff.LeagueOfLegends.Game
{
    public interface ILoLClientGameDataConsumer
    {
        void Consume(LoLClientGameData gameData);
    }
}