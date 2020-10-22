using System;
using GoldDiff.Shared.LeagueOfLegends;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLMapTerrainConverter : ReadOnlyConverter<string>
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
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

            return LoLMapTerrainType.Undefined;
        }
    }
}