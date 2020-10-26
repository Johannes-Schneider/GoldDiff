using System;
using System.Linq;
using GoldDiff.LeagueOfLegends.ClientApi;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using GoldDiff.LeagueOfLegends.StaticResource;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.View.Model;

namespace GoldDiff.LeagueOfLegends.Game
{
    public class LoLGame : ViewModel, ILoLClientGameDataConsumer
    {
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
        
        public LoLStaticResourceCache StaticResourceCache { get; }

        public LoLGame(LoLStaticResourceCache? staticResourceCache)
        {
            StaticResourceCache = staticResourceCache ?? throw new ArgumentNullException(nameof(staticResourceCache));
        }
        
        public void Consume(LoLClientGameData? gameData)
        {
            if (gameData == null)
            {
                throw new ArgumentNullException(nameof(gameData));
            }
            
            Mode = gameData.Stats.GameMode;
            if (Mode != LoLGameModeType.Classic5X5)
            {
                // We do support only classic 5v5 games right now
                State = LoLGameStateType.Undefined;
                return;
            }
            
            if (State == LoLGameStateType.Undefined)
            {
                var teams = LoLTeamFactory.ExtractTeams(gameData, StaticResourceCache).ToList();
                TeamBlueSide = teams.FirstOrDefault(team => team.Side == LoLTeamType.BlueSide);
                TeamRedSide = teams.FirstOrDefault(team => team.Side == LoLTeamType.RedSide);
            }

            if (TeamBlueSide == null || TeamRedSide == null)
            {
                return;
            }

            if (gameData.EventCollection.Events.LastOrDefault()?.EventType == LoLClientEventType.GameEnded)
            {
                State = LoLGameStateType.Ended;
            }
            else if (gameData.EventCollection.Events.FirstOrDefault()?.EventType == LoLClientEventType.GameStarted)
            {
                State = LoLGameStateType.Started;
            }
            
            TeamBlueSide.Consume(gameData);
            TeamRedSide.Consume(gameData);

            EventDispatcher!.Invoke(() => GameDataReceived?.Invoke(this, gameData));
        }
    }
}