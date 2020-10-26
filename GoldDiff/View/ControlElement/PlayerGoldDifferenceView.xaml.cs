using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.View.ControlElement
{
    public partial class PlayerGoldDifferenceView : UserControl
    {
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(LoLPositionType), MethodBase.GetCurrentMethod().DeclaringType);
        
        public static readonly DependencyProperty PlayerBlueSideProperty = DependencyProperty.Register(nameof(PlayerBlueSide), typeof(LoLPlayer), MethodBase.GetCurrentMethod().DeclaringType);
        
        public static readonly DependencyProperty PlayerRedSideProperty = DependencyProperty.Register(nameof(PlayerRedSide), typeof(LoLPlayer), MethodBase.GetCurrentMethod().DeclaringType);
        
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

        public PlayerGoldDifferenceView()
        {
            InitializeComponent();
        }
    }
}