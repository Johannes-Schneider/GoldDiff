using System;
using System.Collections.Generic;
using System.Linq;
using FlatXaml.Model;
using GoldDiff.LeagueOfLegends.ClientApi;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.LeagueOfLegends.Game
{
    public class LoLTeamPowerPlayCollection : ViewModel, ILoLClientGameDataConsumer
    {
        private LoLTeamPowerPlay? _baronPowerPlay;

        public LoLTeamPowerPlay? BaronPowerPlay
        {
            get => _baronPowerPlay;
            private set => MutateVerbose(ref _baronPowerPlay, value);
        }

        private LoLTeamPowerPlay? _elderDragonPowerPlay;

        public LoLTeamPowerPlay? ElderDragonPowerPlay
        {
            get => _elderDragonPowerPlay;
            private set => MutateVerbose(ref _elderDragonPowerPlay, value);
        }

        private bool _anyActive = false;

        public bool AnyActive
        {
            get => _anyActive;
            private set => MutateVerbose(ref _anyActive, value);
        }

        private LoLGame Game { get; }

        public LoLTeamPowerPlayCollection(LoLGame? game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public void Consume(LoLClientGameData gameData)
        {
            if (gameData == null)
            {
                throw new ArgumentNullException(nameof(gameData));
            }

            var reversedEvents = new List<LoLClientEvent>(gameData.EventCollection.Events);
            reversedEvents.Reverse();

            UpdateBaronPowerPlay(gameData, reversedEvents);
            UpdateElderDragonPowerPlay(gameData, reversedEvents);

            AnyActive = BaronPowerPlay?.IsActive == true || ElderDragonPowerPlay?.IsActive == true;
        }

        private void UpdateBaronPowerPlay(LoLClientGameData gameData, IEnumerable<LoLClientEvent> reversedEvents)
        {
            var latestKillEvent = reversedEvents.FirstOrDefault(e => e.EventType == LoLClientEventType.BaronKilled) as LoLClientBaronKilledEvent;
            if (latestKillEvent == null
                || latestKillEvent.GameTime < gameData.Stats.GameTime - LoLConstants.BaronBuffDuration)
            {
                BaronPowerPlay = null;
                return;
            }

            if (BaronPowerPlay == null)
            {
                var killerTeam = LoLTeamType.RedSide;
                if (Game.TeamBlueSide?.Players.Any(p => p.SummonerName.Equals(latestKillEvent!.KillerName)) == true)
                {
                    killerTeam = LoLTeamType.BlueSide;
                }

                BaronPowerPlay = new LoLTeamPowerPlay(Game, killerTeam, LoLConstants.BaronBuffDuration);
            }

            BaronPowerPlay.Consume(gameData);
        }

        private void UpdateElderDragonPowerPlay(LoLClientGameData gameData, IEnumerable<LoLClientEvent> reversedEvents)
        {
            var latestKillEvent = reversedEvents.Where(e => e.EventType == LoLClientEventType.DragonKilled)
                                                .Cast<LoLClientDragonKilledEvent>()
                                                .FirstOrDefault(e => e.DragonType == LoLClientDragonType.Elder) as LoLClientDragonKilledEvent;
            if (latestKillEvent == null ||
                latestKillEvent.GameTime < gameData.Stats.GameTime - LoLConstants.ElderDragonBuffDuration)
            {
                ElderDragonPowerPlay = null;
                return;
            }

            if (ElderDragonPowerPlay == null)
            {
                var killerTeam = LoLTeamType.RedSide;
                if (Game.TeamBlueSide?.Players.Any(p => p.SummonerName.Equals(latestKillEvent!.KillerName)) == true)
                {
                    killerTeam = LoLTeamType.BlueSide;
                }

                ElderDragonPowerPlay = new LoLTeamPowerPlay(Game, killerTeam, LoLConstants.ElderDragonBuffDuration);
            }

            ElderDragonPowerPlay.Consume(gameData);
        }
    }
}