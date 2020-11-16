using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLMapTerrainConverter : EnumConverter<LoLMapTerrainType>
    {
        public LoLMapTerrainConverter() : base((LoLMapTerrainType.Undefined, "UNDEFINED"),
                                               (LoLMapTerrainType.Default, "DEFAULT"),
                                               (LoLMapTerrainType.Fire, "FIRE"),
                                               (LoLMapTerrainType.Water, "WATER"),
                                               (LoLMapTerrainType.Wind, "AIR"),
                                               (LoLMapTerrainType.Earth, "MOUNTAIN")) { }
    }
}