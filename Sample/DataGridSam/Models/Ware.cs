using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Sample.Models
{
    public class Ware : INotifyPropertyChanged
    {
        private bool isCompleted;
        private float weight;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsCompleted { 
            get=>isCompleted; 
            set {
                isCompleted = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            }
        }
        public bool IsFinish => (Weight >= Need);
        public int Pos { get; set; }
        public string Name { get; set; }
        public float Need { get; set; } = 100;
        public float Price { get; set; }
        public float Weight { 
            get => weight;
            set {
                weight = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFinish)));
            }
        }
    }
}
