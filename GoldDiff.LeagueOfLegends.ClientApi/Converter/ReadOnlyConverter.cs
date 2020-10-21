using System;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    public abstract class ReadOnlyConverter<TParameterType> : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new InvalidOperationException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TParameterType);
        }
    }
}