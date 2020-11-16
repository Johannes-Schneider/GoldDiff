using GoldDiff.LeagueOfLegends.ClientApi.Event;

namespace GoldDiff.LeagueOfLegends.ClientApi.Converter
{
    internal sealed class LoLClientEventTypeConverter : EnumConverter<LoLClientEventType>
    {
        public LoLClientEventTypeConverter() : base((LoLClientEventType.Undefined, "UNDEFINED"),
                                                    (LoLClientEventType.GameStarted, "GameStart"),
                                                    (LoLClientEventType.FirstMinionWaveSpawned, "MinionsSpawning"),
                                                    (LoLClientEventType.FirstTurretKilled, "FirstBrick"),
                                                    (LoLClientEventType.TurretKilled, "TurretKilled"),
                                                    (LoLClientEventType.InhibitorKilled, "InhibKilled"),
                                                    (LoLClientEventType.InhibitorRespawningSoon, "InhibRespawningSoon"),
                                                    (LoLClientEventType.InhibitorRespawned, "InhibRespawned"),
                                                    (LoLClientEventType.DragonKilled, "DragonKill"),
                                                    (LoLClientEventType.HeraldKilled, "HeraldKill"),
                                                    (LoLClientEventType.BaronKilled, "BaronKill"),
                                                    (LoLClientEventType.FirstChampionKilled, "FirstBlood"),
                                                    (LoLClientEventType.ChampionKilled, "ChampionKill"),
                                                    (LoLClientEventType.MultipleChampionsKilled, "MultiKill"),
                                                    (LoLClientEventType.EntireTeamKilled, "Ace"),
                                                    (LoLClientEventType.GameEnded, "GameEnd")) { }
    }
}