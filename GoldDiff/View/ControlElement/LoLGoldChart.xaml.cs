using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.View.SharedConverter;
using GoldDiff.Shared.View.SharedTheme;
using GoldDiff.View.Converter;
using GoldDiff.View.Settings;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Helpers;
using LiveCharts.Wpf;

namespace GoldDiff.View.ControlElement
{
    public partial class LoLGoldChart : UserControl
    {
    #region public dep props

        public ILoLGoldOwner? GoldOwnerBlueSide
        {
            get => GetValue(GoldOwnerBlueSideProperty) as ILoLGoldOwner;
            set => SetValue(GoldOwnerBlueSideProperty, value);
        }

        public static readonly DependencyProperty GoldOwnerBlueSideProperty = DependencyProperty.Register(nameof(GoldOwnerBlueSide), typeof(ILoLGoldOwner), typeof(LoLGoldChart),
                                                                                                          new PropertyMetadata(PropertyChangedCallback));

        public ILoLGoldOwner? GoldOwnerRedSide
        {
            get => GetValue(GoldOwnerRedSideProperty) as ILoLGoldOwner;
            set => SetValue(GoldOwnerRedSideProperty, value);
        }

        public static readonly DependencyProperty GoldOwnerRedSideProperty = DependencyProperty.Register(nameof(GoldOwnerRedSide), typeof(ILoLGoldOwner), typeof(LoLGoldChart),
                                                                                                         new PropertyMetadata(PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LoLGoldChart chart))
            {
                return;
            }

            switch (e.Property.Name)
            {
                case nameof(GoldOwnerBlueSide): // fallthrough
                case nameof(GoldOwnerRedSide):
                {
                    if (e.OldValue is ILoLGoldOwner oldValue)
                    {
                        oldValue.GoldSnapshotAdded -= chart.GoldOwner_OnGoldSnapshotAdded;
                    }

                    if (e.NewValue is ILoLGoldOwner newValue)
                    {
                        newValue.GoldSnapshotAdded += chart.GoldOwner_OnGoldSnapshotAdded;
                    }
                    
                    chart.Initialize();
                    break;
                }
            }
        }
        
    #endregion

    #region private dep props

        private SeriesCollection SeriesCollection
        {
            get => GetValue(SeriesCollectionProperty) as SeriesCollection ?? throw new NullReferenceException($"{nameof(SeriesCollection)} must not be {null}!");
            set => SetValue(SeriesCollectionProperty, value ?? throw new ArgumentNullException(nameof(value)));
        }

        private static readonly DependencyProperty SeriesCollectionProperty = DependencyProperty.Register(nameof(SeriesCollection), typeof(SeriesCollection), typeof(LoLGoldChart));

        private Func<double, string> XAxisLabelFormatter
        {
            get => GetValue(XAxisLabelFormatterProperty) as Func<double, string> ?? throw new NullReferenceException($"{nameof(XAxisLabelFormatter)} must not be {null}!");
            set => SetValue(XAxisLabelFormatterProperty, value ?? throw new ArgumentNullException(nameof(value)));
        }

        private static readonly DependencyProperty XAxisLabelFormatterProperty = DependencyProperty.Register(nameof(XAxisLabelFormatter), typeof(Func<double, string>), typeof(LoLGoldChart));

        private Func<double, string> YAxisLabelFormatter
        {
            get => GetValue(YAxisLabelFormatterProperty) as Func<double, string> ?? throw new NullReferenceException($"{nameof(YAxisLabelFormatter)} must not be {null}!");
            set => SetValue(YAxisLabelFormatterProperty, value ?? throw new ArgumentNullException(nameof(value)));
        }

        private static readonly DependencyProperty YAxisLabelFormatterProperty = DependencyProperty.Register(nameof(YAxisLabelFormatter), typeof(Func<double, string>), typeof(LoLGoldChart));

        private double MinimumXAxisMaximumValue
        {
            get => (double) GetValue(MinimumXAxisMaximumValueProperty);
            set => SetValue(MinimumXAxisMaximumValueProperty, value);
        }

        private static readonly DependencyProperty MinimumXAxisMaximumValueProperty = DependencyProperty.Register(nameof(MinimumXAxisMaximumValue), typeof(double), typeof(LoLGoldChart));

        private double MinimumYAxisMaximumValue
        {
            get => (double) GetValue(MinimumYAxisMaximumValueProperty);
            set => SetValue(MinimumYAxisMaximumValueProperty, value);
        }

        private static readonly DependencyProperty MinimumYAxisMaximumValueProperty = DependencyProperty.Register(nameof(MinimumYAxisMaximumValue), typeof(double), typeof(LoLGoldChart));

        private double XAxisMaximumValue
        {
            get => (double) GetValue(XAxisMaximumValueProperty);
            set => SetValue(XAxisMaximumValueProperty, value);
        }

        private static readonly DependencyProperty XAxisMaximumValueProperty = DependencyProperty.Register(nameof(XAxisMaximumValue), typeof(double), typeof(LoLGoldChart));

