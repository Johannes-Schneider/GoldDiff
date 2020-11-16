using System;
using System.Collections.Generic;
using System.Linq;
using FlatXaml.Model;
using GoldDiff.LeagueOfLegends.ClientApi;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.LeagueOfLegends.Game
{
    public sealed class LoLTeam : ViewModel, ILoLGoldOwner, ILoLScoreOwner, ILoLClientGameDataConsumer
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

    #region ILoLScoreOwner

        private int _kills;

        public int Kills
        {
            get => _kills;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _kills, value);
            }
        }

        private int _deaths;

        public int Deaths
        {
            get => _deaths;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _deaths, value);
            }
        }

        private int _assists;

        public int Assists
        {
            get => _assists;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _assists, value);
            }
        }

        private double _vision;

        public double Vision
        {
            get => _vision;
            private set
            {
                if (value < 0.0d)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _vision, value);
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

            if (!Players.Any())
            {
                throw new ArgumentException($"{nameof(players)} must contain at least 1 element!");
            }

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
            Deaths = Players.Sum(player => player.Deaths);
            Assists = Players.Sum(player => player.Assists);
            Vision = Players.Sum(player => player.Vision);

            var playerNames = Players.Select(player => player.SummonerName).ToList();
            TurretsKilled = gameData.EventCollection
                                    .Events
                                    .Where(e => e.EventType == LoLClientEventType.TurretKilled)
                                    .Cast<LoLClientTurretKilledEvent>()
                                    .Count(e => playerNames.Contains(e.KillerName) || e.AssistersNames.Any(name => playerNames.Contains(name)));
        }
    }
}