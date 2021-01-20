using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using FlatXaml;
using FlatXaml.Model;
using GoldDiff.LeagueOfLegends.Game;
using LiveCharts;
using LiveCharts.Wpf;

namespace GoldDiff.View.ControlElement
{
    public partial class LoLGoldChartTooltip : IChartTooltip
    {
        private class GoldAdvantage : ViewModel, ILoLGoldOwner
        {
            public event EventHandler<LoLGoldSnapshot>? GoldSnapshotAdded;

            private int _totalGold;

            public int TotalGold
            {
                get => _totalGold;
                set => MutateVerbose(ref _totalGold, value);
            }

            private int _nonConsumableGold;

            public int NonConsumableGold
            {
                get => _nonConsumableGold;
                set => MutateVerbose(ref _nonConsumableGold, value);
            }
            
            public IEnumerable<LoLGoldSnapshot> GoldSnapshots => Enumerable.Empty<LoLGoldSnapshot>();
        }
        
    #region private dep props

        private TimeSpan GameTime
        {
            get => (TimeSpan) GetValue(GameTimeProperty);
            set => SetValue(GameTimeProperty, value);
        }
        
        private static readonly DependencyProperty GameTimeProperty = DependencyProperty.Register(nameof(GameTime), typeof(TimeSpan), typeof(LoLGoldChartTooltip));

        private GoldAdvantage BlueSideAdvantage
        {
            get => GetValue(BlueSideAdvantageProperty) as GoldAdvantage ?? throw new NullReferenceException($"{nameof(BlueSideAdvantage)} must not be {null}!");
            set => SetValue(BlueSideAdvantageProperty, value ?? throw new ArgumentNullException(nameof(value)));
        }
        
        private static readonly DependencyProperty BlueSideAdvantageProperty = DependencyProperty.Register(nameof(BlueSideAdvantage), typeof(GoldAdvantage), typeof(LoLGoldChartTooltip));
        
        private GoldAdvantage RedSideAdvantage
        {
            get => GetValue(RedSideAdvantageProperty) as GoldAdvantage ?? throw new NullReferenceException($"{nameof(RedSideAdvantage)} must not be {null}!");
            set => SetValue(RedSideAdvantageProperty, value ?? throw new ArgumentNullException(nameof(value)));
        }
        
        private static readonly DependencyProperty RedSideAdvantageProperty = DependencyProperty.Register(nameof(RedSideAdvantage), typeof(GoldAdvantage), typeof(LoLGoldChartTooltip));

    #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        private TooltipData _data = null!;

        public TooltipData Data
        {
            get => _data;
            set
            {
                if (!this.MutateVerboseIfNotNull(ref _data, value, PropertyChanged, Dispatcher))
                {
                    return;
                }
                
                UpdateGoldAdvantage();
            }
        }

        private TooltipSelectionMode? _selectionMode;

        public TooltipSelectionMode? SelectionMode
        {
            get => _selectionMode;
            set => this.MutateVerboseIfNotNull(ref _selectionMode, value, PropertyChanged, Dispatcher);
        }

        public LoLGoldChartTooltip()
        {
            InitializeComponent();

            GameTime = TimeSpan.Zero;
            BlueSideAdvantage = new GoldAdvantage();
            RedSideAdvantage = new GoldAdvantage();
        }

        private void UpdateGoldAdvantage()
        {
            var blueSideAdvantage = Data.Points.FirstOrDefault(point => point.Series.Title == LoLGoldChart.BlueSideAdvantageSeries);
            var redSideAdvantage = Data.Points.FirstOrDefault(point => point.Series.Title == LoLGoldChart.RedSideAdvantageSeries);

            if (!(blueSideAdvantage?.ChartPoint.Instance is LoLGoldChart.GoldAdvantageChartPoint blueSideSnapshot) || 
                !(redSideAdvantage?.ChartPoint.Instance is LoLGoldChart.GoldAdvantageChartPoint redSideSnapshot))
            {
                return;
            }

            GameTime = blueSideSnapshot.GameTime;
            BlueSideAdvantage.TotalGold = Math.Max(0, blueSideSnapshot.TotalGoldAdvantage);
            BlueSideAdvantage.NonConsumableGold = Math.Max(0, blueSideSnapshot.NonConsumableGoldAdvantage);
            RedSideAdvantage.TotalGold = Math.Max(0, redSideSnapshot.TotalGoldAdvantage);
            RedSideAdvantage.NonConsumableGold = Math.Max(0, redSideSnapshot.NonConsumableGoldAdvantage);
        }
    }
}