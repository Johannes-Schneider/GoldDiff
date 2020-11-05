using System;
using System.Reflection;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using GoldDiff.Shared.LeagueOfLegends;
using log4net;
using Newtonsoft.Json;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientTurretConverter : ReadOnlyConverter<string>
    {
        private static ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static string[] TokenSeparator { get; } = {"_"};

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (!(reader.Value is string value))
            {
                Log.Error($"Unable to convert {reader.Value} to {nameof(LoLClientTurret)}!");
                return new LoLClientTurret
                       {
                           Team = LoLTeamType.Undefined,
                           Tier = LoLClientTurretTier.Undefined,
                       };
            }

            var tokens = value.Split(TokenSeparator, StringSplitOptions.None);
            if (tokens.Length != 5)
            {
                Log.Error($"Unable to convert {reader.Value} to {nameof(LoLClientTurret)}!");
                return new LoLClientTurret
                       {
                           Team = LoLTeamType.Undefined,
                           Tier = LoLClientTurretTier.Undefined,
                       };
            }

            var team = GetTeam(tokens[1]);
            var tier = GetTurretTier(tokens[2], tokens[3], team);

            return new LoLClientTurret
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

        private LoLClientTurretTier GetTurretTier(string relativeLane, string tier, LoLTeamType team)
        {
            switch (team)
            {
                case LoLTeamType.BlueSide when relativeLane.Equals("L", StringComparison.InvariantCultureIgnoreCase):
                {
                    if (tier.Equals("03", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.TopOuter;
                    }

                    if (tier.Equals("02", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.TopInner;
                    }

                    if (tier.Equals("01", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.TopInhibitor;
                    }

                    break;
                }
                case LoLTeamType.BlueSide when relativeLane.Equals("R", StringComparison.InvariantCultureIgnoreCase):
                {
                    if (tier.Equals("03", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.BottomOuter;
                    }

                    if (tier.Equals("02", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.BottomInner;
                    }

                    if (tier.Equals("01", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.BottomInhibitor;
                    }

                    break;
                }
                case LoLTeamType.BlueSide when relativeLane.Equals("C", StringComparison.InvariantCultureIgnoreCase):
                {
                    if (tier.Equals("05", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.MiddleOuter;
                    }

                    if (tier.Equals("04", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.MiddleInner;
                    }

                    if (tier.Equals("03", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.MiddleInhibitor;
                    }

                    if (tier.Equals("02", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.MiddleNexusBottom;
                    }

                    if (tier.Equals("01", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.MiddleNexusTop;
                    }

                    break;
                }
                case LoLTeamType.RedSide when relativeLane.Equals("R", StringComparison.InvariantCultureIgnoreCase):
                {
                    if (tier.Equals("03", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.TopOuter;
                    }

                    if (tier.Equals("02", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.TopInner;
                    }

                    if (tier.Equals("01", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.TopInhibitor;
                    }

                    break;
                }
                case LoLTeamType.RedSide when relativeLane.Equals("L", StringComparison.InvariantCultureIgnoreCase):
                {
                    if (tier.Equals("03", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.BottomOuter;
                    }

                    if (tier.Equals("02", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.BottomInner;
                    }

                    if (tier.Equals("01", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.BottomInhibitor;
                    }

                    break;
                }
                case LoLTeamType.RedSide when relativeLane.Equals("C", StringComparison.InvariantCultureIgnoreCase):
                {
                    if (tier.Equals("05", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.MiddleOuter;
                    }

                    if (tier.Equals("04", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.MiddleInner;
                    }

                    if (tier.Equals("03", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.MiddleInhibitor;
                    }

                    if (tier.Equals("02", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.MiddleNexusTop;
                    }

                    if (tier.Equals("01", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return LoLClientTurretTier.MiddleNexusBottom;
                    }

                    break;
                }
            }

            return LoLClientTurretTier.Undefined;
        }
    }
}