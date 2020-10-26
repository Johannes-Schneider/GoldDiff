using System.Collections.Generic;
using System.Windows.Media;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.View.Model;

namespace GoldDiff.View.Model
{
    public class GoldDifferenceWindowViewModel : ViewModel
    {
        private Dictionary<LoLTeamType, Dictionary<LoLPositionType, LoLPlayer?>> Players { get; } = new Dictionary<LoLTeamType, Dictionary<LoLPositionType, LoLPlayer?>>
                                                                                                    {
                                                                                                        {
                                                                                                            LoLTeamType.BlueSide, new Dictionary<LoLPositionType, LoLPlayer?>
                                                                                                                                  {
                                                                                                                                      {LoLPositionType.Top, null},
                                                                                                                                      {LoLPositionType.Jungle, null},
                                                                                                                                      {LoLPositionType.Middle, null},
                                                                                                                                      {LoLPositionType.Bottom, null},
                                                                                                                                      {LoLPositionType.Support, null},
                                                                                                                                  }
                                                                                                        },
                                                                                                        {
                                                                                                            LoLTeamType.RedSide, new Dictionary<LoLPositionType, LoLPlayer?>
                                                                                                                                 {
                                                                                                                                     {LoLPositionType.Top, null},
                                                                                                                                     {LoLPositionType.Jungle, null},
                                                                                                                                     {LoLPositionType.Middle, null},
                                                                                                                                     {LoLPositionType.Bottom, null},
                                                                                                                                     {LoLPositionType.Support, null},
                                                                                                                                 }
                                                                                                        },
                                                                                                    };
        
    #region Blue Side
        
        public LoLPlayer? TopPlayerBlueSide
        {
            get => Players[LoLTeamType.BlueSide][LoLPositionType.Top];
            set
            {
                Players[LoLTeamType.BlueSide][LoLPositionType.Top] = value;
                OnPropertyChanged();
            }
        }
        
        public LoLPlayer? MiddlePlayerBlueSide
        {
            get => Players[LoLTeamType.BlueSide][LoLPositionType.Middle];
            set
            {
                Players[LoLTeamType.BlueSide][LoLPositionType.Middle] = value;
                OnPropertyChanged();
            }
        }
        
        public LoLPlayer? BottomPlayerBlueSide
        {
            get => Players[LoLTeamType.BlueSide][LoLPositionType.Bottom];
            set
            {
                Players[LoLTeamType.BlueSide][LoLPositionType.Bottom] = value;
                OnPropertyChanged();
            }
        }
        
        public LoLPlayer? JunglePlayerBlueSide
        {
            get => Players[LoLTeamType.BlueSide][LoLPositionType.Jungle];
            set
            {
                Players[LoLTeamType.BlueSide][LoLPositionType.Jungle] = value;
                OnPropertyChanged();
            }
        }
        
        public LoLPlayer? SupportPlayerBlueSide
        {
            get => Players[LoLTeamType.BlueSide][LoLPositionType.Support];
            set
            {
                Players[LoLTeamType.BlueSide][LoLPositionType.Support] = value;
                OnPropertyChanged();
            }
        }

    #endregion
        
    #region Red Side

        public LoLPlayer? TopPlayerRedSide
        {
            get => Players[LoLTeamType.RedSide][LoLPositionType.Top];
            set
            {
                Players[LoLTeamType.RedSide][LoLPositionType.Top] = value;
                OnPropertyChanged();
            }
        }
        
        public LoLPlayer? MiddlePlayerRedSide
        {
            get => Players[LoLTeamType.RedSide][LoLPositionType.Middle];
            set
            {
                Players[LoLTeamType.RedSide][LoLPositionType.Middle] = value;
                OnPropertyChanged();
            }
        }
        
        public LoLPlayer? BottomPlayerRedSide
        {
            get => Players[LoLTeamType.RedSide][LoLPositionType.Bottom];
            set
            {
                Players[LoLTeamType.RedSide][LoLPositionType.Bottom] = value;
                OnPropertyChanged();
            }
        }
        
        public LoLPlayer? JunglePlayerRedSide
        {
            get => Players[LoLTeamType.RedSide][LoLPositionType.Jungle];
            set
            {
                Players[LoLTeamType.RedSide][LoLPositionType.Jungle] = value;
                OnPropertyChanged();
            }
        }
        
        public LoLPlayer? SupportPlayerRedSide
        {
            get => Players[LoLTeamType.RedSide][LoLPositionType.Support];
            set
            {
                Players[LoLTeamType.RedSide][LoLPositionType.Support] = value;
                OnPropertyChanged();
            }
        }

    #endregion

        private Brush? _activePlayerOnBlueSideBackground;

        public Brush? ActivePlayerOnBlueSideBackground
        {
            get => _activePlayerOnBlueSideBackground;
            set => MutateVerbose(ref _activePlayerOnBlueSideBackground, value);
        }
        
        private Brush? _activePlayerOnRedSideBackground;

        public Brush? ActivePlayerOnRedSideBackground
        {
            get => _activePlayerOnRedSideBackground;
            set => MutateVerbose(ref _activePlayerOnRedSideBackground, value);
        }

        private Brush? _inactivePlayerBackground;

        public Brush? InactivePlayerBackground
        {
            get => _inactivePlayerBackground;
            set => MutateVerbose(ref _inactivePlayerBackground, value);
        }

        private Brush? _topPlayerBackground;

        public Brush? TopPlayerBackground
        {
            get => _topPlayerBackground;
            set => MutateVerbose(ref _topPlayerBackground, value);
        }
        
        private Brush? _junglePlayerBackground;

        public Brush? JunglePlayerBackground
        {
            get => _junglePlayerBackground;
            set => MutateVerbose(ref _junglePlayerBackground, value);
        }
        
        private Brush? _middlePlayerBackground;

        public Brush? MiddlePlayerBackground
        {
            get => _middlePlayerBackground;
            set => MutateVerbose(ref _middlePlayerBackground, value);
        }
        
        private Brush? _bottomPlayerBackground;

        public Brush? BottomPlayerBackground
        {
            get => _bottomPlayerBackground;
            set => MutateVerbose(ref _bottomPlayerBackground, value);
        }
        
        private Brush? _supportPlayerBackground;

        public Brush? SupportPlayerBackground
        {
            get => _supportPlayerBackground;
            set => MutateVerbose(ref _supportPlayerBackground, value);
        }

        public void SwapPlayers(LoLTeamType team, LoLPositionType positionA, LoLPositionType positionB)
        {
            (Players[team][positionA], Players[team][positionB]) = (Players[team][positionB], Players[team][positionA]);
            OnPropertyChanged(string.Empty);
        }
    }
}