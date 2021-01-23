using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal class LoLGameModeConverter : EnumConverter<LoLGameModeType>
    {
        public LoLGameModeConverter() : base((LoLGameModeType.Undefined, "UNDEFINED"),
                                             (LoLGameModeType.Classic5X5, "CLASSIC"),
                                             (LoLGameModeType.PracticeTool, "PRACTICETOOL"),
                                             (LoLGameModeType.AllRandomAllMid, "ARAM")) { }
    }
}