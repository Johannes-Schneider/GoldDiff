using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using GoldDiff.Shared.Properties;

namespace GoldDiff.Shared
{
    public abstract class BaseNotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected Dispatcher? EventDispatcher { get; }

        protected BaseNotifyPropertyChanged(Dispatcher? eventDispatcher = null)
        {
            EventDispatcher = eventDispatcher;
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (PropertyChanged == null)
            {
                return;
            }

            if (EventDispatcher == null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            else
            {
                EventDispatcher.Invoke(() => PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName)));
            }
        }
    }
}