using System;
using System.IO;
using System.Linq;
using GoldDiff.LeagueOfLegends.ClientApi;
using GoldDiff.LeagueOfLegends.ClientApi.Event;
using GoldDiff.Shared.LeagueOfLegends;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Test.GoldDiff.LeagueOfLegends.ClientApi
{
    [TestFixture]
    public class LoLClientGameDataTest
    {
        private static string FullGameAsJsonPath { get; } = Path.Combine(Environment.CurrentDirectory, "Resources", "full-game.json");
        private static string SpectatorGameAsJsonPath { get; } = Path.Combine(Environment.CurrentDirectory, "Resources", "spectator-game.json");
        
        [Test]
        public void TestDeserializeFullGame()
        {
            var gameData = JsonConvert.DeserializeObject<LoLClientGameData>(File.ReadAllText(FullGameAsJsonPath));
            
            Assert.AreEqual(236, gameData.EventCollection.Events.Count);
            Assert.AreEqual(LoLClientEventType.GameStarted, gameData.EventCollection.Events.First().EventType);
            Assert.AreEqual(LoLClientEventType.GameEnded, gameData.EventCollection.Events.Last().EventType);
            
            Assert.AreEqual(10, gameData.Players.Count);
            Assert.AreEqual(5, gameData.Players.Count(player => player.Team == LoLTeamType.BlueSide));
            Assert.AreEqual(5, gameData.Players.Count(player => player.Team == LoLTeamType.RedSide));
        }

        [Test]
        public void TestDeserializeSpectatorGame()
        {
            var gameData = JsonConvert.DeserializeObject<LoLClientGameData>(File.ReadAllText(SpectatorGameAsJsonPath));
            
            Assert.AreEqual(string.Empty, gameData.ActivePlayer.SummonerName);
        }
    }
}