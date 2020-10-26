using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.View.Controller;
using GoldDiff.View.Model;

namespace GoldDiff.View
{
    public partial class GoldDifferenceWindow : Window
    {
        public const string ActivePlayerOnBlueSideBackground = nameof(ActivePlayerOnBlueSideBackground);
        public const string ActivePlayerOnRedSideBackground = nameof(ActivePlayerOnRedSideBackground);

        private static readonly DependencyProperty PrivateModelProperty =
            DependencyProperty.Register(nameof(PrivateModel), typeof(GoldDifferenceWindowViewModel), MethodBase.GetCurrentMethod().DeclaringType);

        private GoldDifferenceWindowViewModel PrivateModel
        {
            get => GetValue(PrivateModelProperty) as GoldDifferenceWindowViewModel ?? throw new NullReferenceException($"{nameof(PrivateModel)} must not be {null}!");
            set => SetValue(PrivateModelProperty, value ?? throw new ArgumentNullException(nameof(value)));
        }

        public GoldDifferenceWindowViewModel Model { get; }

        public GoldDifferenceWindowController Controller { get; }

        public GoldDifferenceWindow(LoLGame? game)
        {
            InitializeComponent();

            Model = new GoldDifferenceWindowViewModel
                    {
                        ActivePlayerOnBlueSideBackground = Resources[ActivePlayerOnBlueSideBackground] as Brush,
                        ActivePlayerOnRedSideBackground = Resources[ActivePlayerOnRedSideBackground] as Brush,
                        InactivePlayerBackground = null,
                    };
            Controller = new GoldDifferenceWindowController(Model, game);

            PrivateModel = Model;
        }
    }
}