        private double YAxisMaximumValue
        {
            get => (double) GetValue(YAxisMaximumValueProperty);
            set => SetValue(YAxisMaximumValueProperty, value);
        }

        private static readonly DependencyProperty YAxisMaximumValueProperty = DependencyProperty.Register(nameof(YAxisMaximumValue), typeof(double), typeof(LoLGoldChart));

    #endregion

        public class GoldAdvantageChartPoint
        {
            public int TotalGoldAdvantage { get; set; }
            
            public int NonConsumableGoldAdvantage { get; set; }
            
            public int DisplayGoldAdvantage { get; set; }
            
            public TimeSpan GameTime { get; set; }
        }

        public const string BlueSideAdvantageSeries = nameof(BlueSideAdvantageSeries);
        public const string RedSideAdvantageSeries = nameof(RedSideAdvantageSeries);
        private const int HiddenValue = -100;

        private ChartValues<GoldAdvantageChartPoint> BlueSideAdvantages { get; } = new();
        private ChartValues<GoldAdvantageChartPoint> RedSideAdvantages { get; } = new();
        private DisplayGoldType _displayGoldType;
        private bool _minimumXValueExceeded;
        private bool _minimumYValueExceeded;

        public LoLGoldChart()
        {
            InitializeComponent();

            _displayGoldType = ViewSettings.Instance.DisplayGoldType;
            ViewSettings.Instance.PropertyChanged += ViewSettings_OnPropertyChanged;
            
            Initialize();
        }

