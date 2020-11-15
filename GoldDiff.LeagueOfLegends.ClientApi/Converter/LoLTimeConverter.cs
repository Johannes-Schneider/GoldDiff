using System;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLTimeConverter : JsonConverter<TimeSpan>
    {
        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            writer.WriteValue(value.TotalSeconds);
        }

        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return TimeSpan.FromSeconds(reader.Value as double? ?? 0.0d);
        }
    }
}