using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using GoldDiff.Properties;
using GoldDiff.Shared.View.Command;
using GoldDiff.View.Model;
using GoldDiff.View.Settings;

namespace GoldDiff.View
{
    public partial class MainWindow : Window
    {
        private static readonly DependencyProperty PrivateModelProperty = DependencyProperty.Register(nameof(PrivateModel), typeof(MainWindowViewModel), MethodBase.GetCurrentMethod().DeclaringType);
        
        private MainWindowViewModel PrivateModel
        {
            get => GetValue(PrivateModelProperty) as MainWindowViewModel ?? throw new Exception($"{nameof(PrivateModel)} must not be {null}!");
            set => SetValue(PrivateModelProperty, value ?? throw new ArgumentNullException(nameof(value)));
        }

        public MainWindowViewModel Model { get; }
        
        public ICommand OpenViewSettingsCommand { get; }

        public MainWindow()
        {
            Model = new MainWindowViewModel();
            PrivateModel = Model;
            OpenViewSettingsCommand = new GenericCommand(_ => new ViewSettingsDialog {Owner = this}.ShowDialog());
            
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}