using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using GoldDiff.LeagueOfLegends.Game;

namespace GoldDiff.View.ControlElement
{
    public partial class LoLGameView : UserControl
    {
    #region public DepProps

        public LoLGame? Game
        {
            get => GetValue(GameProperty) as LoLGame;
            set => SetValue(GameProperty, value);
        }

        public static readonly DependencyProperty GameProperty = DependencyProperty.Register(nameof(Game), typeof(LoLGame), typeof(LoLGameView),
                                                                                             new PropertyMetadata(PublicPropertyChangedCallback));

        private static void PublicPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LoLGameView gameView))
            {
                return;
            }

            switch (e.Property.Name)
            {
                case nameof(Game):
                {
                    if (e.OldValue is LoLGame oldValue)
                    {
                        oldValue.PropertyChanged -= gameView.Game_OnPropertyChanged;
                    }

                    if (e.NewValue is LoLGame newValue)
                    {
                        newValue.PropertyChanged += gameView.Game_OnPropertyChanged;
                    }

                    gameView.UpdateTeams();
                    gameView.UpdateRespawnTimerCollection();
                    break;
                }
            }
        }

    #endregion

    #region private DepProps

        private LoLTeam? TeamBlueSide
        {
            get => GetValue(TeamBlueSideProperty) as LoLTeam;
            set => SetValue(TeamBlueSideProperty, value);
        }

        private static readonly DependencyProperty TeamBlueSideProperty = DependencyProperty.Register(nameof(TeamBlueSide), typeof(LoLTeam), typeof(LoLGameView));

        private LoLTeam? TeamRedSide
        {
            get => GetValue(TeamRedSideProperty) as LoLTeam;
            set => SetValue(TeamRedSideProperty, value);
        }

        private static readonly DependencyProperty TeamRedSideProperty = DependencyProperty.Register(nameof(TeamRedSide), typeof(LoLTeam), typeof(LoLGameView));

        private LoLObjectiveRespawnTimerCollection? RespawnTimerCollection
        {
            get => GetValue(RespawnTimerCollectionProperty) as LoLObjectiveRespawnTimerCollection;
            set => SetValue(RespawnTimerCollectionProperty, value);
        }

        private static readonly DependencyProperty RespawnTimerCollectionProperty =
            DependencyProperty.Register(nameof(RespawnTimerCollection), typeof(LoLObjectiveRespawnTimerCollection), typeof(LoLGameView));

    #endregion

        public LoLGameView()
        {
            InitializeComponent();
        }

        private void Game_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName))
            {
                UpdateTeams();
                UpdateRespawnTimerCollection();
            }
            else if (e.PropertyName.Equals(nameof(LoLGame.TeamBlueSide)) ||
                     e.PropertyName.Equals(nameof(LoLGame.TeamRedSide)))
            {
                UpdateTeams();
            }
            else if (e.PropertyName.Equals(nameof(LoLGame.ObjectiveRespawnTimerCollection)))
            {
                UpdateRespawnTimerCollection();
            }
        }

        private void UpdateTeams()
        {
            TeamBlueSide = Game?.TeamBlueSide;
            TeamRedSide = Game?.TeamRedSide;
        }

        private void UpdateRespawnTimerCollection()
        {
            RespawnTimerCollection = Game?.ObjectiveRespawnTimerCollection;
        }
    }
}