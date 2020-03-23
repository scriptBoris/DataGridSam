using DemoNuget.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoNuget.Models
{
    public class Ware : BaseNotify
    {
        #region Support & XAML
        public bool IsProcess
        {
            get
            {
                if (Need == 0)
                    return false;

                if (Weight < Need && Weight > 0)
                    return true;
                else
                    return false;
            }
        }

        public bool IsCompleted
        {
            get
            {
                if (Need == 0)
                    return false;

                if (Weight == Need)
                    return true;
                else
                    return false;
            }
        }

        public bool IsOverload
        {
            get
            {
                if (Need == 0)
                    return false;

                if (Weight > Need)
                    return true;
                else
                    return false;
            }
        }
        #endregion Support & XAML

        private float need;
        private float weight;


        public string Name { get; set; }
        public float Price { get; set; }

        public float Need
        {
            get => need;
            set
            {
                need = value;
                OnPropertyChanged(nameof(Need));
                OnPropertyChanged(nameof(IsProcess));
                OnPropertyChanged(nameof(IsCompleted));
                OnPropertyChanged(nameof(IsOverload));
            }
        }

        public float Weight
        {
            get => weight;
            set
            {
                if (value < 0)
                    weight = 0.0f;
                else
                    weight = value;

                OnPropertyChanged(nameof(Weight));
                OnPropertyChanged(nameof(IsProcess));
                OnPropertyChanged(nameof(IsCompleted));
                OnPropertyChanged(nameof(IsOverload));
            }
        }
    }
}
