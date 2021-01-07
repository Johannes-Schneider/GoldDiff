using System;
using GoldDiff.View.Model;

namespace GoldDiff.View.Controller
{
    public class GoldChartWindowController : AbstractWindowController
    {
        public GoldChartWindowViewModel Model { get; }

        public GoldChartWindowController(GoldChartWindowViewModel? model) : base(model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }
    }
}