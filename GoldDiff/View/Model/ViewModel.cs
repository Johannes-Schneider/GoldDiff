using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace GoldDiff.View.Model
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private Dispatcher EventDispatcher { get; }

        protected ViewModel()
        {
            EventDispatcher = Application.Current?.Dispatcher ?? throw new Exception($"Unable to get the {nameof(Dispatcher)} of the current {nameof(Application)}!");
        }

        protected bool MutateVerboseIfNotNull<TPropertyType>(ref TPropertyType property, TPropertyType value, [CallerMemberName] string? propertyName = null)
        {
            if (value == null)
            {
                return false;
            }

            return MutateVerbose(ref property, value, propertyName);
        }

        protected bool MutateVerbose<TPropertyType>(ref TPropertyType property, TPropertyType value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<TPropertyType>.Default.Equals(property, value))
            {
                return false;
            }

            property = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (PropertyChanged == null)
            {
                return;
            }

            EventDispatcher.Invoke(() => PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName)));
        }
    }
}