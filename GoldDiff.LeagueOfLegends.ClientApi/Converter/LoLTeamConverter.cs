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
                return LoLTeam.Undefined;
            }

            if (value.Equals("ORDER", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLTeam.BlueSide;
            }

            if (value.Equals("CHAOS", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLTeam.RedSide;
            }

            return LoLTeam.Undefined;
        }
    }
}