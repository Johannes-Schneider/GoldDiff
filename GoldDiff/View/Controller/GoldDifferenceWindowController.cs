using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.View.Model;
using GoldDiff.View.Settings;

namespace GoldDiff.View.Controller
{
    public class GoldDifferenceWindowController
    {
        private static List<string> PlayerPropertyNames { get; } = new List<string>
                                                                   {
                                                                       nameof(GoldDifferenceWindowViewModel.TopPlayerBlueSide),
                                                                       nameof(GoldDifferenceWindowViewModel.JunglePlayerBlueSide),
                                                                       nameof(GoldDifferenceWindowViewModel.MiddlePlayerBlueSide),
                                                                       nameof(GoldDifferenceWindowViewModel.BottomPlayerBlueSide),
                                                                       nameof(GoldDifferenceWindowViewModel.SupportPlayerBlueSide),
                                                                       nameof(GoldDifferenceWindowViewModel.TopPlayerRedSide),
                                                                       nameof(GoldDifferenceWindowViewModel.JunglePlayerRedSide),
                                                                       nameof(GoldDifferenceWindowViewModel.MiddlePlayerRedSide),
                                                                       nameof(GoldDifferenceWindowViewModel.BottomPlayerRedSide),
                                                                       nameof(GoldDifferenceWindowViewModel.SupportPlayerRedSide),
                                                                   };
        
        public GoldDifferenceWindowViewModel Model { get; }

        public GoldDifferenceWindowController(GoldDifferenceWindowViewModel? model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Model.PropertyChanged += ModelOnPropertyChanged;
            SubscribeToPropertyChangedEvent(Model.Game, Game_OnPropertyChanged);
            ViewSettings.Instance.PropertyChanged += ViewSettings_OnPropertyChanged;
            
            TryInitializeModel();
            UpdateTopmost();
        }

        private void SubscribeToPropertyChangedEvent(INotifyPropertyChanged? sender, PropertyChangedEventHandler handler)
        {
            if (sender == null)
            {
                return;
            }

            sender.PropertyChanged += handler;
        }

        private void Game_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!ReferenceEquals(sender, Model.Game))
            {
                return;
            }
            
