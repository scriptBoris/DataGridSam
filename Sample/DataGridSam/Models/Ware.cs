using Sample.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Sample.Models
{
    public class Ware : BaseNotify
    {
        private string name;
        private float need = 100f;
        private int pos;
        private bool isCompleted;
        private float weight;
        private float price;

        public bool IsCanCompleted => (Weight >= Need);

        public bool IsCompleted { 
            get=>isCompleted; 
            set {
                isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
            }
        }
        public int Pos { 
            get => pos;
            set {
                pos = value;
                OnPropertyChanged(nameof(Pos));
            }
        }
        public string Name { get => name;
            set {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public float Need { 
            get => need; 
            set { need = value;
                OnPropertyChanged(nameof(Need));
                OnPropertyChanged(nameof(IsCanCompleted));
            }
        }

        public float Price {
            get => price;
            set {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
        public float Weight { 
            get => weight;
            set {
                weight = value;
                OnPropertyChanged(nameof(Weight));
                OnPropertyChanged(nameof(IsCanCompleted));
            }
        }
    }
}
