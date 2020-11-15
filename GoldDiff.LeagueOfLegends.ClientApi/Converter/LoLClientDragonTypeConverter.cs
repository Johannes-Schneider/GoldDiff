using GoldDiff.LeagueOfLegends.ClientApi.Event;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientDragonTypeConverter : EnumConverter<LoLClientDragonType>
    {
        public LoLClientDragonTypeConverter() : base((LoLClientDragonType.Undefined, "UNDEFINED"),
                                                     (LoLClientDragonType.Fire, "FIRE"),
                                                     (LoLClientDragonType.Water, "WATER"),
                                                     (LoLClientDragonType.Wind, "AIR"),
                                                     (LoLClientDragonType.Earth, "EARTH"),
                                                     (LoLClientDragonType.Elder, "ELDER")) { }
    }
}