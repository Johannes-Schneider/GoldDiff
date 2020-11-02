using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.View;

namespace GoldDiff.View.ControlElement
{
    public partial class GoldDifferenceView : UserControl
    {
    #region public DepProps

        public static readonly DependencyProperty GoldOwnerBlueSideProperty = DependencyProperty.Register(nameof(GoldOwnerBlueSide), typeof(ILoLGoldOwner), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                          new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty GoldOwnerRedSideProperty = DependencyProperty.Register(nameof(GoldOwnerRedSide), typeof(ILoLGoldOwner), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                         new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty BlueSideForegroundProperty = DependencyProperty.Register(nameof(BlueSideForeground), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                           new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty RedSideForegroundProperty = DependencyProperty.Register(nameof(RedSideForeground), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                          new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty NoDifferenceForegroundProperty = DependencyProperty.Register(nameof(NoDifferenceForeground), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                               new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty MinorGoldDifferenceProperty = DependencyProperty.Register(nameof(MinorGoldDifference), typeof(int), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                            new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty MediocreGoldDifferenceProperty = DependencyProperty.Register(nameof(MediocreGoldDifference), typeof(int), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                               new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty LargeGoldDifferenceProperty = DependencyProperty.Register(nameof(LargeGoldDifference), typeof(int), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                            new PropertyMetadata(PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is GoldDifferenceView goldDifferenceIndicator))
            {
                return;
            }

            if (e.Property.Name.Equals(nameof(GoldOwnerBlueSide)))
            {
                goldDifferenceIndicator.GoldComparisonHelper.GoldOwnerBlueSide = e.NewValue as ILoLGoldOwner;
            }
            else if (e.Property.Name.Equals(nameof(GoldOwnerRedSide)))
            {
                goldDifferenceIndicator.GoldComparisonHelper.GoldOwnerRedSide = e.NewValue as ILoLGoldOwner;
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

        private static readonly DependencyProperty GoldDifferenceProperty = DependencyProperty.Register(nameof(GoldDifference), typeof(int), MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly DependencyProperty GoldAdvantageBlueSideIconProperty = DependencyProperty.Register(nameof(GoldAdvantageBlueSideIcon), typeof(Geometry), MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly DependencyProperty GoldAdvantageRedSideIconProperty = DependencyProperty.Register(nameof(GoldAdvantageRedSideIcon), typeof(Geometry), MethodBase.GetCurrentMethod().DeclaringType);

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

        private GoldComparisonHelper GoldComparisonHelper { get; }

        public GoldDifferenceView()
        {
            GoldComparisonHelper = new GoldComparisonHelper();
            GoldComparisonHelper.PropertyChanged += GoldComparisonHelper_OnPropertyChanged;

            InitializeComponent();
        }

        private void GoldComparisonHelper_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName.Equals(nameof(GoldComparisonHelper.GoldBlueSide)) 
                || e.PropertyName.Equals(nameof(GoldComparisonHelper.GoldRedSide)))
            {
                UpdateGoldDifference();
            }
        }

        private void UpdateGoldDifference()
        {
            if (GoldOwnerBlueSide == null || GoldOwnerRedSide == null)
            {
                Visibility = Visibility.Hidden;
                return;
            }

            Visibility = Visibility.Visible;
            GoldDifference = Math.Abs(GoldComparisonHelper.GoldBlueSide - GoldComparisonHelper.GoldRedSide);
            var winningSide = GoldComparisonHelper.GoldBlueSide >= GoldComparisonHelper.GoldRedSide ? LoLTeamType.BlueSide : LoLTeamType.RedSide;

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
                                            LoLTeamType.BlueSide when GoldDifference < MinorGoldDifference => Application.Current.Resources[GoldDiffSharedResourceKeys.IconArrowLeft1] as Geometry,
                                            LoLTeamType.BlueSide when GoldDifference < MediocreGoldDifference => Application.Current.Resources[GoldDiffSharedResourceKeys.IconArrowLeft2] as Geometry,
                                            LoLTeamType.BlueSide when GoldDifference < LargeGoldDifference => Application.Current.Resources[GoldDiffSharedResourceKeys.IconArrowLeft3] as Geometry,
                                            LoLTeamType.BlueSide when GoldDifference >= LargeGoldDifference => Application.Current.Resources[GoldDiffSharedResourceKeys.IconArrowLeft4] as Geometry,
                                            _ => null,
                                        };
            GoldAdvantageRedSideIcon = winningSide switch
                                       {
                                           _ when GoldDifference == 0 => null,
                                           LoLTeamType.RedSide when GoldDifference < MinorGoldDifference => Application.Current.Resources[GoldDiffSharedResourceKeys.IconArrowRight1] as Geometry,
                                           LoLTeamType.RedSide when GoldDifference < MediocreGoldDifference => Application.Current.Resources[GoldDiffSharedResourceKeys.IconArrowRight2] as Geometry,
                                           LoLTeamType.RedSide when GoldDifference < LargeGoldDifference => Application.Current.Resources[GoldDiffSharedResourceKeys.IconArrowRight3] as Geometry,
                                           LoLTeamType.RedSide when GoldDifference >= LargeGoldDifference => Application.Current.Resources[GoldDiffSharedResourceKeys.IconArrowRight4] as Geometry,
                                           _ => null,
                                       };
        }
    }
}