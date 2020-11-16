using System;
using GoldDiff.Shared.LeagueOfLegends;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal class LoLMapConverter : JsonConverter<LoLMapType>
    {
        public override void WriteJson(JsonWriter writer, LoLMapType value, JsonSerializer serializer)
        {
            writer.WriteValue(value switch
                              {
                                  LoLMapType.SummonersRift => 11,
                                  _ => 0,
                              });
        }

        public override LoLMapType ReadJson(JsonReader reader, Type objectType, LoLMapType existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return reader.Value switch
                   {
                       11 => LoLMapType.SummonersRift,
                       _ => LoLMapType.Undefined,
                   };
        }
    }
}