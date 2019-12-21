using Sample.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Sample.Models
{
    public class Ware : BaseNotify
    {
        #region Support & XAML
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
        #endregion

        public int Pos { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }

        private float need;
        public float Need { 
            get => need; 
            set { need = value;
                OnPropertyChanged(nameof(Need));
                OnPropertyChanged(nameof(IsProcess));
                OnPropertyChanged(nameof(IsCompleted));
                OnPropertyChanged(nameof(IsOverload));
            }
        }

        private float weight;
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