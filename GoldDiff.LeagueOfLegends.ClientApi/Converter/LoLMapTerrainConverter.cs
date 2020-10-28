using System;
using System.Reflection;
using GoldDiff.Shared.LeagueOfLegends;
using log4net;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLMapTerrainConverter : ReadOnlyConverter<string>
    {
        private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                Log.Error($"Unable to convert {reader.Value} ({reader.ValueType?.Name}) to {nameof(LoLMapTerrainType)}!");
                return LoLMapTerrainType.Undefined;
            }

            if (value.Equals("DEFAULT", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLMapTerrainType.Default;
            }

            if (value.Equals("FIRE", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLMapTerrainType.Fire;
            }

            if (value.Equals("WATER", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLMapTerrainType.Water;
            }

            if (value.Equals("AIR", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLMapTerrainType.Wind;
            }

            if (value.Equals("MOUNTAIN", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLMapTerrainType.Earth;
            }

            Log.Error($"Unable to convert {value} to {nameof(LoLMapTerrainType)}!");
            return LoLMapTerrainType.Undefined;
        }
    }
}