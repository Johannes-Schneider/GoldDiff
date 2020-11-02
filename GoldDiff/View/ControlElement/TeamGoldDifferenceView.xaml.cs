using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using GoldDiff.LeagueOfLegends.Game;

namespace GoldDiff.View.ControlElement
{
    public partial class TeamGoldDifferenceView : UserControl
    {
        public static readonly DependencyProperty TeamBlueSideProperty = DependencyProperty.Register(nameof(TeamBlueSide), typeof(LoLTeam), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                     new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty TeamRedSideProperty = DependencyProperty.Register(nameof(TeamRedSide), typeof(LoLTeam), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                    new PropertyMetadata(PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TeamGoldDifferenceView teamGoldDifferenceView))
            {
                return;
            }

            if (e.Property.Name.Equals(nameof(TeamBlueSide)))
            {
                teamGoldDifferenceView.GoldComparisonHelper.GoldOwnerBlueSide = e.NewValue as ILoLGoldOwner;
            }
            else if (e.Property.Name.Equals(nameof(TeamRedSide)))
            {
                teamGoldDifferenceView.GoldComparisonHelper.GoldOwnerRedSide = e.NewValue as ILoLGoldOwner;
            }
        }

        public LoLTeam? TeamBlueSide
        {
            get => GetValue(TeamBlueSideProperty) as LoLTeam;
            set => SetValue(TeamBlueSideProperty, value);
        }
        
        public LoLTeam? TeamRedSide
        {
            get => GetValue(TeamRedSideProperty) as LoLTeam;
            set => SetValue(TeamRedSideProperty, value);
        }
        
        public GoldComparisonHelper GoldComparisonHelper { get; }
        
        public TeamGoldDifferenceView()
        {
            GoldComparisonHelper = new GoldComparisonHelper();
            
            InitializeComponent();
        }
    }
}