using System;
using System.IO;
using GoldDiff.Shared.View.Model;
using Newtonsoft.Json;

namespace GoldDiff.View.Settings
{
    public class ViewSettings : ViewModel
    {
        private static string StorageLocation { get; } = Path.Combine(Environment.CurrentDirectory, "Config" , "ViewSettings.json");
        
        private static ViewSettings? _instance;

        public static ViewSettings Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                try
                {
                    _instance = JsonConvert.DeserializeObject<ViewSettings>(File.ReadAllText(StorageLocation));
                }
                catch
                {
                    _instance = new ViewSettings();
                }

                return _instance;
            }
        }
        
        [JsonProperty]
        public PlayerGoldDifferenceSettings PlayerGoldDifferenceSettings { get; private set; } = new PlayerGoldDifferenceSettings();

        private DisplayGoldType _displayGoldType = DisplayGoldType.NonConsumable;

        [JsonProperty]
        public DisplayGoldType DisplayGoldType
        {
            get => _displayGoldType;
            set => MutateVerbose(ref _displayGoldType, value);
        }

        private ViewSettings() { }

        public void Save()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(StorageLocation)!);
                File.WriteAllText(StorageLocation, JsonConvert.SerializeObject(this, Formatting.Indented));
            }
            catch (Exception exception)
            {
                // TODO: implement error handling
            }
        }
    }
}