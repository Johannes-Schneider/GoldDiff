using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using GoldDiff.Annotations;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.View.Settings;

namespace GoldDiff.View.ControlElement
{
    public class GoldComparisonHelper : FrameworkElement, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public static readonly DependencyProperty GoldOwnerBlueSideProperty =  DependencyProperty.Register(nameof(GoldOwnerBlueSide), typeof(ILoLGoldOwner), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                           new PropertyMetadata(PropertyChangedCallback));
        
        public static readonly DependencyProperty GoldOwnerRedSideProperty =  DependencyProperty.Register(nameof(GoldOwnerRedSide), typeof(ILoLGoldOwner), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                           new PropertyMetadata(PropertyChangedCallback));
        
        public static readonly DependencyProperty GoldBlueSideProperty = DependencyProperty.Register(nameof(GoldBlueSide), typeof(int), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                     new PropertyMetadata(PropertyChangedCallback));
        
        public static readonly DependencyProperty GoldRedSideProperty = DependencyProperty.Register(nameof(GoldRedSide), typeof(int), MethodBase.GetCurrentMethod().DeclaringType,
                                                                                                    new PropertyMetadata(PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is GoldComparisonHelper goldComparisonHelper))
            {
                return;
            }

            if (e.Property.Name.Equals(nameof(GoldOwnerBlueSide)) || e.Property.Name.Equals(nameof(GoldOwnerRedSide)))
            {
                goldComparisonHelper.OnGoldOwnerChanged(e.OldValue as ILoLGoldOwner, e.NewValue as ILoLGoldOwner);
            }
            
            goldComparisonHelper.OnPropertyChanged(e.Property.Name);
        }

        public ILoLGoldOwner? GoldOwnerBlueSide
        {
            get => GetValue(GoldOwnerBlueSideProperty) as ILoLGoldOwner;
            set => SetValue(GoldOwnerBlueSideProperty, value);
        }

        public ILoLGoldOwner? GoldOwnerRedSide
        {
            get => GetValue(GoldOwnerRedSideProperty) as ILoLGoldOwner;
            set => SetValue(GoldOwnerRedSideProperty, value);
        }

        public int GoldBlueSide
        {
            get => (int) GetValue(GoldBlueSideProperty);
            private set => SetValue(GoldBlueSideProperty, value);
        }

        public int GoldRedSide
        {
            get => (int) GetValue(GoldRedSideProperty);
            private set => SetValue(GoldRedSideProperty, value);
        }

        public GoldComparisonHelper()
        {
            ViewSettings.Instance.PropertyChanged += ViewSettings_OnPropertyChanged;
            UpdateGold();
        }

        private void ViewSettings_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(ViewSettings.DisplayGoldType)))
            {
                UpdateGold();
            }
        }

        private void OnGoldOwnerChanged(ILoLGoldOwner? oldValue, ILoLGoldOwner? newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= GoldOwner_OnPropertyChanged;
            }

            if (newValue != null)
            {
                newValue.PropertyChanged += GoldOwner_OnPropertyChanged;
            }
            
            UpdateGold();
        }

        private void GoldOwner_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.PropertyName) 
                || e.PropertyName.Equals(nameof(ILoLGoldOwner.TotalGold)) 
                || e.PropertyName.Equals(nameof(ILoLGoldOwner.NonConsumableGold)))
            {
                UpdateGold();
            }
        }

        private void UpdateGold()
        {
            switch (ViewSettings.Instance.DisplayGoldType)
            {
                case DisplayGoldType.Total:
                {
                    GoldBlueSide = GoldOwnerBlueSide?.TotalGold ?? 0;
                    GoldRedSide = GoldOwnerRedSide?.TotalGold ?? 0;
                    break;
                }
                case DisplayGoldType.NonConsumable:
                {
                    GoldBlueSide = GoldOwnerBlueSide?.NonConsumableGold ?? 0;
                    GoldRedSide = GoldOwnerRedSide?.NonConsumableGold ?? 0;
                    break;
                }
                default:
                {
                    throw new Exception($"Unknown {nameof(DisplayGoldType)} {ViewSettings.Instance.DisplayGoldType}!");
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (PropertyChanged == null)
            {
                return;
            }
            
            Dispatcher.Invoke(() => PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName)));
        }
    }
}