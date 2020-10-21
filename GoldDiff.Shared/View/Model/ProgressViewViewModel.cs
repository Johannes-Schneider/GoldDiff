using System;
using GoldDiff.Shared.Properties;

namespace GoldDiff.Shared.View.Model
{
    public class ProgressViewViewModel : ViewModel
    {
        private const double UpdateProgressThreshold = 0.005d;
        
        private string _title = string.Empty;

        [NotNull]
        public string Title
        {
            get => _title;
            set => MutateVerboseIfNotNull(ref _title, value);
        }

        private int _totalNumberOfSteps = 1;

        public int TotalNumberOfSteps
        {
            get => _totalNumberOfSteps;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (!MutateVerbose(ref _totalNumberOfSteps, value))
                {
                    return;
                }
                
                UpdateTotalProgress();
            }
        }

        private int _currentStepNumber;

        public int CurrentStepNumber
        {
            get => _currentStepNumber;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (!MutateVerbose(ref _currentStepNumber, value))
                {
                    return;
                }
                
                UpdateTotalProgress();
            }
        }

        private string _currentStepDescription = string.Empty;

        [NotNull]
        public string CurrentStepDescription
        {
            get => _currentStepDescription;
            set => MutateVerboseIfNotNull(ref _currentStepDescription, value);
        }

        private double _currentStepProgress;

        public double CurrentStepProgress
        {
            get => _currentStepProgress;
            set
            {
                if (value < 0.0d || value > 1.0d)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (!value.Equals(1.0d) && Math.Abs(value - CurrentStepProgress) < UpdateProgressThreshold)
                {
                    return;
                }

                if (!MutateVerbose(ref _currentStepProgress, value))
                {
                    return;
                }
                
                UpdateTotalProgress();
            }
        }

        private double _totalProgress;

        public double TotalProgress
        {
            get => _totalProgress;
            private set
            {
                if (!value.Equals(1.0d) && Math.Abs(TotalProgress - value) < UpdateProgressThreshold)
                {
                    return;
                }
                MutateVerbose(ref _totalProgress, value);
            }
        }

        private void UpdateTotalProgress()
        {
            var totalProgressPerStep = 1.0d / TotalNumberOfSteps;
            TotalProgress = Math.Max(0, CurrentStepNumber - 1) * totalProgressPerStep + CurrentStepProgress * totalProgressPerStep;
        }
    }
}