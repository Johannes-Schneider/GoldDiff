using System;
using System.Windows.Input;

namespace GoldDiff.Shared.View.Command
{
    public class GenericCommand : ICommand
    {
        public delegate bool CanExecuteDelegate(object? parameter);

        public delegate void ExecuteDelegate(object? parameter);

        private CanExecuteDelegate CanExecuteCallback { get; }
        private ExecuteDelegate ExecuteCallback { get; }

        public GenericCommand(ExecuteDelegate? executeDelegate) : this(_ => true, executeDelegate) { }

        public GenericCommand(CanExecuteDelegate? canExecuteDelegate, ExecuteDelegate? executeDelegate)
        {
            CanExecuteCallback = canExecuteDelegate ?? throw new ArgumentNullException(nameof(canExecuteDelegate));
            ExecuteCallback = executeDelegate ?? throw new ArgumentNullException(nameof(executeDelegate));
        }

        public bool CanExecute(object? parameter)
        {
            return CanExecuteCallback.Invoke(parameter);
        }

        public void Execute(object? parameter)
        {
            ExecuteCallback.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}