using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FlatXaml.Icon;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.View.ControlElement
{
    public partial class LoLTeamPowerPlayView : UserControl
    {
    #region public DepProps

    #endregion

        public LoLTeamPowerPlay? PowerPlay
        {
            get => GetValue(PowerPlayProperty) as LoLTeamPowerPlay;
            set => SetValue(PowerPlayProperty, value);
        }

        public static readonly DependencyProperty PowerPlayProperty = DependencyProperty.Register(nameof(PowerPlay), typeof(LoLTeamPowerPlay), typeof(LoLTeamPowerPlayView),
                                                                                                  new PropertyMetadata(PropertyChangedCallback));

        public Geometry? PowerPlayIcon
        {
            get => GetValue(PowerPlayIconProperty) as Geometry;
            set => SetValue(PowerPlayIconProperty, value);
        }

        public static readonly DependencyProperty PowerPlayIconProperty = DependencyProperty.Register(nameof(PowerPlayIcon), typeof(Geometry), typeof(LoLTeamPowerPlayView));

        public int MinorGoldDifference
        {
            get => (int) GetValue(MinorGoldDifferenceProperty);
            set => SetValue(MinorGoldDifferenceProperty, value);
        }

        public static readonly DependencyProperty MinorGoldDifferenceProperty = DependencyProperty.Register(nameof(MinorGoldDifference), typeof(int), typeof(LoLTeamPowerPlayView),
                                                                                                            new PropertyMetadata(PropertyChangedCallback));

        public int MediocreGoldDifference
        {
            get => (int) GetValue(MediocreGoldDifferenceProperty);
            set => SetValue(MediocreGoldDifferenceProperty, value);
        }

        public static readonly DependencyProperty MediocreGoldDifferenceProperty = DependencyProperty.Register(nameof(MediocreGoldDifference), typeof(int), typeof(LoLTeamPowerPlayView),
                                                                                                               new PropertyMetadata(PropertyChangedCallback));

        public int LargeGoldDifference
        {
            get => (int) GetValue(LargeGoldDifferenceProperty);
            set => SetValue(LargeGoldDifferenceProperty, value);
        }

        public static readonly DependencyProperty LargeGoldDifferenceProperty = DependencyProperty.Register(nameof(LargeGoldDifference), typeof(int), typeof(LoLTeamPowerPlayView),
                                                                                                            new PropertyMetadata(PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LoLTeamPowerPlayView teamPowerPlayView))
            {
                return;
            }

            switch (e.Property.Name)
            {
                case nameof(PowerPlay):
                {
                    teamPowerPlayView.GoldOwnerHelper.GoldOwner = e.NewValue as ILoLGoldOwner;
                    break;
                }
            }

            teamPowerPlayView.Update();
        }

    #region private DepProps

        private Geometry? GoldAdvantageBlueSideIcon
        {
            get => GetValue(GoldAdvantageBlueSideIconProperty) as Geometry;
            set => SetValue(GoldAdvantageBlueSideIconProperty, value);
        }

        private static readonly DependencyProperty GoldAdvantageBlueSideIconProperty = DependencyProperty.Register(nameof(GoldAdvantageBlueSideIcon), typeof(Geometry), typeof(LoLTeamPowerPlayView));

        private Geometry? GoldAdvantageRedSideIcon
        {
            get => GetValue(GoldAdvantageRedSideIconProperty) as Geometry;
            set => SetValue(GoldAdvantageRedSideIconProperty, value);
        }

        private static readonly DependencyProperty GoldAdvantageRedSideIconProperty = DependencyProperty.Register(nameof(GoldAdvantageRedSideIcon), typeof(Geometry), typeof(LoLTeamPowerPlayView));

        private int Gold
        {
            get => (int) GetValue(GoldProperty);
            set => SetValue(GoldProperty, value);
        }

        private static readonly DependencyProperty GoldProperty = DependencyProperty.Register(nameof(Gold), typeof(int), typeof(LoLTeamPowerPlayView));

    #endregion

        private LoLGoldOwnerHelper GoldOwnerHelper { get; } = new();

        public LoLTeamPowerPlayView()
        {
            InitializeComponent();

            GoldOwnerHelper.PropertyChanged += (_, _) => Update();
        }

        private void Update()
        {
            if (PowerPlay == null)
            {
                return;
            }

            var winningTeam = PowerPlay.Team switch
                              {
                                  _ when GoldOwnerHelper.Gold >= 0 => PowerPlay.Team,
                                  LoLTeamType.BlueSide => LoLTeamType.RedSide,
                                  LoLTeamType.RedSide => LoLTeamType.BlueSide,
                                  _ => LoLTeamType.Undefined,
                              };

            var absoluteGoldDifference = Math.Abs(GoldOwnerHelper.Gold);

            GoldAdvantageBlueSideIcon = winningTeam switch
                                        {
                                            LoLTeamType.BlueSide when absoluteGoldDifference == 0 => null,
                                            LoLTeamType.BlueSide when absoluteGoldDifference < MinorGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronLeft1] as Geometry,
                                            LoLTeamType.BlueSide when absoluteGoldDifference < MediocreGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronLeft2] as Geometry,
                                            LoLTeamType.BlueSide when absoluteGoldDifference < LargeGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronLeft3] as Geometry,
                                            LoLTeamType.BlueSide when absoluteGoldDifference >= LargeGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronLeft4] as Geometry,
                                            _ => null,
                                        };
            GoldAdvantageRedSideIcon = winningTeam switch
                                       {
                                           LoLTeamType.RedSide when absoluteGoldDifference == 0 => null,
                                           LoLTeamType.RedSide when absoluteGoldDifference < MinorGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronRight1] as Geometry,
                                           LoLTeamType.RedSide when absoluteGoldDifference < MediocreGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronRight2] as Geometry,
                                           LoLTeamType.RedSide when absoluteGoldDifference < LargeGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronRight3] as Geometry,
                                           LoLTeamType.RedSide when absoluteGoldDifference >= LargeGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronRight4] as Geometry,
                                           _ => null,
                                       };

            Gold = GoldOwnerHelper.Gold;
        }
    }
}