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
            CommandDev = new Command(async () =>
            {
                var vm = new DevVm();
                await NavigationGoAsync(vm);
            });
            CommandCustomCells = new Command(async () =>
            {
                var vm = new CustomCellsDemoVm();
                await NavigationGoAsync(vm);
            });
            CommandStyles = new Command(async () =>
            {
                var vm = new StylesVm();
                await NavigationGoAsync(vm);
            });
            CommandTriggersDemo = new Command(async () =>
            {
                var vm = new TriggersDemoVm();
                await NavigationGoAsync(vm);
            });
            CommandEmbeddedDemo = new Command(async () =>
            {
                var vm = new EmbeddedDemoVm();
                await NavigationGoAsync(vm);
            });
        }

        public ICommand CommandDev { get; set; }
        public ICommand CommandCustomCells { get; set; }
        public ICommand CommandStyles { get; set; }
        public ICommand CommandTriggersDemo { get; set; }
        public ICommand CommandEmbeddedDemo { get; set; }
        public override Page View { get; set; } = new Views.RootView();
    }
}
