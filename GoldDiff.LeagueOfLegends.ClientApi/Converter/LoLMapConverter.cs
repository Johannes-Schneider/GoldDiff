using System;
using GoldDiff.Shared.LeagueOfLegends;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal class LoLMapConverter : ReadOnlyConverter<int>
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is int value))
            {
                return LoLMapType.Undefined;
            }

            return value switch
                   {
                       11 => LoLMapType.SummonersRift,
                       _ => LoLMapType.Undefined,
                   };
        }
    }
}