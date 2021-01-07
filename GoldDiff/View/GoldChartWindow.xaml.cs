using System;
using System.Windows;
using System.Windows.Input;
using GoldDiff.View.Controller;
using GoldDiff.View.Model;

namespace GoldDiff.View
{
    public partial class GoldChartWindow : Window
    {
        private GoldChartWindowViewModel PrivateModel
        {
            get => GetValue(PrivateModelProperty) as GoldChartWindowViewModel ?? throw new NullReferenceException($"{nameof(PrivateModel)} must not be {null}!");
            set => SetValue(PrivateModelProperty, value ?? throw new ArgumentNullException(nameof(value)));
        }

        private static readonly DependencyProperty PrivateModelProperty = DependencyProperty.Register(nameof(PrivateModel), typeof(GoldChartWindowViewModel), typeof(GoldChartWindow));

        public GoldChartWindowViewModel Model { get; }

        public GoldChartWindowController Controller { get; }

        public GoldChartWindow()
        {
            InitializeComponent();
            
            Model = new GoldChartWindowViewModel();
            Controller = new GoldChartWindowController(Model);
            PrivateModel = Model;

            WindowControlBar.MouseLeftButtonDown += WindowControlBar_OnMouseLeftButtonDown;
        }

        private void WindowControlBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}