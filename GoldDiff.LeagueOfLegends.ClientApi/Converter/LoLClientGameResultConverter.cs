using System;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientGameResultConverter : ReadOnlyConverter<string>
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                return LoLClientGameResult.Undefined;
            }

            if (value.Equals("Win", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientGameResult.Win;
            }
            
            if (value.Equals("Lose", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientGameResult.Lose;
            }

            return LoLClientGameResult.Undefined;
        }
    }
}