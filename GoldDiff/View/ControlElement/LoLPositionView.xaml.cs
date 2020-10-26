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
            if (Position == LoLPositionType.Undefined)
            {
                Visibility = Visibility.Collapsed;
                return;
            }

            Visibility = Visibility.Visible;
            TopPositionIcon.Visibility = Visibility.Collapsed;
            MiddlePositionIcon.Visibility = Visibility.Collapsed;
            BottomPositionIcon.Visibility = Visibility.Collapsed;
            JunglePositionIcon.Visibility = Visibility.Collapsed;
            SupportPositionIcon.Visibility = Visibility.Collapsed;
            
            switch (Position)
            {
                case LoLPositionType.Top:
                {
                    TopPositionIcon.Visibility = Visibility.Visible;
                    break;
                }
                case LoLPositionType.Middle:
                {
                    MiddlePositionIcon.Visibility = Visibility.Visible;
                    break;
                }
                case LoLPositionType.Bottom:
                {
                    BottomPositionIcon.Visibility = Visibility.Visible;
                    break;
                }
                case LoLPositionType.Jungle:
                {
                    JunglePositionIcon.Visibility = Visibility.Visible;
                    break;
                }
                case LoLPositionType.Support:
                {
                    SupportPositionIcon.Visibility = Visibility.Visible;
                    break;
                }
            }
        }
    }
}