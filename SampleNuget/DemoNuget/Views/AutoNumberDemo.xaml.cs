using DemoNuget.Core;
using DemoNuget.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoNuget.Views
{
    public partial class AutoNumberDemo : ContentPage
    {
        public AutoNumberDemo()
        {
            InitializeComponent();
            BindingContext = this;

            CommandLongTap = new Command(ActionLongTap);
            CommandAddWare = new Command(ActionAddWare);
            Items = DataCollector.GetWares();
        }

        public ICommand CommandLongTap { get; set; }
        public ICommand CommandAddWare { get; set; }
        public ObservableCollection<Ware> Items { get; set; }

        private async void ActionLongTap(object param)
        {
            if (param is Ware ware)
            {
                const string v1 = "Add weight";
                const string v2 = "Remove weight"; 
                const string v3 = "Clear weight";
                const string v4 = "Delete";
                const string v5 = "Cancel";

                string res = await DisplayActionSheet("Select action", null, null, new string[]
                {
                    v1, v2, v3, v4, v5
                });

                if (res == v1)
                    AddWeight(ware, 10.0f);
                else if (res == v2)
                    AddWeight(ware, -10.0f);
                else if (res == v3)
                    AddWeight(ware, -1000000);
                else if (res == v4)
                    DeleteUser(ware);
            }
        }

        private void ActionAddWare(object param)
        {
            var ware = DataCollector.GenerateWare();
            Items.Add(ware);
        }

        private async void DeleteUser(Ware item)
        {
            bool res = await DisplayAlert("Delete", $"Delete ware {item.Name}?",
                "Delete", "Cancel");

            if (res)
                Items.Remove(item);
        }

        private void AddWeight(Ware item, float addWeght)
        {
            item.Weight += addWeght;
        }
    }
}