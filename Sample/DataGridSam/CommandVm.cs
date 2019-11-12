using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Sample
{
    public class CommandVm : ICommand
    {
        private readonly Action<object> action;

        public CommandVm(Action<object> action)
        {
            this.action = action;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action.Invoke(parameter);
        }
    }
}
