using System;
using GoldDiff.Shared.LeagueOfLegends;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLTeamConverter : ReadOnlyConverter<string>
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
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