using System;
using System.Reflection;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using log4net;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientEventTypeConverter : ReadOnlyConverter<string>
    {
        private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                Log.Error($"Unable to convert {reader.Value} ({reader.ValueType?.Name}) to {nameof(LoLClientEventType)}!");
                return LoLClientEventType.Undefined;
            }

            if (value.Equals("GameStart", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.GameStarted;
            }
            
            if (value.Equals("MinionsSpawning", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.FirstMinionWaveSpawned;
            }
            
            if (value.Equals("FirstBrick", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.FirstTurretKilled;
            }
            
            if (value.Equals("TurretKilled", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.TurretKilled;
            }
            
            if (value.Equals("InhibKilled", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.InhibitorKilled;
            }
            
            if (value.Equals("InhibRespawningSoon", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.InhibitorRespawningSoon;
            }
            
            if (value.Equals("InhibRespawned", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.InhibitorRespawned;
            }
            
            if (value.Equals("DragonKill", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.DragonKilled;
            }
            
            if (value.Equals("HeraldKill", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.HeraldKilled;
            }
            
            if (value.Equals("BaronKill", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.BaronKilled;
            }
            
            if (value.Equals("FirstBlood", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.FirstChampionKilled;
            }
            
            if (value.Equals("ChampionKill", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.ChampionKilled;
            }
            
            if (value.Equals("MultiKill", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.MultipleChampionsKilled;
            }
            
            if (value.Equals("Ace", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.EntireTeamKilled;
            }
            
            if (value.Equals("GameEnd", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientEventType.GameEnded;
            }

            Log.Error($"Unable to convert {value} to {nameof(LoLClientEventType)}!");
            return LoLClientEventType.Undefined;
        }
    }
}