            if (e.PropertyName.Equals(nameof(LoLGame.State)))
            {
                UpdateTopmost();
            }
        }

        private void ViewSettings_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(ViewSettings.GoldDifferenceWindowStayOnTop)))
            {
                UpdateTopmost();
            }
        }

        private void UpdateTopmost()
        {
            Model.Topmost = ViewSettings.Instance.GoldDifferenceWindowStayOnTop switch
                            {
                                StayOnTopType.Off => false,
                                StayOnTopType.DuringGame => Model.Game?.State switch
                                                            {
                                                                null => false,
                                                                LoLGameStateType.Undefined => false,
                                                                LoLGameStateType.Ended => false,
                                                                _ => true,
                                                            },
                                StayOnTopType.Always => true,
                                _ => throw new Exception($"Unknown {nameof(StayOnTopType)} {ViewSettings.Instance.GoldDifferenceWindowStayOnTop}!")
                            };
        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(Model.Game)))
            {
                SubscribeToPropertyChangedEvent(Model.Game, Game_OnPropertyChanged);
                TryInitializeModel();
            }
            else if (string.IsNullOrEmpty(e.PropertyName) || PlayerPropertyNames.Contains(e.PropertyName))
            {
                UpdateActivePlayerBackground();
            }
        }

        private void TryInitializeModel()
        {
            if (Model.Game == null)
            {
                return;
            }

            if (Model.Game.IsInitialized)
            {
                InitializeBlueSidePlayers();
                InitializeRedSidePlayers();
            }
            else
            {
                Model.Game.Initialized += Game_OnInitialized;
            }
        }

        private void UpdateActivePlayerBackground()
        {
            Model.TopPlayerBackground = Model.InactivePlayerBackground;
            Model.JunglePlayerBackground = Model.InactivePlayerBackground;
            Model.MiddlePlayerBackground = Model.InactivePlayerBackground;
            Model.BottomPlayerBackground = Model.InactivePlayerBackground;
            Model.SupportPlayerBackground = Model.InactivePlayerBackground;
            
            if (Model.TopPlayerBlueSide?.IsActivePlayer == true)
            {
                Model.TopPlayerBackground = Model.ActivePlayerOnBlueSideBackground;
                return;
            }
            if (Model.JunglePlayerBlueSide?.IsActivePlayer == true)
            {
                Model.JunglePlayerBackground = Model.ActivePlayerOnBlueSideBackground;
                return;
            }
            if (Model.MiddlePlayerBlueSide?.IsActivePlayer == true)
            {
                Model.MiddlePlayerBackground = Model.ActivePlayerOnBlueSideBackground;
                return;
            }
            if (Model.BottomPlayerBlueSide?.IsActivePlayer == true)
            {
                Model.BottomPlayerBackground = Model.ActivePlayerOnBlueSideBackground;
                return;
            }
            if (Model.SupportPlayerBlueSide?.IsActivePlayer == true)
            {
                Model.SupportPlayerBackground = Model.ActivePlayerOnBlueSideBackground;
                return;
            }
            if (Model.TopPlayerRedSide?.IsActivePlayer == true)
            {
                Model.TopPlayerBackground = Model.ActivePlayerOnRedSideBackground;
                return;
            }
            if (Model.JunglePlayerRedSide?.IsActivePlayer == true)
            {
                Model.JunglePlayerBackground = Model.ActivePlayerOnRedSideBackground;
                return;
            }
            if (Model.MiddlePlayerRedSide?.IsActivePlayer == true)
            {
                Model.MiddlePlayerBackground = Model.ActivePlayerOnRedSideBackground;
                return;
            }
            if (Model.BottomPlayerRedSide?.IsActivePlayer == true)
            {
                Model.BottomPlayerBackground = Model.ActivePlayerOnRedSideBackground;
                return;
            }
            if (Model.SupportPlayerRedSide?.IsActivePlayer == true)
            {
                Model.SupportPlayerBackground = Model.ActivePlayerOnRedSideBackground;
                return;
            }
        }

        private void Game_OnInitialized(object sender, EventArgs e)
        {
            InitializeBlueSidePlayers();
            InitializeRedSidePlayers();
        }

        private void InitializeBlueSidePlayers()
        {
            if (Model.Game?.TeamBlueSide == null)
            {
                return;
            }

            Model.TeamBlueSide = Model.Game.TeamBlueSide;

            var orderedPlayers = OrderPlayers(Model.TeamBlueSide);
            if (orderedPlayers.TryGetValue(LoLPositionType.Top, out var topPlayer))
            {
                Model.TopPlayerBlueSide = topPlayer;
            }

            if (orderedPlayers.TryGetValue(LoLPositionType.Jungle, out var junglePlayer))
            {
                Model.JunglePlayerBlueSide = junglePlayer;
            }

            if (orderedPlayers.TryGetValue(LoLPositionType.Middle, out var middlePlayer))
            {
                Model.MiddlePlayerBlueSide = middlePlayer;
            }

            if (orderedPlayers.TryGetValue(LoLPositionType.Bottom, out var bottomPlayer))
            {
                Model.BottomPlayerBlueSide = bottomPlayer;
            }

            if (orderedPlayers.TryGetValue(LoLPositionType.Support, out var supportPlayer))
            {
                Model.SupportPlayerBlueSide = supportPlayer;
            }
        }

        private void InitializeRedSidePlayers()
        {
            if (Model.Game?.TeamRedSide == null)
            {
                return;
            }

            Model.TeamRedSide = Model.Game.TeamRedSide;

            var orderedPlayers = OrderPlayers(Model.TeamRedSide);
            if (orderedPlayers.TryGetValue(LoLPositionType.Top, out var topPlayer))
            {
                Model.TopPlayerRedSide = topPlayer;
            }

            if (orderedPlayers.TryGetValue(LoLPositionType.Jungle, out var junglePlayer))
            {
                Model.JunglePlayerRedSide = junglePlayer;
            }

            if (orderedPlayers.TryGetValue(LoLPositionType.Middle, out var middlePlayer))
            {
                Model.MiddlePlayerRedSide = middlePlayer;
            }

            if (orderedPlayers.TryGetValue(LoLPositionType.Bottom, out var bottomPlayer))
            {
                Model.BottomPlayerRedSide = bottomPlayer;
            }

            if (orderedPlayers.TryGetValue(LoLPositionType.Support, out var supportPlayer))
            {
                Model.SupportPlayerRedSide = supportPlayer;
            }
        }

        private Dictionary<LoLPositionType, LoLPlayer> OrderPlayers(LoLTeam team)
        {
            var unfilledPositions = new List<LoLPositionType> {LoLPositionType.Top, LoLPositionType.Jungle, LoLPositionType.Middle, LoLPositionType.Bottom, LoLPositionType.Support};
            var result = new Dictionary<LoLPositionType, LoLPlayer>();

            foreach (var player in team.Players)
            {
                var position = player.AssignedPosition;
                if (position == LoLPositionType.Undefined)
                {
                    position = unfilledPositions.First();
                }

                unfilledPositions.Remove(position);
                result.Add(position, player);
            }

            return result;
        }
    }
}