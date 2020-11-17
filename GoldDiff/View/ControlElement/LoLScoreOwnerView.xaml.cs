using System.Windows;
using System.Windows.Controls;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.View.ControlElement
{
    public partial class LoLScoreOwnerView : UserControl
    {
        public ILoLScoreOwner? ScoreOwner
        {
            get => GetValue(ScoreOwnerProperty) as ILoLScoreOwner;
            set => SetValue(ScoreOwnerProperty, value);
        }
        
        public static readonly DependencyProperty ScoreOwnerProperty = DependencyProperty.Register(nameof(ScoreOwner), typeof(ILoLScoreOwner), typeof(LoLScoreOwnerView));

        public LoLTeamType ScoreOwnerTeam
        {
            get => (LoLTeamType) GetValue(ScoreOwnerTeamProperty);
            set => SetValue(ScoreOwnerTeamProperty, value);
        }
        
        public static readonly DependencyProperty ScoreOwnerTeamProperty = DependencyProperty.Register(nameof(ScoreOwnerTeam), typeof(LoLTeamType), typeof(LoLScoreOwnerView),
                                                                                                       new PropertyMetadata(LoLTeamType.BlueSide));

        public bool DisplayScoresSinceLastItemAcquisition
        {
            get => (bool) GetValue(DisplayScoresSinceLastItemAcquisitionProperty);
            set => SetValue(DisplayScoresSinceLastItemAcquisitionProperty, value);
        }
        
        public static readonly DependencyProperty DisplayScoresSinceLastItemAcquisitionProperty = DependencyProperty.Register(nameof(DisplayScoresSinceLastItemAcquisition), typeof(bool), typeof(LoLScoreOwnerView));
        
        public LoLScoreOwnerView()
        {
            InitializeComponent();
        }
    }
}