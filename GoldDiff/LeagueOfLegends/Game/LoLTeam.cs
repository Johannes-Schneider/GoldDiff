using System;
using System.Collections.Generic;
using System.Linq;
using FlatXaml.Model;
using GoldDiff.LeagueOfLegends.ClientApi;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.LeagueOfLegends.Game
{
    public sealed class LoLTeam : BaseLoLScoreOwner, ILoLGoldOwner, ILoLClientGameDataConsumer
    {
    #region ILoLGoldOwner

        private int _totalGold;

        public int TotalGold
        {
            get => _totalGold;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _totalGold, value);
            }
        }

        private int _nonConsumableGold;

        public int NonConsumableGold
        {
            get => _nonConsumableGold;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _nonConsumableGold, value);
            }
        }

    #endregion

        private int _turretsKilled;

        public int TurretsKilled
        {
            get => _turretsKilled;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _turretsKilled, value);
            }
        }

        public LoLTeamType Side { get; }

        public IEnumerable<LoLPlayer> Players { get; }

        public LoLTeam(LoLTeamType team, IEnumerable<LoLPlayer>? players)
        {
            Side = team;
            Players = players?.ToList() ?? throw new ArgumentNullException(nameof(players));

            if (Players.Any(player => player.Team != team))
            {
                throw new ArgumentException($"At least one {nameof(LoLPlayer)} given in the {nameof(players)} does not belong to this {nameof(LoLTeam)}!");
            }
        }

        public void Consume(LoLClientGameData? gameData)
        {
            if (gameData == null)
            {
                throw new ArgumentNullException(nameof(gameData));
            }

            UpdatePlayers(gameData);
            UpdateGold();
            UpdateScore(gameData);
        }

        private void UpdatePlayers(LoLClientGameData gameData)
        {
            foreach (var player in Players)
            {
                player.Consume(gameData);
            }
        }

        private void UpdateGold()
        {
            TotalGold = Players.Sum(player => player.TotalGold);
            NonConsumableGold = Players.Sum(player => player.NonConsumableGold);
        }

        private void UpdateScore(LoLClientGameData gameData)
        {
            Kills = Players.Sum(player => player.Kills);
            KillsAtLastItemAcquisition = Players.Sum(player => player.KillsAtLastItemAcquisition);
            KillsSinceLastItemAcquisition = Players.Sum(player => player.KillsSinceLastItemAcquisition);
            Deaths = Players.Sum(player => player.Deaths);
            DeathsAtLastItemAcquisition = Players.Sum(player => player.DeathsAtLastItemAcquisition);
            DeathsSinceLastItemAcquisition = Players.Sum(player => player.DeathsSinceLastItemAcquisition);
            Assists = Players.Sum(player => player.Assists);
            AssistsAtLastItemAcquisition = Players.Sum(player => player.AssistsAtLastItemAcquisition);
            AssistsSinceLastItemAcquisition = Players.Sum(player => player.AssistsSinceLastItemAcquisition);
            Vision = Players.Sum(player => player.Vision);

            var playerNames = Players.Select(player => player.SummonerName).ToList();
            TurretsKilled = gameData.EventCollection
                                    .Events
                                    .Where(e => e.EventType == LoLClientEventType.TurretKilled)
                                    .Cast<LoLClientTurretKilledEvent>()
                                    .Count(e => playerNames.Contains(e.KillerName) || e.AssistersNames.Any(name => playerNames.Contains(name)))
                            + gameData.EventCollection.Events.Where(e => e.EventType == LoLClientEventType.FirstTurretKilled)
                                      .Cast<LoLClientFirstTurretKilledEvent>()
                                      .Count(e => playerNames.Contains(e.KillerName));
        }
    }
}