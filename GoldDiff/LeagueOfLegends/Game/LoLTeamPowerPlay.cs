using System;
using System.Collections.Generic;
using FlatXaml.Model;
using GoldDiff.LeagueOfLegends.ClientApi;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.LeagueOfLegends.Game
{
    public class LoLTeamPowerPlay : ViewModel, ILoLClientGameDataConsumer, ILoLGoldOwner
    {
        public event EventHandler<LoLGoldSnapshot>? GoldSnapshotAdded; 
        
        public LoLTeamType Team => OwningTeam.Side;

        private TimeSpan? _remainingDuration;

        public TimeSpan? RemainingDuration
        {
            get => _remainingDuration;
            set
            {
                if (!MutateVerbose(ref _remainingDuration, value))
                {
                    return;
                }
                
                RemainingDurationRelative = (value?.TotalSeconds ?? 0.0d) / TotalDuration.TotalSeconds;
                IsActive = value != null;
            }
        }

        private double _remainingDurationRelative;

        public double RemainingDurationRelative
        {
            get => _remainingDurationRelative;
            private set => MutateVerbose(ref _remainingDurationRelative, value);
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            private set => MutateVerbose(ref _isActive, value);
        }

        private int _totalGold;

        public int TotalGold
        {
            get => _totalGold;
            private set => MutateVerbose(ref _totalGold, value);
        }

        private int _nonConsumableGold;

        public int NonConsumableGold
        {
            get => _nonConsumableGold;
            private set => MutateVerbose(ref _nonConsumableGold, value);
        }

        private readonly List<LoLGoldSnapshot> _goldSnapshots = new();

        public IEnumerable<LoLGoldSnapshot> GoldSnapshots => _goldSnapshots;

        private TimeSpan StartTime { get; }
        private TimeSpan TotalDuration { get; }
        private LoLTeam OwningTeam { get; }
        private LoLTeam OpponentTeam { get; }
        private int InitialTotalGoldDifference { get; }
        private int InitialNonConsumableGoldDifference { get; }

        public LoLTeamPowerPlay(LoLGame? game, LoLTeamType powerPlayTeam, TimeSpan totalDuration)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            StartTime = game.Time;
            TotalDuration = totalDuration;

            OwningTeam = powerPlayTeam switch
                   {
                       LoLTeamType.BlueSide => game.TeamBlueSide,
                       LoLTeamType.RedSide => game.TeamRedSide,
                       _ => throw new ArgumentException($"Unknown {nameof(LoLTeamType)} {powerPlayTeam}!"),
                   } ?? throw new Exception($"Unable to determine the {nameof(OwningTeam)} of {nameof(LoLTeamPowerPlay)}!");

            OpponentTeam = powerPlayTeam switch
                           {
                               LoLTeamType.BlueSide => game.TeamRedSide,
                               LoLTeamType.RedSide => game.TeamBlueSide,
                               _ => throw new ArgumentException($"Unknown {nameof(LoLTeamType)} {powerPlayTeam}!"),
                           } ?? throw new Exception($"Unable to determine the {nameof(OpponentTeam)} of {nameof(LoLTeamPowerPlay)}!");

            InitialTotalGoldDifference = OwningTeam.TotalGold - OpponentTeam.TotalGold;
            InitialNonConsumableGoldDifference = OwningTeam.NonConsumableGold - OpponentTeam.NonConsumableGold;
        }

        public void Consume(LoLClientGameData gameData)
        {
            if (gameData == null)
            {
                throw new ArgumentNullException(nameof(gameData));
            }

            Update(gameData.Stats.GameTime);
        }

        private void Update(TimeSpan currentGameTime)
        {
            var duration = TotalDuration - (currentGameTime - StartTime);
            RemainingDuration = duration.TotalSeconds < 0.0d ? null : duration;

            if (!IsActive)
            {
                return;
            }

            var currentTotalGoldDifference = OwningTeam.TotalGold - OpponentTeam.TotalGold;
            TotalGold = currentTotalGoldDifference - InitialTotalGoldDifference;

            var currentNonConsumableGoldDifference = OwningTeam.NonConsumableGold - OpponentTeam.NonConsumableGold;
            NonConsumableGold = currentNonConsumableGoldDifference - InitialNonConsumableGoldDifference;

            var newGoldSnapshot = new LoLGoldSnapshot(currentGameTime, TotalGold, NonConsumableGold);
            _goldSnapshots.Add(newGoldSnapshot);

            if (GoldSnapshotAdded != null)
            {
                OnEventDispatcher(() => GoldSnapshotAdded.Invoke(this, newGoldSnapshot));
            }
        }
    }
}