using System;
using System.IO;
using FlatXaml.Model;
using Newtonsoft.Json;

namespace GoldDiff.View.Settings
{
    public class ViewSettings : ViewModel
    {
        private static string StorageLocation { get; } = Path.Combine(Environment.CurrentDirectory, "Config", "ViewSettings.json");

    #region Singleton

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

        private ViewSettings() { }

    #endregion

        private StayOnTopType _goldDifferenceWindowStayOnTop = StayOnTopType.DuringGame;

        [JsonProperty]
        public StayOnTopType GoldDifferenceWindowStayOnTop
        {
            get => _goldDifferenceWindowStayOnTop;
            set => MutateVerbose(ref _goldDifferenceWindowStayOnTop, value);
        }

        private int _goldDifferenceWindowLeft;

        [JsonProperty]
        public int GoldDifferenceWindowLeft
        {
            get => _goldDifferenceWindowLeft;
            set => MutateVerbose(ref _goldDifferenceWindowLeft, value);
        }

        private int _goldDifferenceWindowTop;

        [JsonProperty]
        public int GoldDifferenceWindowTop
        {
            get => _goldDifferenceWindowTop;
            set => MutateVerbose(ref _goldDifferenceWindowTop, value);
        }

        private int _goldDifferenceWindowWidth;

        [JsonProperty]
        public int GoldDifferenceWindowWidth
        {
            get => _goldDifferenceWindowWidth;
            set => MutateVerbose(ref _goldDifferenceWindowWidth, value);
        }

        private int _goldDifferenceWindowHeight;

        [JsonProperty]
        public int GoldDifferenceWindowHeight
        {
            get => _goldDifferenceWindowHeight;
            set => MutateVerbose(ref _goldDifferenceWindowHeight, value);
        }

        private DisplayGoldType _displayGoldType = DisplayGoldType.NonConsumable;

        [JsonProperty]
        public DisplayGoldType DisplayGoldType
        {
            get => _displayGoldType;
            set => MutateVerbose(ref _displayGoldType, value);
        }

        private bool _displayPlayerStats = true;

        [JsonProperty]
        public bool DisplayPlayerStats
        {
            get => _displayPlayerStats;
            set => MutateVerbose(ref _displayPlayerStats, value);
        }

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