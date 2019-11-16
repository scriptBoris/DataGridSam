using Sample.Core;
using Sample.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Sample.ViewModels
{
    public class WareDetailVm : BaseNotify
    {
        private Ware ware;
        private readonly Page view;

        public WareDetailVm(Page view, Ware openWare)
        {
            this.view = view;
            Ware = openWare;
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
    }
}
