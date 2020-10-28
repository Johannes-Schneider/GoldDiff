using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.View.ControlElement
{
    public partial class PlayerGoldDifferenceView : UserControl
    {
        public class SwapPlayersEventArguments
        {
            public LoLTeamType Team { get; }
            
            public LoLPositionType PositionA { get; }
            
            public LoLPositionType PositionB { get; }

            public SwapPlayersEventArguments(LoLTeamType team, LoLPositionType positionA, LoLPositionType positionB)
            {
                Team = team;
                PositionA = positionA;
                PositionB = positionB;
            }
        }

        public event EventHandler<SwapPlayersEventArguments>? SwapPlayers;

    #region public DepProps

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(LoLPositionType), MethodBase.GetCurrentMethod().DeclaringType);
        
        public static readonly DependencyProperty PlayerBlueSideProperty = DependencyProperty.Register(nameof(PlayerBlueSide), typeof(LoLPlayer), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                       new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty PlayerRedSideProperty = DependencyProperty.Register(nameof(PlayerRedSide), typeof(LoLPlayer), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                      new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty CanSwapPlayersProperty  = DependencyProperty.Register(nameof(CanSwapPlayers), typeof(bool), MethodBase.GetCurrentMethod().DeclaringType);

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is PlayerGoldDifferenceView playerGoldDifferenceView))
            {
                return;
            }

            if (e.Property.Name.Equals(nameof(PlayerBlueSide)))
            {
                if (e.OldValue is LoLPlayer oldPlayer)
                {
                    oldPlayer.PropertyChanged -= playerGoldDifferenceView.PlayerBlueSide_OnPropertyChanged;
                }

                if (e.NewValue is LoLPlayer newPlayer)
                {
                    newPlayer.PropertyChanged += playerGoldDifferenceView.PlayerBlueSide_OnPropertyChanged;
                }
            }
            else if (e.Property.Name.Equals(nameof(PlayerRedSide)))
            {
                if (e.OldValue is LoLPlayer oldPlayer)
                {
                    oldPlayer.PropertyChanged -= playerGoldDifferenceView.PlayerRedSide_OnPropertyChanged;
                }

                if (e.NewValue is LoLPlayer newPlayer)
                {
                    newPlayer.PropertyChanged += playerGoldDifferenceView.PlayerRedSide_OnPropertyChanged;
                }
            }
        }

        public LoLPositionType Position
        {
            get => (LoLPositionType) GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public LoLPlayer? PlayerBlueSide
        {
            get => GetValue(PlayerBlueSideProperty) as LoLPlayer;
            set 
            {
                if (PlayerBlueSide != null)
                {
                    PlayerBlueSide.PropertyChanged -= PlayerBlueSide_OnPropertyChanged;
                }
                
                SetValue(PlayerBlueSideProperty, value);

                if (value != null)
                {
                    value.PropertyChanged += PlayerBlueSide_OnPropertyChanged;
                }
            }
        }
        
        public LoLPlayer? PlayerRedSide
        {
            get => GetValue(PlayerRedSideProperty) as LoLPlayer;
            set
            {
                if (PlayerRedSide != null)
                {
                    PlayerRedSide.PropertyChanged -= PlayerRedSide_OnPropertyChanged;
                }
                
                SetValue(PlayerRedSideProperty, value);

                if (value != null)
                {
                    value.PropertyChanged += PlayerRedSide_OnPropertyChanged;
                }
            }
        }

        public bool CanSwapPlayers
        {
            get => (bool) GetValue(CanSwapPlayersProperty);
            set => SetValue(CanSwapPlayersProperty, value);
        }

    #endregion


    #region private DepProps

        private static readonly DependencyProperty BlueSidePlayerKillsSinceLastItemAcquisitionProperty = DependencyProperty.Register(nameof(BlueSidePlayerKillsSinceLastItemAcquisition), typeof(int), MethodBase.GetCurrentMethod().DeclaringType);
        
        private int BlueSidePlayerKillsSinceLastItemAcquisition
        {
            get => (int) GetValue(BlueSidePlayerKillsSinceLastItemAcquisitionProperty);
            set => SetValue(BlueSidePlayerKillsSinceLastItemAcquisitionProperty, value);
        }

    #endregion

        public PlayerGoldDifferenceView()
        {
            InitializeComponent();
        }

        private void PlayerBlueSide_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }
        
        private void PlayerRedSide_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }

    #region Swap Players (Drag & Drop)
        
        private class DragDropData
        {
            public LoLTeamType Team;
            public LoLPositionType Position;
        }

        private void ChampionTileBlueSide_OnMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            StartDragging(PlayerBlueSide, e);
        }

        private void ChampionTileRedSide_OnMouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            StartDragging(PlayerRedSide, e);
        }

        private void StartDragging(LoLPlayer? player, MouseEventArgs e)
        {
            if (!CanSwapPlayers)
            {
                return;
            }
            
            if (player == null)
            {
                return;
            }

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            DragDrop.DoDragDrop(this, new DataObject(typeof(DragDropData), new DragDropData{Team = player.Team, Position = Position}), DragDropEffects.Move);
        }
        
        private void ChampionTileBlueSide_OnDragOver(object sender, DragEventArgs e)
        {
            HandleDragOver(LoLTeamType.BlueSide, e);
        }

        private void ChampionTileRedSide_OnDragOver(object sender, DragEventArgs e)
        {
            HandleDragOver(LoLTeamType.RedSide, e);
        }

        private void HandleDragOver(LoLTeamType acceptedTeam, DragEventArgs e)
        {
            e.Effects = AcceptDrop(acceptedTeam, e, out _) ? DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }

        private void ChampionTileBlueSide_OnDrop(object sender, DragEventArgs e)
        {
            HandleDrop(LoLTeamType.BlueSide, e);
        }
        
        private void ChampionTileRedSide_OnDrop(object sender, DragEventArgs e)
        {
            HandleDrop(LoLTeamType.RedSide, e);
        }

        private void HandleDrop(LoLTeamType acceptedTeam, DragEventArgs e)
        {
            if (!AcceptDrop(acceptedTeam, e, out var dragDropData))
            {
                return;
            }
            
            if (dragDropData.Position == Position)
            {
                return;
            }
            
            SwapPlayers?.Invoke(this, new SwapPlayersEventArguments(dragDropData.Team, dragDropData.Position, Position));
        }

        private bool AcceptDrop(LoLTeamType acceptedTeam, DragEventArgs e, out DragDropData dragDropData)
        {
            dragDropData = null!;
            
            if (!e.Data.GetDataPresent(typeof(DragDropData)))
            {
                return false;
            }

            dragDropData = (e.Data.GetData(typeof(DragDropData)) as DragDropData)!;
            
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (ReferenceEquals(dragDropData, null) || dragDropData.Team != acceptedTeam)
            {
                return false;
            }
            return true;
        }

    #endregion
        
    }
}