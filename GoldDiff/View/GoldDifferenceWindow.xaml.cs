using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.View.ControlElement;
using GoldDiff.View.Controller;
using GoldDiff.View.Model;

namespace GoldDiff.View
{
    public partial class GoldDifferenceWindow : Window
    {
        public const string ActivePlayerOnBlueSideBackground = nameof(ActivePlayerOnBlueSideBackground);
        public const string ActivePlayerOnRedSideBackground = nameof(ActivePlayerOnRedSideBackground);

        private GoldDifferenceWindowViewModel PrivateModel
        {
            get => GetValue(PrivateModelProperty) as GoldDifferenceWindowViewModel ?? throw new NullReferenceException($"{nameof(PrivateModel)} must not be {null}!");
            set => SetValue(PrivateModelProperty, value ?? throw new ArgumentNullException(nameof(value)));
        }

        private static readonly DependencyProperty PrivateModelProperty =
            DependencyProperty.Register(nameof(PrivateModel), typeof(GoldDifferenceWindowViewModel), typeof(GoldDifferenceWindow));

        public GoldDifferenceWindowViewModel Model { get; }

        public GoldDifferenceWindowController Controller { get; }

        public GoldDifferenceWindow(LoLGame? game)
        {
            InitializeComponent();

            Model = new GoldDifferenceWindowViewModel
                    {
                        Game = game,
                        ActivePlayerOnBlueSideBackground = Resources[ActivePlayerOnBlueSideBackground] as Brush,
                        ActivePlayerOnRedSideBackground = Resources[ActivePlayerOnRedSideBackground] as Brush,
                        InactivePlayerBackground = null,
                    };
            Controller = new GoldDifferenceWindowController(Model);
            PrivateModel = Model;
            
            WindowControlBar.MouseLeftButtonDown += WindowControlBar_OnMouseLeftButtonDown;
        }

        private void PlayerGoldDifferenceView_OnSwapPlayers(object sender, LoLPlayerGoldDifferenceView.SwapPlayersEventArguments e)
        {
            if (e.PositionA == e.PositionB)
            {
                return;
            }
            
            Model.SwapPlayers(e.Team, e.PositionA, e.PositionB);
        }

        private void WindowControlBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}