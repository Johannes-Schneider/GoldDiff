using System;
using System.Collections.Generic;
using System.Linq;
using GoldDiff.LeagueOfLegends.ClientApi;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.LeagueOfLegends.Game
{
    public class LoLRespawnTimerCollection : ILoLClientGameDataConsumer
    {
    #region Inhibitors Blue Side
        
        public LoLRespawnTimer TopInhibitorBlueSide { get; } = new();
        public LoLRespawnTimer MiddleInhibitorBlueSide { get; } = new();

        public LoLRespawnTimer BottomInhibitorBlueSide { get; } = new();

    #endregion

    #region Inhibitors Red Side

        public LoLRespawnTimer TopInhibitorRedSide { get; } = new();

        public LoLRespawnTimer MiddleInhibitorRedSide { get; } = new();

        public LoLRespawnTimer BottomInhibitorRedSide { get; } = new();

    #endregion

        public void Consume(LoLClientGameData gameData)
        {
            if (gameData == null)
            {
                throw new ArgumentNullException(nameof(gameData));
            }

            var reversedEvents = new List<LoLClientEvent>(gameData.EventCollection.Events);
            reversedEvents.Reverse();

            TopInhibitorBlueSide.Time = InhibitorRespawnTime(gameData, reversedEvents, LoLTeamType.BlueSide, LoLClientInhibitorTier.Top);
            MiddleInhibitorBlueSide.Time = InhibitorRespawnTime(gameData, reversedEvents, LoLTeamType.BlueSide, LoLClientInhibitorTier.Middle);
            BottomInhibitorBlueSide.Time = InhibitorRespawnTime(gameData, reversedEvents, LoLTeamType.BlueSide, LoLClientInhibitorTier.Bottom);
            TopInhibitorRedSide.Time = InhibitorRespawnTime(gameData, reversedEvents, LoLTeamType.RedSide, LoLClientInhibitorTier.Top);
            MiddleInhibitorRedSide.Time = InhibitorRespawnTime(gameData, reversedEvents, LoLTeamType.RedSide, LoLClientInhibitorTier.Middle);
            BottomInhibitorRedSide.Time = InhibitorRespawnTime(gameData, reversedEvents, LoLTeamType.RedSide, LoLClientInhibitorTier.Bottom);
        }

        private TimeSpan? InhibitorRespawnTime(LoLClientGameData gameData, ICollection<LoLClientEvent> reversedEvents, LoLTeamType team, LoLClientInhibitorTier tier)
        {
            var latestKilledEvent = reversedEvents.Where(e => e.EventType == LoLClientEventType.InhibitorKilled)
                                                  .Cast<LoLClientInhibitorKilledEvent>()
                                                  .FirstOrDefault(e => e.Inhibitor.Team.Equals(team) && e.Inhibitor.Tier.Equals(tier));

            if (latestKilledEvent == null)
            {
                // inhibitor has not been killed yet
                return null;
            }

            var latestRespawnedEvent = reversedEvents.Where(e => e.EventType == LoLClientEventType.InhibitorRespawned)
                                                     .Cast<LoLClientInhibitorRespawnedEvent>()
                                                     .FirstOrDefault(e => e.Inhibitor.Team.Equals(team) && e.Inhibitor.Tier.Equals(tier));

            if (latestRespawnedEvent?.GameTime >= latestKilledEvent.GameTime)
            {
                // inhibitor is respawned already
                return null;
            }

            return LoLConstants.InhibitorRespawnTime - (gameData.Stats.GameTime - latestKilledEvent.GameTime);
        }
    }
}