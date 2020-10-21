using System;
using System.Windows;
using GoldDiff.Shared.View.Model;

namespace GoldDiff.Shared.View.Controller
{
    public class ProgressViewController
    {
        public EventHandler? Finished;
        
        public ProgressViewViewModel Model { get; }

        public ProgressViewController(ProgressViewViewModel? model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }
        
        public void StartNextStep(string? stepDescription)
        {
            Model.CurrentStepDescription = stepDescription ?? throw new ArgumentNullException(nameof(stepDescription));
            Model.CurrentStepNumber = Math.Min(Model.TotalNumberOfSteps, Model.CurrentStepNumber + 1);
            Model.CurrentStepProgress = 0.0d;
        }

        public void Done()
        {
            Model.CurrentStepDescription = string.Empty;
            Model.CurrentStepNumber = Model.TotalNumberOfSteps;
            Model.CurrentStepProgress = 1.0d;
            
            Application.Current?.Dispatcher.Invoke(() => Finished?.Invoke(this, EventArgs.Empty));
        }
    }
}