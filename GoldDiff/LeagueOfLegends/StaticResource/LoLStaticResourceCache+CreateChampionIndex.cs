using System;
using System.IO;
using System.Linq;
using GoldDiff.Properties;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.View.Controller;
using Newtonsoft.Json.Linq;

namespace GoldDiff.LeagueOfLegends.StaticResource
{
    public sealed partial class LoLStaticResourceCache
    {
        private void CreateChampionIndex(ProgressViewController progressViewController, LoLVersion gameVersion, string staticResourceRootDirectory)
        {
            progressViewController.StartNextStep(LoLStaticResourceCacheResources.CreateChampionIndexProgressStepDescription);

            var championCollectionFile = Path.Combine(staticResourceRootDirectory, gameVersion.ToString(), "data", "en_US", "champion.json");
            var championCollectionFileText = File.ReadAllText(championCollectionFile);

            var championCollection = JObject.Parse(championCollectionFileText);
            var numberOfChampions = championCollection["data"]!.Children().Count();
            var currentChampionIndex = 0;
            foreach (var championJson in championCollection["data"]!.Children())
            {
                progressViewController.Model.CurrentStepProgress = currentChampionIndex++ / (double) numberOfChampions;

                var champion = ToChampion(championJson.First!, gameVersion, staticResourceRootDirectory);
                if (!Champions.TryAdd(champion.Id, champion))
                {
                    throw new Exception($"The champion id {champion.Id} has been defined multiple times!");
                }
            }
        }

        private LoLStaticChampion ToChampion(JToken token, LoLVersion gameVersion, string staticResourceRootDirectory)
        {
            var name = token.Value<string>("name");
            var id = token.Value<int>("key");
            var imageFileName = token.Value<JToken>("image").Value<string>("full");

            var smallTileImage = Path.Combine(staticResourceRootDirectory, gameVersion.ToString(), "img", "champion", imageFileName);
            var largeTileImage = Path.Combine(staticResourceRootDirectory, "img", "champion", "tiles", $"{Path.GetFileNameWithoutExtension(imageFileName)}_0.jpg");
            
            return new LoLStaticChampion(id, name, smallTileImage, largeTileImage);
        }
    }
}