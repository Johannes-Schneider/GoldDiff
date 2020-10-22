using System;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using GoldDiff.Shared.LeagueOfLegends;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientInhibitorConverter : ReadOnlyConverter<string>
    {
        private static string[] TokenSeparator { get; } = {"_"};

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                throw ConversionFailed(reader.Value);
            }

            var tokens = value.Split(TokenSeparator, StringSplitOptions.None);
            if (tokens.Length != 3)
            {
                throw ConversionFailed(reader.Value);
            }

            var team = GetTeam(reader.Value, tokens[1]);
            var tier = GetTier(reader.Value, tokens[2]);
            return new LoLClientInhibitor
                   {
                       Team = team,
                       Tier = tier,
                   };
        }

        private LoLTeamType GetTeam(object? readerValue, string token)
        {
            if (token.Equals("T1", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLTeamType.BlueSide;
            }

            if (token.Equals("T2", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLTeamType.RedSide;
            }

            throw ConversionFailed(readerValue);
        }

        private LoLClientInhibitorTier GetTier(object? readerValue, string token)
        {
            if (token.Equals("L1", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientInhibitorTier.Top;
            }

            if (token.Equals("C1", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLClientInhibitorTier.Middle;
            }

            if (token.Equals("R1", StringComparison.CurrentCultureIgnoreCase))
            {
                return LoLClientInhibitorTier.Bottom;
            }

            throw ConversionFailed(readerValue);
        }

        private Exception ConversionFailed(object? readerValue)
        {
            return new Exception($"Unable to convert {readerValue} tp {nameof(LoLClientInhibitor)}!");
        }
    }
}