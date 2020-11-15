using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLTeamConverter : EnumConverter<LoLTeamType>
    {
        public LoLTeamConverter() : base((LoLTeamType.Undefined, "UNDEFINED"), 
                                         (LoLTeamType.BlueSide, "ORDER"), 
                                         (LoLTeamType.RedSide, "CHAOS")) { }
    }
}