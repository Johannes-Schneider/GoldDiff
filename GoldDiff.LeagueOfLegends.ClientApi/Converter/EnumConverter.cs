using System;
using GoldDiff.Shared.Utility;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal class EnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : Enum
    {
        public BidirectionalStringMapping<TEnum> Mapping { get; }

        public EnumConverter(ValueTuple<TEnum, string> errorValue, params ValueTuple<TEnum, string>[] validValues)
        {
            Mapping = new BidirectionalStringMapping<TEnum>(errorValue, validValues);
        }

        public override void WriteJson(JsonWriter writer, TEnum value, JsonSerializer serializer)
        {
            writer.WriteValue(Mapping.Get(value));
        }

        public override TEnum ReadJson(JsonReader reader, Type objectType, TEnum existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return Mapping.Get(reader.Value as string);
        }
    }
}