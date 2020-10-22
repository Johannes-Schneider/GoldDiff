using System;
using GoldDiff.Shared.LeagueOfLegends;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal class LoLGameModeConverter : ReadOnlyConverter<string>
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                return LoLGameModeType.Undefined;
            }

            if (value.Equals("CLASSIC", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLGameModeType.Classic5X5;
            }

            return LoLGameModeType.Undefined;
        }
    }
}