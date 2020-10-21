using System;
using System.Windows;
using System.Windows.Threading;

namespace GoldDiff.Shared.View.Model
{
    public abstract class ViewModel : BaseNotifyPropertyChanged
    {
        protected ViewModel() : base(Application.Current?.Dispatcher ?? throw new Exception($"Unable to get the {nameof(Dispatcher)} of the current {nameof(Application)}!")) { }
    }
}