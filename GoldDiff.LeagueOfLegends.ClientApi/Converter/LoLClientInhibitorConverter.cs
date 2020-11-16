using System;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.Utility;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientInhibitorConverter : JsonConverter<LoLClientInhibitor>
    {
        private const string StringPrefix = "Barracks";
        private const string TokenSeparator = "_";

        private static BidirectionalStringMapping<LoLTeamType> TeamMapping { get; } = new((LoLTeamType.Undefined, "UNDEFINED"),
                                                                                          (LoLTeamType.BlueSide, "T1"),
                                                                                          (LoLTeamType.RedSide, "T2"));

        private static BidirectionalStringMapping<LoLClientInhibitorTier> TierMapping { get; } = new((LoLClientInhibitorTier.Undefined, "UNDEFINED"),
                                                                                                     (LoLClientInhibitorTier.Top, "L1"),
                                                                                                     (LoLClientInhibitorTier.Middle, "C1"),
                                                                                                     (LoLClientInhibitorTier.Bottom, "R1"));
        public override void WriteJson(JsonWriter writer, LoLClientInhibitor value, JsonSerializer serializer)
        {
            writer.WriteValue(string.Join(TokenSeparator, StringPrefix, TeamMapping.Get(value.Team), TierMapping.Get(value.Tier)));
        }

        public override LoLClientInhibitor ReadJson(JsonReader reader, Type objectType, LoLClientInhibitor existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value as string;
            var tokens = value?.Split(TokenSeparator) ?? Array.Empty<string>();
            var team = tokens.Length == 3 ? TeamMapping.Get(tokens[1]) : LoLTeamType.Undefined;
            var tier = tokens.Length == 3 ? TierMapping.Get(tokens[2]) : LoLClientInhibitorTier.Undefined;
            return new LoLClientInhibitor
                   {
                       Team = team,
                       Tier = tier,
                   };
        }
    }
}