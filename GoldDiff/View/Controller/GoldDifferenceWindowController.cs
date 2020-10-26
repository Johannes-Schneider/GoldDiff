using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.View.Model;

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
        
        public LoLGame Game { get; }

        public GoldDifferenceWindowController(GoldDifferenceWindowViewModel? model, LoLGame? game)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Model.PropertyChanged += ModelOnPropertyChanged;

            Game = game ?? throw new ArgumentNullException(nameof(game));
            if (Game.IsInitialized)
            {
                InitializeBlueSidePlayers();
                InitializeRedSidePlayers();
            }
            else
            {
                Game.Initialized += Game_OnInitialized;
            }
        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName) || PlayerPropertyNames.Contains(e.PropertyName))
            {
                UpdateActivePlayerBackground();
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
            if (Model.JunglePlayerBlueSide?.IsActivePlayer == true || Model.JunglePlayerRedSide?.IsActivePlayer == true)
            {
                Model.JunglePlayerBackground = Model.ActivePlayerOnBlueSideBackground;
                return;
            }
            if (Model.MiddlePlayerBlueSide?.IsActivePlayer == true || Model.MiddlePlayerRedSide?.IsActivePlayer == true)
            {
                Model.MiddlePlayerBackground = Model.ActivePlayerOnBlueSideBackground;
                return;
            }
            if (Model.BottomPlayerBlueSide?.IsActivePlayer == true || Model.BottomPlayerRedSide?.IsActivePlayer == true)
            {
                Model.BottomPlayerBackground = Model.ActivePlayerOnBlueSideBackground;
                return;
            }
            if (Model.SupportPlayerBlueSide?.IsActivePlayer == true || Model.SupportPlayerRedSide?.IsActivePlayer == true)
            {
                Model.SupportPlayerBackground = Model.ActivePlayerOnBlueSideBackground;
                return;
            }
            if (Model.TopPlayerRedSide?.IsActivePlayer == true)
            {
                Model.TopPlayerBackground = Model.ActivePlayerOnRedSideBackground;
                return;
            }
            if (Model.JunglePlayerRedSide?.IsActivePlayer == true || Model.JunglePlayerRedSide?.IsActivePlayer == true)
            {
                Model.JunglePlayerBackground = Model.ActivePlayerOnRedSideBackground;
                return;
            }
            if (Model.MiddlePlayerRedSide?.IsActivePlayer == true || Model.MiddlePlayerRedSide?.IsActivePlayer == true)
            {
                Model.MiddlePlayerBackground = Model.ActivePlayerOnRedSideBackground;
                return;
            }
            if (Model.BottomPlayerRedSide?.IsActivePlayer == true || Model.BottomPlayerRedSide?.IsActivePlayer == true)
            {
                Model.BottomPlayerBackground = Model.ActivePlayerOnRedSideBackground;
                return;
            }
            if (Model.SupportPlayerRedSide?.IsActivePlayer == true || Model.SupportPlayerRedSide?.IsActivePlayer == true)
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
            if (Game.TeamBlueSide == null)
            {
                return;
            }

            Model.TeamBlueSide = Game.TeamBlueSide;

            var orderedPlayers = OrderPlayers(Game.TeamBlueSide);
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
            if (Game.TeamRedSide == null)
            {
                return;
            }

            Model.TeamRedSide = Game.TeamRedSide;

            var orderedPlayers = OrderPlayers(Game.TeamRedSide);
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