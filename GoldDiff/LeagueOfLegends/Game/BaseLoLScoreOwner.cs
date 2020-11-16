using System;
using FlatXaml.Model;

namespace GoldDiff.LeagueOfLegends.Game
{
    public abstract class BaseLoLScoreOwner : ViewModel, ILoLScoreOwner
    {
        private int _kills;

        public int Kills
        {
            get => _kills;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (!MutateVerbose(ref _kills, value))
                {
                    return;
                }

                KillsSinceLastItemAcquisition = Kills - KillsAtLastItemAcquisition;
            }
        }

        private int _killsAtLastItemAcquisition;

        public int KillsAtLastItemAcquisition
        {
            get => _killsAtLastItemAcquisition;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (!MutateVerbose(ref _killsAtLastItemAcquisition, value))
                {
                    return;
                }

                KillsSinceLastItemAcquisition = Kills - KillsAtLastItemAcquisition;
            }
        }

        private int _killsSinceLastItemAcquisition;

        public int KillsSinceLastItemAcquisition
        {
            get => _killsSinceLastItemAcquisition;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _killsSinceLastItemAcquisition, value);
            }
        }

        private int _deaths;

        public int Deaths
        {
            get => _deaths;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (!MutateVerbose(ref _deaths, value))
                {
                    return;
                }

                DeathsSinceLastItemAcquisition = Deaths - DeathsAtLastItemAcquisition;
            }
        }

        private int _deathsAtLastItemAcquisition;

        public int DeathsAtLastItemAcquisition
        {
            get => _deathsAtLastItemAcquisition;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (!MutateVerbose(ref _deathsAtLastItemAcquisition, value))
                {
                    return;
                }

                DeathsSinceLastItemAcquisition = Deaths - DeathsAtLastItemAcquisition;
            }
        }

        private int _deathsSinceLastItemAcquisition;

        public int DeathsSinceLastItemAcquisition
        {
            get => _deathsSinceLastItemAcquisition;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _deathsSinceLastItemAcquisition, value);
            }
        }

        private int _assists;

        public int Assists
        {
            get => _assists;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (!MutateVerbose(ref _assists, value))
                {
                    return;
                }

                AssistsSinceLastItemAcquisition = Assists - AssistsAtLastItemAcquisition;
            }
        }

        private int _assistsAtLastItemAcquisition;

        public int AssistsAtLastItemAcquisition
        {
            get => _assistsAtLastItemAcquisition;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (!MutateVerbose(ref _assistsAtLastItemAcquisition, value))
                {
                    return;
                }

                AssistsSinceLastItemAcquisition = Assists - AssistsAtLastItemAcquisition;
            }
        }

        private int _assistsSinceLastItemAcquisition;

        public int AssistsSinceLastItemAcquisition
        {
            get => _assistsSinceLastItemAcquisition;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _assistsSinceLastItemAcquisition, value);
            }
        }

        private int _minionKills;

        public int MinionKills
        {
            get => _minionKills;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _minionKills, value);
            }
        }

        private double _vision;

        public double Vision
        {
            get => _vision;
            protected set
            {
                if (value < 0.0d)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                MutateVerbose(ref _vision, value);
            }
        }
    }
}