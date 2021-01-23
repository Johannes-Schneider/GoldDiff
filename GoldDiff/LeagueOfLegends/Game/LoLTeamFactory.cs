using System;
using System.Collections.Generic;
using System.Linq;
using GoldDiff.LeagueOfLegends.ClientApi;
using GoldDiff.LeagueOfLegends.StaticResource;

namespace GoldDiff.LeagueOfLegends.Game
{
    public static class LoLTeamFactory
    {
        public static IEnumerable<LoLTeam> ExtractTeams(LoLClientGameData? gameData, LoLStaticResourceCache? staticResourceCache)
        {
            if (gameData == null)
            {
                throw new ArgumentNullException(nameof(gameData));
            }

            if (staticResourceCache == null)
            {
                throw new ArgumentNullException(nameof(staticResourceCache));
            }

            return ExtractPlayers(gameData, staticResourceCache)
                   .GroupBy(player => player.Team)
                   .Select(group => new LoLTeam(group.Key, group));
        }

        private static IEnumerable<LoLPlayer> ExtractPlayers(LoLClientGameData gameData, LoLStaticResourceCache staticResourceCache)
        {
            return gameData.Players.Select(clientPlayer => new LoLPlayer(staticResourceCache,
                                                                         clientPlayer.SummonerName,
                                                                         clientPlayer.Team,
                                                                         staticResourceCache.GetChampion(clientPlayer.ChampionName),
                                                                         gameData.ActivePlayer.SummonerName.Equals(clientPlayer.SummonerName))
                                                                         {
                                                                             Position = clientPlayer.Position,
                                                                         });
        }
    }
}