using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using GoldDiff.LeagueOfLegends.Game;

namespace GoldDiff.View.ControlElement
{
    public partial class TeamGoldDifferenceView : UserControl
    {
        public static readonly DependencyProperty TeamBlueSideProperty = DependencyProperty.Register(nameof(TeamBlueSide), typeof(LoLTeam), MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly DependencyProperty TeamRedSideProperty = DependencyProperty.Register(nameof(TeamRedSide), typeof(LoLTeam), MethodBase.GetCurrentMethod().DeclaringType);
        
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
        
        public TeamGoldDifferenceView()
        {
            InitializeComponent();
        }
    }
}