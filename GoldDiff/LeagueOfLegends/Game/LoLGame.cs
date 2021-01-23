using System;
using System.Linq;
using FlatXaml.Model;
using GoldDiff.LeagueOfLegends.ClientApi;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using GoldDiff.LeagueOfLegends.StaticResource;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.View.Settings;

namespace GoldDiff.LeagueOfLegends.Game
{
    public class LoLGame : ViewModel, ILoLClientGameDataConsumer
    {
        public event EventHandler? Initialized;
        public event EventHandler<LoLClientGameData>? GameDataReceived;

        private LoLGameStateType _state = LoLGameStateType.Undefined;

        public LoLGameStateType State
        {
            get => _state;
            private set => MutateVerbose(ref _state, value);
        }

        private LoLGameModeType _mode = LoLGameModeType.Undefined;

        public LoLGameModeType Mode
        {
            get => _mode;
            private set => MutateVerbose(ref _mode, value);
        }

        private LoLTeam? _teamBlueSide;

        public LoLTeam? TeamBlueSide
        {
            get => _teamBlueSide;
            private set => MutateVerboseIfNotNull(ref _teamBlueSide, value);
        }

        private LoLTeam? _teamRedSide;

        public LoLTeam? TeamRedSide
        {
            get => _teamRedSide;
            set => MutateVerboseIfNotNull(ref _teamRedSide, value);
        }

        private bool _isInitialized = false;

        public bool IsInitialized
        {
            get => _isInitialized;
            private set => MutateVerbose(ref _isInitialized, value);
        }

        private TimeSpan _time = TimeSpan.Zero;

        public TimeSpan Time
        {
            get => _time;
            private set => MutateVerbose(ref _time, value);
        }

        private LoLRespawnTimerCollection _respawnTimerCollection = new();

        public LoLRespawnTimerCollection RespawnTimerCollection
        {
            get => _respawnTimerCollection;
            private set => MutateVerboseIfNotNull(ref _respawnTimerCollection, value);
        }

        private LoLTeamPowerPlayCollection _teamPowerPlayCollection = null!;

        public LoLTeamPowerPlayCollection TeamPowerPlayCollection
        {
            get => _teamPowerPlayCollection;
            private set => MutateVerboseIfNotNull(ref _teamPowerPlayCollection, value);
        }

        public LoLStaticResourceCache StaticResourceCache { get; }


        public LoLGame(LoLStaticResourceCache? staticResourceCache)
        {
            StaticResourceCache = staticResourceCache ?? throw new ArgumentNullException(nameof(staticResourceCache));
        }

        public void GameClientClosed()
        {
            if (!IsInitialized)
            {
                return;
            }

            State = LoLGameStateType.Ended;
        }

        public void Consume(LoLClientGameData? gameData)
        {
            if (gameData == null)
            {
                throw new ArgumentNullException(nameof(gameData));
            }

            var invokeInitializedEvent = false;
            if (!IsInitialized)
            {
                if (!Initialize(gameData))
                {
                    return;
                }

                invokeInitializedEvent = true;
            }

            Time = gameData.Stats.GameTime;
            UpdateState(gameData);

            TeamPowerPlayCollection.Consume(gameData);
            TeamBlueSide?.Consume(gameData);
            TeamRedSide?.Consume(gameData);
            RespawnTimerCollection.Consume(gameData);

            if (invokeInitializedEvent && Initialized != null)
            {
                EventDispatcher!.Invoke(() => Initialized.Invoke(this, EventArgs.Empty));
            }

            EventDispatcher!.Invoke(() => GameDataReceived?.Invoke(this, gameData));
        }

        private bool Initialize(LoLClientGameData gameData)
        {
            if (IsInitialized)
            {
                return true;
            }

            Mode = gameData.Stats.GameMode;
            if (!ViewSettings.Instance.EnableForAllGameModes
                && Mode != LoLGameModeType.Classic5X5
            #if DEBUG
                && Mode != LoLGameModeType.PracticeTool
            #endif
            )
            {
                // We do support only classic 5v5 games right now
                State = LoLGameStateType.Undefined;
                return false;
            }

            var teams = LoLTeamFactory.ExtractTeams(gameData, StaticResourceCache).ToList();
            TeamBlueSide = teams.FirstOrDefault(team => team.Side == LoLTeamType.BlueSide);
            TeamRedSide = teams.FirstOrDefault(team => team.Side == LoLTeamType.RedSide);

            if (TeamBlueSide == null && TeamRedSide == null)
            {
                return false;
            }

            TeamBlueSide ??= new LoLTeam(LoLTeamType.BlueSide, Enumerable.Empty<LoLPlayer>());
            TeamRedSide ??= new LoLTeam(LoLTeamType.RedSide, Enumerable.Empty<LoLPlayer>());

            RespawnTimerCollection = new LoLRespawnTimerCollection();
            TeamPowerPlayCollection = new LoLTeamPowerPlayCollection(this);
            IsInitialized = true;
            return true;
        }

        private void UpdateState(LoLClientGameData gameData)
        {
            if (gameData.EventCollection.Events.LastOrDefault()?.EventType == LoLClientEventType.GameEnded)
            {
                State = LoLGameStateType.Ended;
            }
            else if (gameData.EventCollection.Events.FirstOrDefault()?.EventType == LoLClientEventType.GameStarted)
            {
                State = LoLGameStateType.Started;
            }
            else
            {
                State = LoLGameStateType.PreStart;
            }
        }
    }
}