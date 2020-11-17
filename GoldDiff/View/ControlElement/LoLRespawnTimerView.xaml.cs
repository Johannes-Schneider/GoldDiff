using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.View.ControlElement
{
    public partial class LoLRespawnTimerView : UserControl
    {
        public LoLRespawnTimer? RespawnTimer
        {
            get => GetValue(RespawnTimerProperty) as LoLRespawnTimer;
            set => SetValue(RespawnTimerProperty, value);
        }
        
        public static readonly DependencyProperty RespawnTimerProperty = DependencyProperty.Register(nameof(RespawnTimer), typeof(LoLRespawnTimer), typeof(LoLRespawnTimerView));

        public LoLTeamType RespawnTimerTeam
        {
            get => (LoLTeamType) GetValue(RespawnTimerTeamProperty);
            set => SetValue(RespawnTimerTeamProperty, value);
        }
        
        public static readonly DependencyProperty RespawnTimerTeamProperty = DependencyProperty.Register(nameof(RespawnTimerTeam), typeof(LoLTeamType), typeof(LoLRespawnTimerView),
                                                                                                         new PropertyMetadata(LoLTeamType.BlueSide));

        public Geometry? Icon
        {
            get => GetValue(IconProperty) as Geometry;
            set => SetValue(IconProperty, value);
        }
        
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(Geometry), typeof(LoLRespawnTimerView));

        public LoLRespawnTimerView()
        {
            InitializeComponent();
        }
    }
}