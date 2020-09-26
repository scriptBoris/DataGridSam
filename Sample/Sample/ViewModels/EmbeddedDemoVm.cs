using Sample.Core;
using Sample.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sample.ViewModels
{
    public class EmbeddedDemoVm : BaseViewModel
    {

        public EmbeddedDemoVm()
        {
            CommandSelectItem = new Command(ActionSelectedItem);
            CommandLongTap = new Command(ActionLongTap);

            CommandAddItem = new Command(ActionAddItem);
            CommandRemoveItem = new Command(ActionRemoveItem);

            CommandAddValue = new Command(ActionAddValue);
            CommandRemoveValue = new Command(ActionRemoveValue);

        }

        public override void OnAppearing()
        {
            LatencyInit();
        }

        #region Props
        public ObservableCollection<Vehicle> Items { get; set; }
        public Vehicle SelectedItem { get; set; }
        public int ItemsCount => Items?.Count ?? 0;
        public ICommand CommandSelectItem { get; set; }
        public ICommand CommandLongTap { get; set; }
        public ICommand CommandAddItem { get; set; }
        public ICommand CommandRemoveItem { get; set; }
        public ICommand CommandAddValue { get; set; }
        public ICommand CommandRemoveValue { get; set; }
        public override Page View { get; set; } = new Views.EmbeddedDemoView();
        #endregion

        #region Actions

        private void ActionSelectedItem(object obj)
        {
        }

        private void ActionLongTap(object obj)
        {
        }

        private void ActionAddValue(object obj)
        {
            if (SelectedItem != null)
            {
                SelectedItem.Engine.HorsePower += 10;
            }
        }

        private void ActionRemoveValue(object obj)
        {
            if (SelectedItem != null)
            {
                SelectedItem.Engine.HorsePower -= 10;
            }
        }

        private void ActionAddItem(object obj)
        {
            Items.Add(new Vehicle
            {
                Company = "Nisan",
                Name = "GTR",
                Engine = new Engine
                {
                    HorsePower = 270,
                    SerialNumber = "17EJ998",
                    Volume = 2.32f,
                },
            });
        }

        private void ActionRemoveItem(object obj)
        {
            if (Items != null && Items.Count > 0)
            {
                try
                {
                    Items.Remove(SelectedItem);
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion

        #region methods
        private async void LatencyInit()
        {
            await Task.Delay(100);
            var vehicles = DataCollector.GetVehicle();

            Items = new ObservableCollection<Vehicle>();
            foreach (var item in vehicles)
                Items.Add(item);

            SelectedItem = Items.FirstOrDefault();
        }
        #endregion methods
    }
}
