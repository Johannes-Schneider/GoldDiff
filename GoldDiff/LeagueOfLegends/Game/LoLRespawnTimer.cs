using System;
using FlatXaml.Model;

namespace GoldDiff.LeagueOfLegends.Game
{
    public class LoLRespawnTimer : ViewModel
    {
        private TimeSpan? _time;

        public TimeSpan? Time
        {
            get => _time;
            set
            {
                if (!MutateVerbose(ref _time, value))
                {
                    return;
                }

                IsActive = value != null;
            }
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            private set => MutateVerbose(ref _isActive, value);
        }
    }
}