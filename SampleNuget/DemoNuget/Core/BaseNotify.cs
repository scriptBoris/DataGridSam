using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DemoNuget.Core
{
    public abstract class BaseNotify : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propName));
        }
    }
}
