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
        private float need;
        private int pos;
        private float weight;
        private float price;

        public bool IsProcess {
            get {
                if (Need == 0)
                    return false;

                if (Weight < Need && Weight > 0)
                    return true;
                else
                    return false;
            }
        }

        public bool IsCompleted {
            get {
                if (Need == 0)
                    return false;

                if (Weight == Need)
                    return true;
                else
                    return false;
            }
        }

        public bool IsOverload {
            get {
                if (Need == 0)
                    return false;

                if (Weight > Need)
                    return true;
                else
                    return false;
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
                OnPropertyChanged(nameof(IsProcess));
                OnPropertyChanged(nameof(IsCompleted));
                OnPropertyChanged(nameof(IsOverload));
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
                OnPropertyChanged(nameof(IsProcess));
                OnPropertyChanged(nameof(IsCompleted));
                OnPropertyChanged(nameof(IsOverload));
            }
        }
    }
}
