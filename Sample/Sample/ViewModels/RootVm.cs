using Sample.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sample.ViewModels
{
    public class RootVm : BaseViewModel
    {
        public RootVm()
        {
            CommandDefault = new Command(async () =>
            {
                var vm = new DefaultVm();
                await NavigationGoAsync(vm);
            });
            CommandCustomCells = new Command(async () =>
            {
                var vm = new CustomCellsVm();
                await NavigationGoAsync(vm);
            });
        }

        public ICommand CommandDefault { get; set; }
        public ICommand CommandCustomCells { get; set; }
        public override Page View { get; set; } = new Views.RootView();
    }
}
