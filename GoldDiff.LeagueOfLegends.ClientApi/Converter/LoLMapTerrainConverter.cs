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
                return LoLMapTerrain.Undefined;
            }

            if (value.Equals("DEFAULT", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLMapTerrain.Default;
            }

            if (value.Equals("FIRE", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLMapTerrain.Fire;
            }

            if (value.Equals("WATER", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLMapTerrain.Water;
            }

            if (value.Equals("AIR", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLMapTerrain.Wind;
            }

            if (value.Equals("MOUNTAIN", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLMapTerrain.Earth;
            }

            return LoLMapTerrain.Undefined;
        }
    }
}