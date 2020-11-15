using System;
using System.Windows;
using System.Windows.Input;
using FlatXaml.Command;
using GoldDiff.View.Model;
using GoldDiff.View.Settings;

namespace GoldDiff.View
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel PrivateModel
        {
            get => (GetValue(PrivateModelProperty) as MainWindowViewModel)!;
            set => SetValue(PrivateModelProperty, value);
        }

        private static readonly DependencyProperty PrivateModelProperty = DependencyProperty.Register(nameof(PrivateModel), typeof(MainWindowViewModel), typeof(MainWindow));

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