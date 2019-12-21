using Sample.Core;
using Sample.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Sample.ViewModels
{
    public class WareDetailVm : BaseNotify
    {
        private Ware ware;
        private readonly Page view;

        [Obsolete("For XAML")]
        public WareDetailVm()
        {
        }

        public WareDetailVm(Page view, Ware openWare)
        {
            this.view = view;
            Ware = openWare;
            Items = new ObservableCollection<Ware>
            {
                openWare
            };
        }

        public Ware Ware
        {
            get => ware;
            set
            {
                ware = value;
                OnPropertyChanged(nameof(Ware));
            }
        }

        public ObservableCollection<Ware> Items { get; set; }
    }
}
