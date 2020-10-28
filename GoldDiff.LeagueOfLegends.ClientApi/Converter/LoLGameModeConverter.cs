using System;
using System.Reflection;
using GoldDiff.Shared.LeagueOfLegends;
using log4net;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal class LoLGameModeConverter : ReadOnlyConverter<string>
    {
        private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                Log.Error($"Unable to convert {reader.Value} ({reader.ValueType?.Name}) to {nameof(LoLGameModeType)}!");
                return LoLGameModeType.Undefined;
            }

            if (value.Equals("CLASSIC", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLGameModeType.Classic5X5;
            }

            Log.Error($"Unable to convert {value} to {nameof(LoLGameModeType)}!");
            return LoLGameModeType.Undefined;
        }
    }
}