using System;
using System.ComponentModel;
using FlatXaml;
using FlatXaml.Model;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.View.Model;
using GoldDiff.View.Settings;

namespace GoldDiff.View.Controller
{
    public abstract class AbstractWindowController
    {
        private AbstractWindowViewModel Model { get; }

        protected AbstractWindowController(AbstractWindowViewModel? model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Model.PropertyAboutToChange += Model_OnPropertyAboutToChange;
            ViewSettings.Instance.PropertyChanged += ViewSettings_OnPropertyChanged;
            
            UpdateIsTopmost();
            UpdateDisplayTitleBar();
        }

        private void Model_OnPropertyAboutToChange(object? sender, PropertyAboutToChangeEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AbstractWindowViewModel.Game):
                {
                    UnsubscribeFromPropertyChangedEvent(e.OldValue as ViewModel, Game_OnPropertyChanged);
                    SubscribeToPropertyChangedEvent(e.NewValue as ViewModel, Game_OnPropertyChanged);
                    UpdateIsTopmost();
                    UpdateDisplayTitleBar();
                    break;
                }
            }
        }

        protected void SubscribeToPropertyChangedEvent(INotifyPropertyChanged? sender, PropertyChangedEventHandler handler)
        {
            if (sender == null)
            {
                return;
            }

            sender.PropertyChanged += handler;
        }

        protected void UnsubscribeFromPropertyChangedEvent(INotifyPropertyChanged? sender, PropertyChangedEventHandler handler)
        {
            if (sender == null)
            {
                return;
            }

            sender.PropertyChanged -= handler;
        }

        private void Game_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!ReferenceEquals(sender, Model.Game))
            {
                return;
            }

            switch (e.PropertyName)
            {
                case nameof(LoLGame.State):
                {
                    UpdateIsTopmost();
                    UpdateDisplayTitleBar();
                    break;
                }
            }
        }

        private void ViewSettings_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewSettings.WindowStayOnTop):
                {
                    UpdateIsTopmost();
                    break;
                }
                case nameof(ViewSettings.WindowDisplayTitleBar):
                {
                    UpdateDisplayTitleBar();
                    break;
                }
            }
        }

        private void UpdateIsTopmost()
        {
            Model.IsTopmost = ViewSettings.Instance.WindowStayOnTop switch
                              {
                                  StayOnTopType.Off => false,
                                  StayOnTopType.WhileGameIsRunning => Model.Game?.State switch
                                                              {
                                                                  null => false,
                                                                  LoLGameStateType.Undefined => false,
                                                                  LoLGameStateType.Ended => false,
                                                                  _ => true,
                                                              },
                                  StayOnTopType.Always => true,
                                  _ => throw new Exception($"Unknown {nameof(StayOnTopType)} {ViewSettings.Instance.WindowStayOnTop}!")
                              };
        }

        private void UpdateDisplayTitleBar()
        {
            Model.DisplayTitleBar = ViewSettings.Instance.WindowDisplayTitleBar switch
                                    {
                                        DisplayTitleBarType.Off => false,
                                        DisplayTitleBarType.WhileGameIsNotRunning => Model.Game?.State switch
                                                                                     {
                                                                                         LoLGameStateType.Started => false,
                                                                                         _ => true,
                                                                                     },
                                        DisplayTitleBarType.Always => true,
                                        _ => throw new Exception($"Unknown {nameof(DisplayTitleBarType)} {ViewSettings.Instance.WindowDisplayTitleBar}!"),
                                    };
        }
    }
}