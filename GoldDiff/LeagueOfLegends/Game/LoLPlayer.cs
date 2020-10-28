using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GoldDiff.LeagueOfLegends.ClientApi;
using GoldDiff.LeagueOfLegends.ClientApi.Player;
using GoldDiff.LeagueOfLegends.StaticResource;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.Utility;
using GoldDiff.Shared.View.Model;

namespace GoldDiff.LeagueOfLegends.Game
{
    public sealed class LoLPlayer : ViewModel, ILoLGoldOwner, ILoLItemOwner, ILoLScoreOwner, ILoLClientGameDataConsumer
    {
        public event EventHandler<ItemsChangedEventArguments>? ItemsChanged;

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

    #region ILoLItemOwner

        public IEnumerable<LoLItem> Items => new ReadOnlyCollection<LoLItem>(MutableItems);

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

                if (!MutateVerbose(ref _kills, value))
                {
                    return;
                }
                KillsSinceLastItemAcquisition = Kills - _killsAtLastItemAcquisition;
            }
        }

        private int _killsAtLastItemAcquisition;

        public int KillsAtLastItemAcquisition
        {
            get => _killsAtLastItemAcquisition;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (!MutateVerbose(ref _killsAtLastItemAcquisition, value))
                {
                    return;
                }
                KillsSinceLastItemAcquisition = Kills - KillsSinceLastItemAcquisition;
            }
        }

        private int _killsSinceLastItemAcquisition;

        public int KillsSinceLastItemAcquisition
        {
            get => _killsSinceLastItemAcquisition;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _killsSinceLastItemAcquisition, value);
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

                if (!MutateVerbose(ref _deaths, value))
                {
                    return;
                }
                DeathsSinceLastItemAcquisition = Deaths - DeathsSinceLastItemAcquisition;
            }
        }

        private int _deathsAtLastItemAcquisition;

        public int DeathsAtLastItemAcquisition
        {
            get => _deathsAtLastItemAcquisition;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (!MutateVerbose(ref _deathsAtLastItemAcquisition, value))
                {
                    return;
                }
                DeathsSinceLastItemAcquisition = Deaths - DeathsAtLastItemAcquisition;
            }
        }

        private int _deathsSinceLastItemAcquisition;

        public int DeathsSinceLastItemAcquisition
        {
            get => _deathsSinceLastItemAcquisition;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _deathsSinceLastItemAcquisition, value);
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

                if (!MutateVerbose(ref _assists, value))
                {
                    return;
                }
                AssistsSinceLastItemAcquisition = Assists - AssistsAtLastItemAcquisition;
            }
        }

        private int _assistsAtLastItemAcquisition;

        public int AssistsAtLastItemAcquisition
        {
            get => _assistsAtLastItemAcquisition;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (!MutateVerbose(ref _assistsAtLastItemAcquisition, value))
                {
                    return;
                }
                AssistsSinceLastItemAcquisition = Assists - AssistsAtLastItemAcquisition;
            }
        }

        private int _assistsSinceLastItemAcquisition;

        public int AssistsSinceLastItemAcquisition
        {
            get => _assistsSinceLastItemAcquisition;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _assistsSinceLastItemAcquisition, value);
            }
        }

        private int _minionKills;

        public int MinionKills
        {
            get => _minionKills;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (!MutateVerbose(ref _minionKills, value))
                {
                    return;
                }
                MinionKillsSinceLastItemAcquisition = MinionKills - MinionKillsAtLastItemAcquisition;
            }
        }

        private int _minionKillsAtLastItemAcquisition;

        public int MinionKillsAtLastItemAcquisition
        {
            get => _minionKillsAtLastItemAcquisition;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (!MutateVerbose(ref _minionKillsAtLastItemAcquisition, value))
                {
                    return;
                }
                MinionKillsSinceLastItemAcquisition = MinionKills - MinionKillsAtLastItemAcquisition;
            }
        }

        private int _minionKillsSinceLastItemAcquisition;

        public int MinionKillsSinceLastItemAcquisition
        {
            get => _minionKillsSinceLastItemAcquisition;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _minionKillsSinceLastItemAcquisition, value);
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

        public string SummonerName { get; }

        public LoLTeamType Team { get; }

        public LoLStaticChampion Champion { get; }

        public bool IsActivePlayer { get; }
        
        public LoLPositionType AssignedPosition { get; }

        private LoLStaticResourceCache StaticResourceCache { get; }
        private List<LoLItem> MutableItems { get; set; } = new List<LoLItem>();

        public LoLPlayer(LoLStaticResourceCache? staticResourceCache, string? summonerName, LoLTeamType team, LoLPositionType assignedPosition, LoLStaticChampion? champion, bool isActivePlayer)
        {
            if (string.IsNullOrEmpty(summonerName))
            {
                throw new ArgumentNullException(nameof(summonerName));
            }

            StaticResourceCache = staticResourceCache ?? throw new ArgumentNullException(nameof(staticResourceCache));
            SummonerName = summonerName!;
            Team = team;
            AssignedPosition = assignedPosition;
            Champion = champion ?? throw new ArgumentNullException(nameof(champion));
            IsActivePlayer = isActivePlayer;
        }

        public void Consume(LoLClientGameData? gameData)
        {
            if (gameData == null)
            {
                throw new ArgumentNullException(nameof(gameData));
            }

            var clientPlayer = gameData.Players.FirstOrDefault(player => player.SummonerName.Equals(SummonerName) && player.Team == Team);
            if (clientPlayer == null)
            {
                throw new Exception($"Unable to find the {nameof(LoLClientPlayer)} named {SummonerName} in the given {nameof(LoLClientGameData)}!");
            }

            UpdateScore(clientPlayer);
            UpdateItems(clientPlayer);
            UpdateGold(clientPlayer);
        }

        private void UpdateItems(LoLClientPlayer clientPlayer)
        {
            var oldItemIds = Items.SelectMany(item => Enumerable.Repeat(item.StaticProperties.Id, item.Amount)).ToList();
            var newItemIds = clientPlayer.Items.SelectMany(item => Enumerable.Repeat(item.Id, item.Amount)).ToList();

            var addedItems = MultiSet.Difference(newItemIds, oldItemIds).GroupBy(id => id).Select(group => new LoLItem(StaticResourceCache.GetItem(group.Key), group.Count()));
            var removedItems = MultiSet.Difference(oldItemIds, newItemIds).GroupBy(id => id).Select(group => new LoLItem(StaticResourceCache.GetItem(group.Key), group.Count()));

            MutableItems = clientPlayer.Items.Select(item => new LoLItem(StaticResourceCache.GetItem(item.Id), item.Amount)).ToList();
            EventDispatcher!.Invoke(() => ItemsChanged?.Invoke(this, new ItemsChangedEventArguments(addedItems, removedItems)));

            if (addedItems.Any())
            {
                KillsAtLastItemAcquisition = Kills;
                DeathsAtLastItemAcquisition = Deaths;
                AssistsAtLastItemAcquisition = Assists;
                MinionKillsAtLastItemAcquisition = MinionKills;
            }
        }

        private void UpdateGold(LoLClientPlayer clientPlayer)
        {
            var nonConsumableItemIds = clientPlayer.Items.Where(item => !item.IsConsumable).Select(item => item.Id).ToList();
            
            TotalGold = Items.Sum(item => Math.Max(0, item.Amount) * item.StaticProperties.TotalCosts);
            NonConsumableGold = Items.Where(item => nonConsumableItemIds.Contains(item.StaticProperties.Id)).Sum(item => Math.Max(0, item.Amount) * item.StaticProperties.TotalCosts);
        }

        private void UpdateScore(LoLClientPlayer clientPlayer)
        {
            Kills = clientPlayer.Score.Kills;
            Deaths = clientPlayer.Score.Deaths;
            Assists = clientPlayer.Score.Assists;
            Vision = clientPlayer.Score.Vision;
            MinionKills = clientPlayer.Score.MinionKills;
        }
    }
}