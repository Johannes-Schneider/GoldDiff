using GoldDiff.LeagueOfLegends.ClientApi.Event;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientGameResultConverter : EnumConverter<LoLClientGameResult>
    {
        public LoLClientGameResultConverter() : base((LoLClientGameResult.Undefined, "UNDEFINED"),
                                                     (LoLClientGameResult.Win, "WIN"),
                                                     (LoLClientGameResult.Lose, "LOSE")) { }
    }
}