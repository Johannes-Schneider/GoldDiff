using System;
using System.Collections.Generic;

namespace GoldDiff.LeagueOfLegends.ClientApi.Event
{
    public enum LoLClientEventType
    {
        Undefined,
        
        GameStarted,
        
        FirstMinionWaveSpawned,
        
        FirstTurretKilled,
        
        TurretKilled,
        
        InhibitorKilled,
        
        DragonKilled,
        
        HeraldKilled,
        
        BaronKilled,
        
        FirstChampionKilled,
        
        ChampionKilled,
        
        MultipleChampionsKilled,
        
        EntireTeamKilled,
        
        GameEnded,
    }

    internal static class LoLClientEventTypeExtensions
    {
        private static Dictionary<LoLClientEventType, Func<LoLClientEvent>> EventFactory { get; } = new Dictionary<LoLClientEventType, Func<LoLClientEvent>>
                                                                                                    {
                                                                                                        {LoLClientEventType.GameStarted, () => new LoLClientGameStartedEvent()},
                                                                                                        {LoLClientEventType.FirstMinionWaveSpawned, () => new LoLClientFirstMinionWaveSpawnedEvent()},
                                                                                                        {LoLClientEventType.FirstTurretKilled, () => new LoLClientFirstTurretKilledEvent()},
                                                                                                        {LoLClientEventType.TurretKilled, () => new LoLClientTurretKilledEvent()},
                                                                                                        {LoLClientEventType.InhibitorKilled, () => new LoLClientInhibitorKilledEvent()},
                                                                                                        {LoLClientEventType.DragonKilled, () => new LoLClientDragonKilledEvent()},
                                                                                                        {LoLClientEventType.HeraldKilled, () => new LoLClientHeraldKilledEvent()},
                                                                                                        {LoLClientEventType.BaronKilled, () => new LoLClientBaronKilledEvent()},
                                                                                                        {LoLClientEventType.FirstChampionKilled, () => new LoLClientFirstChampionKilledEvent()},
                                                                                                        {LoLClientEventType.ChampionKilled, () => new LoLClientChampionKilledEvent()},
                                                                                                        {LoLClientEventType.MultipleChampionsKilled, () => new LoLClientMultipleChampionsKilledEvent()},
                                                                                                        {LoLClientEventType.EntireTeamKilled, () => new LoLClientEntireTeamKilledEvent()},
                                                                                                        {LoLClientEventType.GameEnded, () => new LoLClientGameEndedEvent()},
                                                                                                    };

        public static LoLClientEvent CreateEvent(this LoLClientEventType thisEventType)
        {
            if (!EventFactory.TryGetValue(thisEventType, out var factory))
            {
                throw new Exception($"Unknown {nameof(LoLClientEventType)} {thisEventType}!");
            }

            return factory.Invoke();
        }
    }
}