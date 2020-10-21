using System;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLTimeConverter : ReadOnlyConverter<double>
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value is double value)
            {
                return TimeSpan.FromSeconds(value);
            }
            
            throw new Exception($"Unable to convert {reader.Value} to {nameof(TimeSpan)}!");
        }
    }
}