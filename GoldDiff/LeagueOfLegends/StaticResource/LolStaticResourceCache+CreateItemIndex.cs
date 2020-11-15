using System;
using System.IO;
using System.Linq;
using FlatXaml.Model;
using GoldDiff.View.Resource;
using GoldDiff.Shared.LeagueOfLegends;
using Newtonsoft.Json.Linq;

namespace GoldDiff.LeagueOfLegends.StaticResource
{
    public sealed partial class LoLStaticResourceCache
    {
        private void CreateItemIndex(Progression progression, LoLVersion gameVersion, string staticResourceRootDirectory)
        {
            progression.StartNextStep(LoLStaticResourceCacheResources.CreateItemIndexProgressStepDescription);
            Items.Clear();

            var itemCollectionFile = Path.Combine(staticResourceRootDirectory, gameVersion.ToString(), "data", "en_US", "item.json");
            var itemCollectionFileText = File.ReadAllText(itemCollectionFile);

            var itemCollection = JObject.Parse(itemCollectionFileText);
            var numberOfItems = itemCollection["data"]!.Children().Count();
            var currentItemIndex = 0;
            foreach (var itemJson in itemCollection["data"]!.Children().Cast<JProperty>())
            {
                progression.CurrentStepProgress = currentItemIndex++ / (double) numberOfItems;

                var item = ToItem(itemJson, gameVersion, staticResourceRootDirectory);
                if (!Items.TryAdd(item.Id, item))
                {
                    throw new Exception($"The item id {item.Id} has been defined multiple times!");
                }
            }
        }

        private LoLStaticItem ToItem(JProperty property, LoLVersion gameVersion, string staticResourceRootDirectory)
        {
            var id = int.Parse(property.Name);
            var token = property.First!;

            var name = token.Value<string>("name");
            var imageFileName = token.Value<JToken>("image")!.Value<string>("full");
            var smallTileImage = Path.Combine(staticResourceRootDirectory, gameVersion.ToString(), "img", "item", imageFileName);

            var gold = token.Value<JToken>("gold");
            var totalCosts = gold.Value<int>("total");
            var recipeCosts = gold.Value<int>("base");
            var sellWorth = gold.Value<int>("sell");

            return new LoLStaticItem(id, name, smallTileImage, totalCosts, recipeCosts, sellWorth);
        }
    }
}