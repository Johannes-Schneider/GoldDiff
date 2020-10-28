using System;
using System.Reflection;
using GoldDiff.Shared.LeagueOfLegends;
using log4net;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLPositionConverter : ReadOnlyConverter<string>
    {
        private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                Log.Error($"Unable to convert {reader.Value} ({reader.ValueType?.Name}) to {nameof(LoLPositionType)}!");
                return LoLPositionType.Undefined;
            }
            
            if (value.Equals("TOP", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLPositionType.Top;
            }

            if (value.Equals("JUNGLE", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLPositionType.Jungle;
            }

            if (value.Equals("MIDDLE", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLPositionType.Middle;
            }

            if (value.Equals("BOTTOM", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLPositionType.Bottom;
            }

            if (value.Equals("UTILITY", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLPositionType.Support;
            }
            
            return LoLPositionType.Undefined;
        }
    }
}