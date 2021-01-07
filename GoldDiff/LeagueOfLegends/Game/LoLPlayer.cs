using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FlatXaml.Model;
using GoldDiff.LeagueOfLegends.ClientApi;
using GoldDiff.LeagueOfLegends.ClientApi.Player;
using GoldDiff.LeagueOfLegends.StaticResource;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.Utility;

namespace GoldDiff.LeagueOfLegends.Game
{
    public sealed class LoLPlayer : BaseLoLScoreOwner, ILoLGoldOwner, ILoLItemOwner, ILoLClientGameDataConsumer
    {
        public event EventHandler<ItemsChangedEventArguments>? ItemsChanged;

    #region ILoLGoldOwner

        public event EventHandler<LoLGoldSnapshot>? GoldSnapshotAdded; 

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

        private readonly List<LoLGoldSnapshot> _goldSnapshots = new();

        public IEnumerable<LoLGoldSnapshot> GoldSnapshots => _goldSnapshots;

    #endregion

    #region ILoLItemOwner

        public IEnumerable<LoLItem> Items => new ReadOnlyCollection<LoLItem>(MutableItems);

    #endregion

        public string SummonerName { get; }

        public LoLTeamType Team { get; }

        public LoLStaticChampion Champion { get; }

        public bool IsActivePlayer { get; }
        
        public LoLPositionType AssignedPosition { get; }

        private LoLStaticResourceCache StaticResourceCache { get; }
        private List<LoLItem> MutableItems { get; set; } = new();

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
            AddGoldSnapshot(gameData.Stats.GameTime);
        }

        private void UpdateItems(LoLClientPlayer clientPlayer)
        {
            var oldItemIds = Items.SelectMany(item => Enumerable.Repeat(item.StaticProperties.Id, item.Amount)).ToList();
            var newItemIds = clientPlayer.Items.SelectMany(item => Enumerable.Repeat(item.Id, item.Amount)).ToList();
            
            var addedItems = MultiSet.Difference(newItemIds, oldItemIds).GroupBy(id => id).Select(group => new LoLItem(StaticResourceCache.GetItem(group.Key), group.Count()));

            MutableItems = clientPlayer.Items.Select(item => new LoLItem(StaticResourceCache.GetItem(item.Id), item.Amount)).ToList();
            if (ItemsChanged != null)
            {
                
                var removedItems = MultiSet.Difference(oldItemIds, newItemIds).GroupBy(id => id).Select(group => new LoLItem(StaticResourceCache.GetItem(group.Key), group.Count()));
                OnEventDispatcher(() => ItemsChanged.Invoke(this, new ItemsChangedEventArguments(addedItems, removedItems)));
            }

            if (addedItems.Any())
            {
                KillsAtLastItemAcquisition = Kills;
                DeathsAtLastItemAcquisition = Deaths;
                AssistsAtLastItemAcquisition = Assists;
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

        private void AddGoldSnapshot(TimeSpan gameTime)
        {
            var newGoldSnapshot = new LoLGoldSnapshot(gameTime, TotalGold, NonConsumableGold);
            _goldSnapshots.Add(newGoldSnapshot);

            if (GoldSnapshotAdded != null)
            {
                OnEventDispatcher(() => GoldSnapshotAdded.Invoke(this, newGoldSnapshot));
            }
        }

        public override string ToString()
        {
            return SummonerName;
        }
    }
}