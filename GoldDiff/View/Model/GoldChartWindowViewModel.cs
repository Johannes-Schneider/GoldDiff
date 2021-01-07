using System;
using GoldDiff.View.Converter;
using LiveCharts;
using LiveCharts.Wpf;

namespace GoldDiff.View.Model
{
    public class GoldChartWindowViewModel : AbstractWindowViewModel
    {
        public SeriesCollection SeriesCollection { get; } = new()
                                                            {
                                                                new LineSeries
                                                                {
                                                                    Values = new ChartValues<int> {2000, 2500, 3500, -4500, -15000, -27000},
                                                                },
                                                            };

        public Func<double, string> GoldDifferenceAxisFormatter { get; } = val => IntToLoLGoldValueConverter.Convert((int) val);
    }
}