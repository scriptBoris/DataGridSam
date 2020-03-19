using Sample.Core;
using Sample.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sample.ViewModels
{
    public class EmbeddedDemoVm : BaseViewModel
    {
        private Ware lastWare;

        public EmbeddedDemoVm()
        {
            CommandSelectItem = new Command(ActionSelectedItem);
            CommandLongTap = new Command(ActionLongTap);
            CommandRemoveItem = new Command(ActionRemoveItem);
            CommandAddValue = new Command(ActionAddValue);
            CommandRemoveValue = new Command(ActionRemoveValue);

            Items = DataCollector.GetVehicle();
            SelectedItem = Items.FirstOrDefault();
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
    }
}
