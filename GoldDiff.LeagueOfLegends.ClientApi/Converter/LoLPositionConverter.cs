using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLPositionConverter : EnumConverter<LoLPositionType>
    {
        public LoLPositionConverter() : base((LoLPositionType.Undefined, "UNDEFINED"),
                                             (LoLPositionType.Top, "TOP"),
                                             (LoLPositionType.Jungle, "JUNGLE"),
                                             (LoLPositionType.Middle, "MIDDLE"),
                                             (LoLPositionType.Bottom, "BOTTOM"),
                                             (LoLPositionType.Support, "UTILITY")) { }
    }
}