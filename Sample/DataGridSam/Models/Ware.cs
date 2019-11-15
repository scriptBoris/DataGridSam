using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Sample.Models
{
    public class Ware : INotifyPropertyChanged
    {
        private bool isCompleted;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsCompleted { 
            get=>isCompleted; 
            set {
                isCompleted = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            }
        }
        public int Pos { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public float Weight { get; set; }
    }
}
