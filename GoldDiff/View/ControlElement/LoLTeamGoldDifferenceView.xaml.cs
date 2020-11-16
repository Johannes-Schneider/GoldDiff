using System.Windows;
using System.Windows.Controls;
using GoldDiff.LeagueOfLegends.Game;

namespace GoldDiff.View.ControlElement
{
    public partial class LoLTeamGoldDifferenceView : UserControl
    {
        public static readonly DependencyProperty TeamBlueSideProperty = DependencyProperty.Register(nameof(TeamBlueSide), typeof(LoLTeam), typeof(LoLTeamGoldDifferenceView),
                                                                                                     new PropertyMetadata(PropertyChangedCallback));

        public static readonly DependencyProperty TeamRedSideProperty = DependencyProperty.Register(nameof(TeamRedSide), typeof(LoLTeam), typeof(LoLTeamGoldDifferenceView),
                                                                                                    new PropertyMetadata(PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LoLTeamGoldDifferenceView teamGoldDifferenceView))
            {
                return;
            }

            switch (e.Property.Name)
            {
                case nameof(TeamBlueSide):
                {
                    teamGoldDifferenceView.GoldOwnerHelperBlueSide.GoldOwner = e.NewValue as ILoLGoldOwner;
                    break;
                }
                case nameof(TeamRedSide):
                {
                    teamGoldDifferenceView.GoldOwnerHelperRedSide.GoldOwner = e.NewValue as ILoLGoldOwner;
                    break;
                }
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

        private LoLGoldOwnerHelper GoldOwnerHelperBlueSide { get; } = new();

        private LoLGoldOwnerHelper GoldOwnerHelperRedSide { get; } = new();
        
        public LoLTeamGoldDifferenceView()
        {
            InitializeComponent();
        }
    }
}