using System;
using System.Diagnostics;
using System.Reflection;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using GoldDiff.Shared.LeagueOfLegends;
using log4net;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientInhibitorConverter : ReadOnlyConverter<string>
    {
        private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private static string[] TokenSeparator { get; } = {"_"};

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                Log.Error($"Unable to convert {reader.Value} to {nameof(LoLClientInhibitor)}!");
                return new LoLClientInhibitor
                       {
                           Team = LoLTeamType.Undefined,
                           Tier = LoLClientInhibitorTier.Undefined,
                       };
            }

            var tokens = value.Split(TokenSeparator, StringSplitOptions.None);
            if (tokens.Length != 3)
            {
                Log.Error($"Unable to convert {reader.Value} to {nameof(LoLClientInhibitor)}!");
                return new LoLClientInhibitor
                       {
                           Team = LoLTeamType.Undefined,
                           Tier = LoLClientInhibitorTier.Undefined,
                       };
            }

            var team = GetTeam(tokens[1]);
            var tier = GetTier(tokens[2]);
            return new LoLClientInhibitor
                   {
                       Team = team,
                       Tier = tier,
                   };
        }

        private LoLTeamType GetTeam(string token)
        {
            if (token.Equals("T1", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLTeamType.BlueSide;
            }

            if (token.Equals("T2", StringComparison.InvariantCultureIgnoreCase))
            {
                return LoLTeamType.RedSide;
            }

            return LoLTeamType.Undefined;
        }

        private LoLClientInhibitorTier GetTier(string token)
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

            return LoLClientInhibitorTier.Undefined;
        }
    }
}