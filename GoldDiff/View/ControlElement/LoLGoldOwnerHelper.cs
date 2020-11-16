using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using FlatXaml.Annotations;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.View.Settings;

namespace GoldDiff.View.ControlElement
{
    public class LoLGoldOwnerHelper : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ILoLGoldOwner? GoldOwner
        {
            get => GetValue(GoldOwnerProperty) as ILoLGoldOwner;
            set => SetValue(GoldOwnerProperty, value);
        }
        
        public static readonly DependencyProperty GoldOwnerProperty = DependencyProperty.Register(nameof(GoldOwner), typeof(ILoLGoldOwner), typeof(LoLGoldOwnerHelper),
                                                                                                  new PropertyMetadata(PropertyChangedCallback));

        public int Gold
        {
            get => (int) GetValue(GoldProperty);
            set => SetValue(GoldProperty, value);
        }
        
        public static readonly DependencyProperty GoldProperty = DependencyProperty.Register(nameof(Gold), typeof(int), typeof(LoLGoldOwnerHelper),
                                                                                             new PropertyMetadata(PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is LoLGoldOwnerHelper goldOwnerHelper))
            {
                return;
            }

            if (e.Property.Name.Equals(nameof(GoldOwner)))
            {
                if (e.OldValue is ILoLGoldOwner oldValue)
                {
                    oldValue.PropertyChanged -= goldOwnerHelper.GoldOwner_OnPropertyChanged;
                }

                if (e.NewValue is ILoLGoldOwner newValue)
                {
                    newValue.PropertyChanged += goldOwnerHelper.GoldOwner_OnPropertyChanged;
                }
                
                goldOwnerHelper.UpdateGold();
            }
            
            goldOwnerHelper.OnPropertyChanged(e.Property.Name);
        }

        public LoLGoldOwnerHelper()
        {
            ViewSettings.Instance.PropertyChanged += ViewSettings_OnPropertyChanged;
            UpdateGold();
        }

        private void ViewSettings_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName)
                || e.PropertyName.Equals(nameof(ViewSettings.DisplayGoldType)))
            {
                UpdateGold();
            }
        }
        
        private void GoldOwner_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateGold();
        }
        
        private void UpdateGold()
        {
            Gold = ViewSettings.Instance.DisplayGoldType switch
                   {
                       DisplayGoldType.Total => GoldOwner?.TotalGold ?? 0,
                       DisplayGoldType.NonConsumable => GoldOwner?.NonConsumableGold ?? 0,
                       _ => throw new Exception($"Unknown {nameof(DisplayGoldType)} {ViewSettings.Instance.DisplayGoldType}!")
                   };
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (PropertyChanged == null)
            {
                return;
            }

            if (Dispatcher == null || Dispatcher.CheckAccess())
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            else
            {
                Dispatcher.Invoke(() => PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName)));
            }
        }
    }
}