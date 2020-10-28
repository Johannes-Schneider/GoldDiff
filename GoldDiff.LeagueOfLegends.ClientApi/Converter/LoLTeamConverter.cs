using System;
using System.Reflection;
using GoldDiff.Shared.LeagueOfLegends;
using log4net;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLTeamConverter : ReadOnlyConverter<string>
    {
        private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                Log.Error($"Unable to convert {reader.Value} ({reader.ValueType?.Name}) to {nameof(LoLTeamType)}!");
                return LoLTeamType.Undefined;
            }

            if (value.Equals("ORDER", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLTeamType.BlueSide;
            }

            if (value.Equals("CHAOS", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLTeamType.RedSide;
            }

            return LoLTeamType.Undefined;
        }
    }
}