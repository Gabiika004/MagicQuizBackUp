using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MagicQuizDesktop.Commands
{
    internal class AsyncRelayCommand : ICommand
    {
        // Fields
        private readonly Func<object, Task> _executeActionAsync;
        private readonly Predicate<object> _canExecute;

        // Constructors
        public AsyncRelayCommand(Func<object, Task> executeActionAsync)
            : this(executeActionAsync, null)
        {
        }

        public AsyncRelayCommand(Func<object, Task> executeActionAsync, Predicate<object> canExecute)
        {
            _executeActionAsync = executeActionAsync ?? throw new ArgumentNullException(nameof(executeActionAsync));
            _canExecute = canExecute;
        }

        // Events
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // Methods
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public async void Execute(object parameter)
        {
            if (_executeActionAsync != null && CanExecute(parameter))
            {
                await _executeActionAsync(parameter);
            }
        }

        // Method to trigger the CanExecuteChanged event to re-evaluate the can execute logic
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
