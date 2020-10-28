using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using GoldDiff.LeagueOfLegends.Game;

namespace GoldDiff.View.ControlElement
{
    public partial class PlayerKdaView : UserControl
    {
    #region public DepProps

        public static readonly DependencyProperty PlayerProperty = DependencyProperty.Register(nameof(Player), typeof(LoLPlayer), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                               new PropertyMetadata(PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is PlayerKdaView playerKdaView))
            {
                return;
            }

            if (e.Property.Name.Equals(nameof(Player)))
            {
                playerKdaView.OnPlayerChanged(e.OldValue as LoLPlayer, e.NewValue as LoLPlayer);
            }
        }
        
        public LoLPlayer? Player
        {
            get => GetValue(PlayerProperty) as LoLPlayer;
            set
            {
                OnPlayerChanged(Player, value);
                SetValue(PlayerProperty, value);
            }
        }

        private void OnPlayerChanged(LoLPlayer? oldValue, LoLPlayer? newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= Player_OnPropertyChanged;
                oldValue.ItemsChanged -= Player_OnItemsChanged;
            }

            if (newValue != null)
            {
                newValue.PropertyChanged += Player_OnPropertyChanged;
                newValue.ItemsChanged += Player_OnItemsChanged;
            }
        }

    #endregion

    #region private DepProps

        private static readonly DependencyProperty NewKillsProperty = DependencyProperty.Register(nameof(NewKills), typeof(int), MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly DependencyProperty NewAssistsProperty = DependencyProperty.Register(nameof(NewAssists), typeof(int), MethodBase.GetCurrentMethod().DeclaringType);

        private int NewKills
        {
            get => (int) GetValue(NewKillsProperty);
            set => SetValue(NewKillsProperty, value);
        }

        private int NewAssists
        {
            get => (int) GetValue(NewAssistsProperty);
            set => SetValue(NewAssistsProperty, value);
        }

    #endregion

        private int _killsAtLastItemAcquisition;
        private int _assistsAtLastItemAcquisition;

        public PlayerKdaView()
        {
            InitializeComponent();
        }

        private void Player_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(Player.Kills)))
            {
                NewKills = Player?.Kills - _killsAtLastItemAcquisition ?? 0;
            }
            else if (e.PropertyName.Equals(nameof(Player.Assists)))
            {
                NewAssists = Player?.Assists - _assistsAtLastItemAcquisition ?? 0;
            }
        }
        
        private void Player_OnItemsChanged(object sender, ItemsChangedEventArguments e)
        {
            if (!e.AddedItems.Any())
            {
                return;
            }

            _killsAtLastItemAcquisition = Player?.Kills ?? 0;
            _assistsAtLastItemAcquisition = Player?.Assists ?? 0;
            NewKills = 0;
            NewAssists = 0;
        }
    }
}