using System;
using System.ComponentModel;
using FlatXaml;
using FlatXaml.Model;
using GoldDiff.View.Model;

namespace GoldDiff.View.Controller
{
    public abstract class AbstractWindowController
    {
        private AbstractWindowViewModel Model { get; }

        protected AbstractWindowController(AbstractWindowViewModel? model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Model.PropertyChanged += Model_OnPropertyChanged;
            Model.PropertyAboutToChange += Model_OnPropertyAboutToChange;
        }
        
        private void Model_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!ReferenceEquals(sender, Model))
            {
                return;
            }
            
            ModelOnPropertyChanged(e);
        }
        
        protected virtual void ModelOnPropertyChanged(PropertyChangedEventArgs e) { }

        private void Model_OnPropertyAboutToChange(object? sender, PropertyAboutToChangeEventArgs e)
        {
            if (!ReferenceEquals(sender, Model))
            {
                return;
            }
            
            switch (e.PropertyName)
            {
                case nameof(AbstractWindowViewModel.Game):
                {
                    Unsubscribe(e.OldValue as ViewModel, Game_OnPropertyChanged);
                    Unsubscribe(e.OldValue as ViewModel, Game_OnPropertyAboutToChange);
                    Subscribe(e.NewValue as ViewModel, Game_OnPropertyChanged);
                    Subscribe(e.NewValue as ViewModel, Game_OnPropertyAboutToChange);
                    break;
                }
            }
            
            ModelOnPropertyIsAboutToChange(e);
        }
        
        protected virtual void ModelOnPropertyIsAboutToChange(PropertyAboutToChangeEventArgs e) { }
        
        private void Game_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!ReferenceEquals(sender, Model.Game))
            {
                return;
            }

            OnGamePropertyChanged(e);
        }
        
        protected virtual void OnGamePropertyChanged(PropertyChangedEventArgs e) { }

        private void Game_OnPropertyAboutToChange(object? sender, PropertyAboutToChangeEventArgs e)
        {
            if (!ReferenceEquals(sender, Model.Game))
            {
                return;
            }
            
            GameOnPropertyAboutToChange(e);
        }
        
        protected virtual void GameOnPropertyAboutToChange(PropertyAboutToChangeEventArgs e) { }

        protected void Subscribe(INotifyPropertyChanged? sender, PropertyChangedEventHandler handler)
        {
            if (sender == null)
            {
                return;
            }

            sender.PropertyChanged += handler;
        }

        protected void Subscribe(INotifyPropertyAboutToChange? sender, EventHandler<PropertyAboutToChangeEventArgs> handler)
        {
            if (sender == null)
            {
                return;
            }

            sender.PropertyAboutToChange += handler;
        }

        protected void Unsubscribe(INotifyPropertyChanged? sender, PropertyChangedEventHandler handler)
        {
            if (sender == null)
            {
                return;
            }

            sender.PropertyChanged -= handler;
        }

        protected void Unsubscribe(INotifyPropertyAboutToChange? sender, EventHandler<PropertyAboutToChangeEventArgs> handler)
        {
            if (sender == null)
            {
                return;
            }

            sender.PropertyAboutToChange -= handler;
        }
    }
}