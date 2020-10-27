using System.Windows;
using System.Windows.Input;
using GoldDiff.LeagueOfLegends.Game;

namespace GoldDiff.Playground
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialog()
                         {
                             Owner = this,
                         };
            dialog.ShowDialog();
        }
    }
}