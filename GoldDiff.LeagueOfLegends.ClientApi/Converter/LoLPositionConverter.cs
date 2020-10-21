using System;
using GoldDiff.Shared.LeagueOfLegends;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLPositionConverter : ReadOnlyConverter<string>
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                return LoLPosition.Undefined;
            }
            
            if (value.Equals("TOP", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLPosition.Top;
            }

            if (value.Equals("JUNGLE", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLPosition.Jungle;
            }

            if (value.Equals("MIDDLE", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLPosition.Middle;
            }

            if (value.Equals("BOTTOM", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLPosition.Bottom;
            }

            if (value.Equals("UTILITY", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLPosition.Support;
            }
            
            return LoLPosition.Undefined;
        }
    }
}