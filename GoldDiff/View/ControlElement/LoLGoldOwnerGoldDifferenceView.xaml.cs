using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FlatXaml.Icon;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.View.ControlElement
{
    public partial class LoLGoldOwnerGoldDifferenceView : UserControl
    {
    #region public DepProps

        public static readonly DependencyProperty GoldOwnerBlueSideProperty = DependencyProperty.Register(nameof(GoldOwnerBlueSide), typeof(ILoLGoldOwner), typeof(LoLGoldOwnerGoldDifferenceView),
                                                                                                          new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty GoldOwnerRedSideProperty = DependencyProperty.Register(nameof(GoldOwnerRedSide), typeof(ILoLGoldOwner), typeof(LoLGoldOwnerGoldDifferenceView),
                                                                                                         new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty BlueSideForegroundProperty = DependencyProperty.Register(nameof(BlueSideForeground), typeof(Brush), typeof(LoLGoldOwnerGoldDifferenceView),
                                                                                                           new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty RedSideForegroundProperty = DependencyProperty.Register(nameof(RedSideForeground), typeof(Brush), typeof(LoLGoldOwnerGoldDifferenceView),
                                                                                                          new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty NoDifferenceForegroundProperty = DependencyProperty.Register(nameof(NoDifferenceForeground), typeof(Brush), typeof(LoLGoldOwnerGoldDifferenceView),
                                                                                                               new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty MinorGoldDifferenceProperty = DependencyProperty.Register(nameof(MinorGoldDifference), typeof(int), typeof(LoLGoldOwnerGoldDifferenceView),
                                                                                                            new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty MediocreGoldDifferenceProperty = DependencyProperty.Register(nameof(MediocreGoldDifference), typeof(int), typeof(LoLGoldOwnerGoldDifferenceView),
                                                                                                               new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty LargeGoldDifferenceProperty = DependencyProperty.Register(nameof(LargeGoldDifference), typeof(int), typeof(LoLGoldOwnerGoldDifferenceView),
                                                                                                            new PropertyMetadata(PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LoLGoldOwnerGoldDifferenceView goldDifferenceIndicator))
            {
                return;
            }

            switch (e.Property.Name)
            {
                case nameof(GoldOwnerBlueSide):
                {
                    goldDifferenceIndicator.GoldOwnerHelperBlueSide.GoldOwner = e.NewValue as ILoLGoldOwner;
                    break;
                }
                case nameof(GoldOwnerRedSide):
                {
                    goldDifferenceIndicator.GoldOwnerHelperRedSide.GoldOwner = e.NewValue as ILoLGoldOwner;
                    break;
                }
            }

            goldDifferenceIndicator.UpdateGoldDifference();
        }

        public ILoLGoldOwner? GoldOwnerBlueSide
        {
            get => GetValue(GoldOwnerBlueSideProperty) as ILoLGoldOwner;
            set => SetValue(GoldOwnerBlueSideProperty, value);
        }

        public ILoLGoldOwner? GoldOwnerRedSide
        {
            get => GetValue(GoldOwnerRedSideProperty) as ILoLGoldOwner;
            set => SetValue(GoldOwnerRedSideProperty, value);
        }

        public Brush? BlueSideForeground
        {
            get => GetValue(BlueSideForegroundProperty) as Brush;
            set => SetValue(BlueSideForegroundProperty, value);
        }

        public Brush? RedSideForeground
        {
            get => GetValue(RedSideForegroundProperty) as Brush;
            set => SetValue(RedSideForegroundProperty, value);
        }

        public Brush? NoDifferenceForeground
        {
            get => GetValue(NoDifferenceForegroundProperty) as Brush;
            set => SetValue(NoDifferenceForegroundProperty, value);
        }

        public int MinorGoldDifference
        {
            get => (int) GetValue(MinorGoldDifferenceProperty);
            set => SetValue(MinorGoldDifferenceProperty, value);
        }

        public int MediocreGoldDifference
        {
            get => (int) GetValue(MediocreGoldDifferenceProperty);
            set => SetValue(MediocreGoldDifferenceProperty, value);
        }

        public int LargeGoldDifference
        {
            get => (int) GetValue(LargeGoldDifferenceProperty);
            set => SetValue(LargeGoldDifferenceProperty, value);
        }

    #endregion

    #region private DepProps

        private static readonly DependencyProperty GoldDifferenceProperty = DependencyProperty.Register(nameof(GoldDifference), typeof(int), typeof(LoLGoldOwnerGoldDifferenceView));

        private static readonly DependencyProperty GoldAdvantageBlueSideIconProperty = DependencyProperty.Register(nameof(GoldAdvantageBlueSideIcon), typeof(Geometry), typeof(LoLGoldOwnerGoldDifferenceView));

        private static readonly DependencyProperty GoldAdvantageRedSideIconProperty = DependencyProperty.Register(nameof(GoldAdvantageRedSideIcon), typeof(Geometry), typeof(LoLGoldOwnerGoldDifferenceView));

        private int GoldDifference
        {
            get => (int) GetValue(GoldDifferenceProperty);
            set => SetValue(GoldDifferenceProperty, value);
        }

        private Geometry? GoldAdvantageBlueSideIcon
        {
            get => GetValue(GoldAdvantageBlueSideIconProperty) as Geometry;
            set => SetValue(GoldAdvantageBlueSideIconProperty, value);
        }

        private Geometry? GoldAdvantageRedSideIcon
        {
            get => GetValue(GoldAdvantageRedSideIconProperty) as Geometry;
            set => SetValue(GoldAdvantageRedSideIconProperty, value);
        }

    #endregion

        private LoLGoldOwnerHelper GoldOwnerHelperBlueSide { get; } = new();

        private LoLGoldOwnerHelper GoldOwnerHelperRedSide { get; } = new();

        public LoLGoldOwnerGoldDifferenceView()
        {
            GoldOwnerHelperBlueSide.PropertyChanged += (_, _) => UpdateGoldDifference();
            GoldOwnerHelperRedSide.PropertyChanged += (_, _) => UpdateGoldDifference();

            InitializeComponent();
        }

        private void UpdateGoldDifference()
        {
            if (GoldOwnerBlueSide == null || GoldOwnerRedSide == null)
            {
                Badge.Visibility = Visibility.Hidden;
                return;
            }

            Badge.Visibility = Visibility.Visible;
            GoldDifference = Math.Abs(GoldOwnerHelperBlueSide.Gold - GoldOwnerHelperRedSide.Gold);
            var winningSide = GoldOwnerHelperBlueSide.Gold >= GoldOwnerHelperRedSide.Gold ? LoLTeamType.BlueSide : LoLTeamType.RedSide;

            Foreground = winningSide switch
                         {
                             _ when GoldDifference == 0 => NoDifferenceForeground,
                             LoLTeamType.BlueSide when GoldDifference > 0 => BlueSideForeground,
                             LoLTeamType.RedSide when GoldDifference > 0 => RedSideForeground,
                             _ => throw new Exception($"Unknown {nameof(winningSide)} {winningSide}!"),
                         };
            GoldAdvantageBlueSideIcon = winningSide switch
                                        {
                                            _ when GoldDifference == 0 => null,
                                            LoLTeamType.BlueSide when GoldDifference < MinorGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronLeft1] as Geometry,
                                            LoLTeamType.BlueSide when GoldDifference < MediocreGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronLeft2] as Geometry,
                                            LoLTeamType.BlueSide when GoldDifference < LargeGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronLeft3] as Geometry,
                                            LoLTeamType.BlueSide when GoldDifference >= LargeGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronLeft4] as Geometry,
                                            _ => null,
                                        };
            GoldAdvantageRedSideIcon = winningSide switch
                                       {
                                           _ when GoldDifference == 0 => null,
                                           LoLTeamType.RedSide when GoldDifference < MinorGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronRight1] as Geometry,
                                           LoLTeamType.RedSide when GoldDifference < MediocreGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronRight2] as Geometry,
                                           LoLTeamType.RedSide when GoldDifference < LargeGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronRight3] as Geometry,
                                           LoLTeamType.RedSide when GoldDifference >= LargeGoldDifference => Application.Current.Resources[FlatIconKeys.ChevronRight4] as Geometry,
                                           _ => null,
                                       };
        }
    }
}