using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GoldDiff.Shared.LeagueOfLegends;

namespace GoldDiff.View.ControlElement
{
    public partial class LoLPositionView : UserControl
    {
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(nameof(Position), typeof(LoLPositionType), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                 new PropertyMetadata(PropertyChangedCallback));
        
        public static readonly DependencyProperty ActivePositionForegroundProperty = DependencyProperty.Register(nameof(ActivePositionForeground), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                                 new PropertyMetadata(PropertyChangedCallback));
        
        public static readonly DependencyProperty InactivePositionForegroundProperty = DependencyProperty.Register(nameof(InactivePositionForeground), typeof(Brush), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                                 new PropertyMetadata(PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LoLPositionView positionView))
            {
                return;
            }

            positionView.OnPositionChanged();
        }

        public LoLPositionType Position
        {
            get => (LoLPositionType) GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public Brush? ActivePositionForeground
        {
            get => GetValue(ActivePositionForegroundProperty) as Brush;
            set => SetValue(ActivePositionForegroundProperty, value);
        }
        
        public Brush? InactivePositionForeground
        {
            get => GetValue(InactivePositionForegroundProperty) as Brush;
            set => SetValue(InactivePositionForegroundProperty, value);
        }

        public LoLPositionView()
        {
            InitializeComponent();
        }

        private void OnPositionChanged()
        {
            switch (Position)
            {
                case LoLPositionType.Top:
                {
                    ShowLanePositions();
                    TopPositionIcon.Foreground = ActivePositionForeground;
                    MiddlePositionIcon.Foreground = InactivePositionForeground;
                    BottomPositionIcon.Foreground = InactivePositionForeground;
                    break;
                }
                case LoLPositionType.Middle:
                {
                    ShowLanePositions();
                    TopPositionIcon.Foreground = InactivePositionForeground;
                    MiddlePositionIcon.Foreground = ActivePositionForeground;
                    BottomPositionIcon.Foreground = InactivePositionForeground;
                    break;
                }
                case LoLPositionType.Bottom:
                {
                    ShowLanePositions();
                    TopPositionIcon.Foreground = InactivePositionForeground;
                    MiddlePositionIcon.Foreground = InactivePositionForeground;
                    BottomPositionIcon.Foreground = ActivePositionForeground;
                    break;
                }
                case LoLPositionType.Jungle:
                {
                    ShowJunglePosition();
                    break;
                }
                case LoLPositionType.Support:
                {
                    ShowSupportPosition();
                    break;
                }
            }

            Visibility = Position == LoLPositionType.Undefined ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ShowLanePositions()
        {
            TopPositionIcon.Visibility = Visibility.Visible;
            MiddlePositionIcon.Visibility = Visibility.Visible;
            BottomPositionIcon.Visibility = Visibility.Visible;
            JunglePositionIcon.Visibility = Visibility.Collapsed;
            SupportPositionIcon.Visibility = Visibility.Collapsed;
        }

        private void ShowJunglePosition()
        {
            TopPositionIcon.Visibility = Visibility.Collapsed;
            MiddlePositionIcon.Visibility = Visibility.Collapsed;
            BottomPositionIcon.Visibility = Visibility.Collapsed;
            JunglePositionIcon.Visibility = Visibility.Visible;
            SupportPositionIcon.Visibility = Visibility.Collapsed;
        }

        private void ShowSupportPosition()
        {
            TopPositionIcon.Visibility = Visibility.Collapsed;
            MiddlePositionIcon.Visibility = Visibility.Collapsed;
            BottomPositionIcon.Visibility = Visibility.Collapsed;
            JunglePositionIcon.Visibility = Visibility.Collapsed;
            SupportPositionIcon.Visibility = Visibility.Visible;
        }
    }
}