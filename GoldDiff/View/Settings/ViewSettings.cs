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

    #region GoldDifferenceWindow settings

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

    #endregion

    #region ILoLGoldOwner settings

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