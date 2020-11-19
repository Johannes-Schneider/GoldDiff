using System.Windows;
using FlatXaml.Model;

namespace GoldDiff.Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Progression? Progression
        {
            get => GetValue(ProgressionProperty) as Progression;
            set => SetValue(ProgressionProperty, value);
        }

        public static readonly DependencyProperty ProgressionProperty = DependencyProperty.Register(nameof(Progression), typeof(Progression), typeof(MainWindow));

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}