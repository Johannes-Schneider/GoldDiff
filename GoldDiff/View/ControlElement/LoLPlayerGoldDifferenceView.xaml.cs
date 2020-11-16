using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.View.ControlElement
{
    public partial class LoLPlayerGoldDifferenceView : UserControl
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

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(LoLPositionType), typeof(LoLPlayerGoldDifferenceView));

        public static readonly DependencyProperty PlayerBlueSideProperty = DependencyProperty.Register(nameof(PlayerBlueSide), typeof(LoLPlayer), typeof(LoLPlayerGoldDifferenceView));

        public static readonly DependencyProperty PlayerRedSideProperty = DependencyProperty.Register(nameof(PlayerRedSide), typeof(LoLPlayer), typeof(LoLPlayerGoldDifferenceView));

        public static readonly DependencyProperty CanSwapPlayersProperty = DependencyProperty.Register(nameof(CanSwapPlayers), typeof(bool), typeof(LoLPlayerGoldDifferenceView));

        public LoLPositionType Position
        {
            get => (LoLPositionType) GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public LoLPlayer? PlayerBlueSide
        {
            get => GetValue(PlayerBlueSideProperty) as LoLPlayer;
            set => SetValue(PlayerBlueSideProperty, value);
        }

        public LoLPlayer? PlayerRedSide
        {
            get => GetValue(PlayerRedSideProperty) as LoLPlayer;
            set => SetValue(PlayerRedSideProperty, value);
        }

        public bool CanSwapPlayers
        {
            get => (bool) GetValue(CanSwapPlayersProperty);
            set => SetValue(CanSwapPlayersProperty, value);
        }

    #endregion

        public LoLPlayerGoldDifferenceView()
        {
            InitializeComponent();
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

            DragDrop.DoDragDrop(this, new DataObject(typeof(DragDropData), new DragDropData {Team = player.Team, Position = Position}), DragDropEffects.Move);
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