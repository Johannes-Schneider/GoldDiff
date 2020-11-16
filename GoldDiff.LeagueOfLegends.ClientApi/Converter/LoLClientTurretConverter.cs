using System;
using System.Collections.Generic;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.Utility;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientTurretConverter : JsonConverter<LoLClientTurret>
    {
        private const string StringPrefix = "Turret";
        private const string StringPostfix = "A";
        private const string TokenSeparator = "_";

        private static BidirectionalStringMapping<LoLTeamType> TeamMapping { get; } = new((LoLTeamType.Undefined, "UNDEFINED"),
                                                                                          (LoLTeamType.BlueSide, "T1"),
                                                                                          (LoLTeamType.RedSide, "T2"));

        private static Dictionary<LoLTeamType, BidirectionalStringMapping<LoLClientTurretTier>> TurretTierMapping { get; }

        static LoLClientTurretConverter()
        {
            var blueTiers = new BidirectionalStringMapping<LoLClientTurretTier>((LoLClientTurretTier.Undefined, "UNDEFINED"),
                                                                                (LoLClientTurretTier.TopOuter, $"L{TokenSeparator}03"),
                                                                                (LoLClientTurretTier.TopInner, $"L{TokenSeparator}02"),
                                                                                (LoLClientTurretTier.TopInhibitor, $"L{TokenSeparator}01"),
                                                                                (LoLClientTurretTier.MiddleOuter, $"C{TokenSeparator}05"),
                                                                                (LoLClientTurretTier.MiddleInner, $"C{TokenSeparator}04"),
                                                                                (LoLClientTurretTier.MiddleInhibitor, $"C{TokenSeparator}03"),
                                                                                (LoLClientTurretTier.MiddleNexusBottom, $"C{TokenSeparator}02"),
                                                                                (LoLClientTurretTier.MiddleNexusTop, $"C{TokenSeparator}01"),
                                                                                (LoLClientTurretTier.BottomOuter, $"R{TokenSeparator}03"),
                                                                                (LoLClientTurretTier.BottomInner, $"R{TokenSeparator}02"),
                                                                                (LoLClientTurretTier.BottomInhibitor, $"R{TokenSeparator}01"));

            var redTiers = new BidirectionalStringMapping<LoLClientTurretTier>((LoLClientTurretTier.Undefined, "UNDEFINED"),
                                                                               (LoLClientTurretTier.TopOuter, $"R{TokenSeparator}03"),
                                                                               (LoLClientTurretTier.TopInner, $"R{TokenSeparator}02"),
                                                                               (LoLClientTurretTier.TopInhibitor, $"R{TokenSeparator}01"),
                                                                               (LoLClientTurretTier.MiddleOuter, $"C{TokenSeparator}05"),
                                                                               (LoLClientTurretTier.MiddleInner, $"C{TokenSeparator}04"),
                                                                               (LoLClientTurretTier.MiddleInhibitor, $"C{TokenSeparator}03"),
                                                                               (LoLClientTurretTier.MiddleNexusTop, $"C{TokenSeparator}02"),
                                                                               (LoLClientTurretTier.MiddleNexusBottom, $"C{TokenSeparator}01"),
                                                                               (LoLClientTurretTier.BottomOuter, $"L{TokenSeparator}03"),
                                                                               (LoLClientTurretTier.BottomInner, $"L{TokenSeparator}02"),
                                                                               (LoLClientTurretTier.BottomInhibitor, $"L{TokenSeparator}01"));

            TurretTierMapping = new Dictionary<LoLTeamType, BidirectionalStringMapping<LoLClientTurretTier>>
                                {
                                    {LoLTeamType.BlueSide, blueTiers},
                                    {LoLTeamType.RedSide, redTiers},
                                    {LoLTeamType.Undefined, new BidirectionalStringMapping<LoLClientTurretTier>((LoLClientTurretTier.Undefined, "UNDEFINED"))},
                                };
        }

        public override void WriteJson(JsonWriter writer, LoLClientTurret value, JsonSerializer serializer)
        {
            writer.WriteValue(string.Join(TokenSeparator, StringPrefix, TeamMapping.Get(value.Team), TurretTierMapping[value.Team].Get(value.Tier), StringPostfix));
        }

        public override LoLClientTurret ReadJson(JsonReader reader, Type objectType, LoLClientTurret existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value as string;
            var tokens = value?.Split(TokenSeparator) ?? Array.Empty<string>();

            var team = tokens.Length == 5 ? TeamMapping.Get(tokens[1]) : LoLTeamType.Undefined;
            var tier = tokens.Length == 5 ? TurretTierMapping[team].Get(string.Join(TokenSeparator, tokens[2], tokens[3])) : LoLClientTurretTier.Undefined;
            return new LoLClientTurret
                   {
                       Team = team,
                       Tier = tier,
                   };
        }
    }
}