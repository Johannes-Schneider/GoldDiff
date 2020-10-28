using System;
using System.Reflection;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using log4net;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientGameResultConverter : ReadOnlyConverter<string>
    {
        private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                Log.Error($"Unable to convert {reader.Value} ({reader.ValueType?.Name}) to {nameof(LoLClientGameResult)}!");
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

            Log.Error($"Unable to convert {value} to {nameof(LoLClientGameResult)}!");
            return LoLClientGameResult.Undefined;
        }
    }
}