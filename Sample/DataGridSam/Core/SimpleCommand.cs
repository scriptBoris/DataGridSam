using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Sample.Core
{
    public class SimpleCommand : ICommand
    {
        private readonly Action<object> action;

        public SimpleCommand(Action<object> action)
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
