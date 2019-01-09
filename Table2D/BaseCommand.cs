using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjetSI
{
    public class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public void OnChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        private Func<bool> canExecute;
        private Action execute;

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }
        public BaseCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public BaseCommand(Action execute) : this(execute, () => true) { }
    }

    public class BaseCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute((T)parameter);
        }

        public void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
        private Func<T, bool> canExecute;
        private Action<T> execute;

        public BaseCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public BaseCommand(Action<T> execute) : this(execute, a => true) { }
    }
}