        private void ViewSettings_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewSettings.DisplayGoldType) when ViewSettings.Instance.DisplayGoldType == DisplayGoldType.Total:
                {
                    _displayGoldType = DisplayGoldType.Total;
                    Dispatcher.InvokeAsync(() => UpdateChartValuesToTotalGold(BlueSideAdvantages.Count));
                    break;
                }
                case nameof(ViewSettings.DisplayGoldType) when ViewSettings.Instance.DisplayGoldType == DisplayGoldType.NonConsumable:
                {
                    _displayGoldType = DisplayGoldType.NonConsumable;
                    Dispatcher.InvokeAsync(() => UpdateChartValuesToNonConsumableGold(BlueSideAdvantages.Count));
                    break;
                }
                case nameof(ViewSettings.DisplayGoldType):
                {
                    throw new Exception($"Unknown {nameof(DisplayGoldType)} \"{ViewSettings.Instance.DisplayGoldType}\"!");
                }
            }
        }

        private void UpdateChartValuesToTotalGold(int currentCount)
        {
            foreach (var chartPoint in BlueSideAdvantages.Take(currentCount).Concat(RedSideAdvantages.Take(currentCount)))
            {
                chartPoint.DisplayGoldAdvantage = chartPoint.TotalGoldAdvantage;
            }
        }
        
        private void UpdateChartValuesToNonConsumableGold(int currentCount)
        {
            foreach (var chartPoint in BlueSideAdvantages.Take(currentCount).Concat(RedSideAdvantages.Take(currentCount)))
            {
                chartPoint.DisplayGoldAdvantage = chartPoint.NonConsumableGoldAdvantage;
            }
        }

        private void Initialize()
        {
            _minimumXValueExceeded = false;
            _minimumYValueExceeded = false;
            XAxisLabelFormatter = val => TimeSpanToStringConverter.Convert(TimeSpan.FromSeconds(val));
            YAxisLabelFormatter = val => IntToLoLGoldValueConverter.Convert((int) Math.Abs(val));
            MinimumXAxisMaximumValue = TimeSpan.FromMinutes(5).TotalSeconds;
            MinimumYAxisMaximumValue = 5000;
            XAxisMaximumValue = MinimumXAxisMaximumValue;
            YAxisMaximumValue = MinimumYAxisMaximumValue;

            InitializeSeriesCollection();
        }

        private void InitializeSeriesCollection()
        {
            InitializeChartValues(BlueSideAdvantages);
            InitializeChartValues(RedSideAdvantages);
            
            var config = Mappers.Xy<GoldAdvantageChartPoint>()
                                .X(snapshot => snapshot.GameTime.TotalSeconds)
                                .Y(snapshot => snapshot.DisplayGoldAdvantage);

            SeriesCollection = new SeriesCollection(config)
                               {
                                   CreateLineSeries(LoLTeamType.BlueSide),
                                   CreateLineSeries(LoLTeamType.RedSide),
                               };
        }

        private static void InitializeChartValues(NoisyCollection<GoldAdvantageChartPoint> values)
        {
            values.Clear();
            //
            // // add two points, to avoid additional optimization logic (see GoldOwner_OnGoldSnapshotAdded)
            // values.Add(new GoldAdvantageChartPoint
            //            {
            //                GameTime = TimeSpan.Zero,
            //                TotalGoldAdvantage = 0,
            //                NonConsumableGoldAdvantage = 0,
            //                DisplayGoldAdvantage = 0,
            //            });
            // values.Add(new GoldAdvantageChartPoint
            //            {
            //                GameTime = TimeSpan.Zero,
            //                TotalGoldAdvantage = 0,
            //                NonConsumableGoldAdvantage = 0,
            //                DisplayGoldAdvantage = 0,
            //            });
        }

        private LineSeries CreateLineSeries(LoLTeamType team)
        {
            return new()
                   {
                       Title = team switch
                              {
                                  LoLTeamType.BlueSide => BlueSideAdvantageSeries,
                                  LoLTeamType.RedSide => RedSideAdvantageSeries,
                                  _ => throw new ArgumentException($"Unknown {nameof(LoLTeamType)} {team}!"),
                              },
                       Values = team switch
                                {
                                    LoLTeamType.BlueSide => BlueSideAdvantages,
                                    LoLTeamType.RedSide => RedSideAdvantages,
                                    _ => throw new ArgumentException($"Unknown {nameof(LoLTeamType)} {team}!"),
                                },
                       Stroke = team switch
                                {
                                    LoLTeamType.BlueSide => Application.Current.Resources[GoldDiffSharedThemeKeys.LoLTeamBlueSideRegular] as Brush,
                                    LoLTeamType.RedSide => Application.Current.Resources[GoldDiffSharedThemeKeys.LoLTeamRedSideRegular] as Brush,
                                    _ => throw new ArgumentException($"Unknown {nameof(LoLTeamType)} {team}!"),
                                },
                       Fill = team switch
                              {
                                  LoLTeamType.BlueSide => Application.Current.Resources[GoldDiffSharedThemeKeys.LoLTeamBlueSideLight] as Brush,
                                  LoLTeamType.RedSide => Application.Current.Resources[GoldDiffSharedThemeKeys.LoLTeamRedSideLight] as Brush,
                                  _ => throw new ArgumentException($"Unknown {nameof(LoLTeamType)} {team}!"),
                              },
                       PointGeometry = null,
                       LineSmoothness = 0,
                   };
        }

        private void GoldOwner_OnGoldSnapshotAdded(object? sender, LoLGoldSnapshot e)
        {
            var newBlueSideSnapshots = (GoldOwnerBlueSide?.GoldSnapshots.Skip(BlueSideAdvantages.Count) ?? Enumerable.Empty<LoLGoldSnapshot>()).ToList();
            var newRedSideSnapshots = (GoldOwnerRedSide?.GoldSnapshots.Skip(RedSideAdvantages.Count) ?? Enumerable.Empty<LoLGoldSnapshot>()).ToList();

            if (newBlueSideSnapshots.Count != newRedSideSnapshots.Count)
            {
                return;
            }
            
            for (var i = 0; i < newBlueSideSnapshots.Count; ++i)
            {
                var blueSnapshot = newBlueSideSnapshots[i];
                var redSnapshot = newRedSideSnapshots[i];

                var totalDifference = blueSnapshot.TotalGold - redSnapshot.TotalGold;
                var nonConsumableDifference = blueSnapshot.NonConsumableGold - redSnapshot.NonConsumableGold;

                var blueSideTotalDifference = totalDifference >= 0 ? totalDifference : HiddenValue; 
                var blueSideNonConsumableDifference = nonConsumableDifference >= 0 ? nonConsumableDifference : HiddenValue;

                BlueSideAdvantages.Add(new GoldAdvantageChartPoint
                                       {
                                           GameTime = blueSnapshot.GameTime,
                                           TotalGoldAdvantage = blueSideTotalDifference,
                                           NonConsumableGoldAdvantage = blueSideNonConsumableDifference,
                                           DisplayGoldAdvantage = _displayGoldType == DisplayGoldType.Total ? blueSideTotalDifference : blueSideNonConsumableDifference,
                                       });

                var redSideTotalDifference = totalDifference < 0 ? -totalDifference : HiddenValue;
                var redSideNonConsumableDifference = nonConsumableDifference < 0 ? -nonConsumableDifference : HiddenValue;
                
                RedSideAdvantages.Add(new GoldAdvantageChartPoint
                                      {
                                          GameTime = redSnapshot.GameTime,
                                          TotalGoldAdvantage = redSideTotalDifference,
                                          NonConsumableGoldAdvantage = redSideNonConsumableDifference,
                                          DisplayGoldAdvantage = _displayGoldType == DisplayGoldType.Total ? redSideTotalDifference : redSideNonConsumableDifference,
                                      });
            }

            var lastTime = newBlueSideSnapshots.Last().GameTime;

            if (!_minimumXValueExceeded && lastTime.TotalSeconds >= MinimumXAxisMaximumValue)
            {
                // set x axis scaling to auto
                _minimumXValueExceeded = true;
                XAxisMaximumValue = double.NaN;
            }

            if (!_minimumYValueExceeded &&
                BlueSideAdvantages.Any(advantage => advantage.DisplayGoldAdvantage >= MinimumYAxisMaximumValue) ||
                RedSideAdvantages.Any(advantage => advantage.DisplayGoldAdvantage >= MinimumYAxisMaximumValue))
            {
                // set y axis scaling to auto
                _minimumYValueExceeded = true;
                YAxisMaximumValue = double.NaN;
            }
        }
    }
}