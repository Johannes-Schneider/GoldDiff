using System;
using System.IO;
using System.Security.AccessControl;
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

    #region common window settings

        private StayOnTopType _windowStayOnTop = StayOnTopType.WhileGameIsRunning;

        [JsonProperty]
        public StayOnTopType WindowStayOnTop
        {
            get => _windowStayOnTop;
            set => MutateVerbose(ref _windowStayOnTop, value);
        }

        private DisplayTitleBarType _windowDisplayTitleBar = DisplayTitleBarType.Always;

        [JsonProperty]
        public DisplayTitleBarType WindowDisplayTitleBar
        {
            get => _windowDisplayTitleBar;
            set => MutateVerbose(ref _windowDisplayTitleBar, value);
        }

    #endregion

    #region GoldDifferenceWindow settings

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

    #endregion

    #region GoldChartWindow settings

        private int _goldChartWindowLeft;

        [JsonProperty]
        public int GoldChartWindowLeft
        {
            get => _goldChartWindowLeft;
            set => MutateVerbose(ref _goldChartWindowLeft, value);
        }

        private int _goldChartWindowTop;

        [JsonProperty]
        public int GoldChartWindowTop
        {
            get => _goldChartWindowTop;
            set => MutateVerbose(ref _goldChartWindowTop, value);
        }

        private int _goldChartWindowWidth;

        [JsonProperty]
        public int GoldChartWindowWidth
        {
            get => _goldChartWindowWidth;
            set => MutateVerbose(ref _goldChartWindowWidth, value);
        }

        private int _goldChartWindowHeight;

        [JsonProperty]
        public int GoldChartWindowHeight
        {
            get => _goldChartWindowHeight;
            set => MutateVerbose(ref _goldChartWindowHeight, value);
        }

    #endregion

    #region ILoLGoldOwner settings
        
        private DisplayGoldType _displayGoldType = DisplayGoldType.NonConsumable;

        [JsonProperty]
        public DisplayGoldType DisplayGoldType
        {
            get => _displayGoldType;
            set => MutateVerbose(ref _displayGoldType, value);
        }

    #endregion

    #region LoLPlayer settings

        private bool _displayPlayerScores = true;

        [JsonProperty]
        public bool DisplayPlayerScores
        {
            get => _displayPlayerScores;
            set => MutateVerbose(ref _displayPlayerScores, value);
        }

        private bool _displayPlayerScoresSinceLastItemAcquisition = true;

        [JsonProperty]
        public bool DisplayPlayerScoresSinceLastItemAcquisition
        {
            get => _displayPlayerScoresSinceLastItemAcquisition;
            set => MutateVerbose(ref _displayPlayerScoresSinceLastItemAcquisition, value);
        }
        
    #endregion

    #region LoLTeam settings

        private bool _displayTeamScores = true;

        [JsonProperty]
        public bool DisplayTeamScores
        {
            get => _displayTeamScores;
            set => MutateVerbose(ref _displayTeamScores, value);
        }

        private bool _displayTeamScoresSinceLastItemAcquisition = true;

        [JsonProperty]
        public bool DisplayTeamScoresSinceLastItemAcquisition
        {
            get => _displayTeamScoresSinceLastItemAcquisition;
            set => MutateVerbose(ref _displayTeamScoresSinceLastItemAcquisition, value);
        }

        private bool _displayInhibitorRespawnTimers = true;

        [JsonProperty]
        public bool DisplayInhibitorRespawnTimers
        {
            get => _displayInhibitorRespawnTimers;
            set => MutateVerbose(ref _displayInhibitorRespawnTimers, value);
        }

    #endregion


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