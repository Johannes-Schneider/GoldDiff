using System.Windows;
using System.Windows.Controls;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.View.ControlElement
{
    public partial class LoLGoldOwnerView : UserControl
    {
        public ILoLGoldOwner? GoldOwner
        {
            get => GetValue(GoldOwnerProperty) as ILoLGoldOwner;
            set => SetValue(GoldOwnerProperty, value);
        }

        public static readonly DependencyProperty GoldOwnerProperty = DependencyProperty.Register(nameof(GoldOwner), typeof(ILoLGoldOwner), typeof(LoLGoldOwnerView),
                                                                                                  new PropertyMetadata(PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LoLGoldOwnerView goldOwnerView))
            {
                return;
            }

            if (e.Property.Name.Equals(nameof(GoldOwner)))
            {
                goldOwnerView.GoldOwnerHelper.GoldOwner = e.NewValue as ILoLGoldOwner;
            }
        }

        public LoLTeamType GoldOwnerTeam
        {
            get => (LoLTeamType) GetValue(GoldOwnerTeamProperty);
            set => SetValue(GoldOwnerTeamProperty, value);
        }

        public static readonly DependencyProperty GoldOwnerTeamProperty = DependencyProperty.Register(nameof(GoldOwnerTeam), typeof(LoLTeamType), typeof(LoLGoldOwnerView),
                                                                                                      new PropertyMetadata(LoLTeamType.BlueSide));

        public LoLGoldOwnerHelper GoldOwnerHelper { get; } = new();

        public LoLGoldOwnerView()
        {
            InitializeComponent();
        }
    }
}