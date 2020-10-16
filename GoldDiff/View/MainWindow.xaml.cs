using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using GoldDiff.View.Model;

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

        public MainWindow()
        {
            InitializeComponent();

            Model = new MainWindowViewModel();

            PrivateModel = Model;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}