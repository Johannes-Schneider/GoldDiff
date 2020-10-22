using System;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientDragonTypeConverter : ReadOnlyConverter<string>
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                return LoLClientDragonType.Undefined;
            }

            if (value.Equals("Fire", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientDragonType.Fire;
            }
            
            if (value.Equals("Water", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientDragonType.Water;
            }
            
            if (value.Equals("Air", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientDragonType.Wind;
            }
            
            if (value.Equals("Earth", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientDragonType.Earth;
            }
            
            if (value.Equals("Elder", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientDragonType.Elder;
            }

            return LoLClientDragonType.Undefined;
        }
    }
}