using System;
using System.Reflection;
using GoldDiff.Shared.LeagueOfLegends;
using log4net;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal class LoLMapConverter : ReadOnlyConverter<int>
    {
        private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is int value))
            {
                Log.Error($"Unable to convert {reader.Value} ({reader.ValueType?.Name}) to {nameof(LoLMapType)}!");
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