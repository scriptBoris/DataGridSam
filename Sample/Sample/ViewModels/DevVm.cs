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
    public class DevVm : BaseViewModel
    {
        private Ware lastWare;

        public DevVm()
        {
            CommandSelectItem = new Command(ActionSelectedItem);
            CommandLongTap = new Command(ActionLongTap);
            CommandOpenWare = new Command(ActionOpenWare);
            CommandAddItem = new Command(ActionAddItem);
            CommandInsertItem = new Command(ActionInsertItem);
            CommandAddItems = new Command(ActionAddItems);
            CommandRemoveItem = new Command(ActionRemoveItem);
            CommandAddWeight = new Command(ActionAddWeight);
            CommandRemoveWeight = new Command(ActionRemoveWeight);

            Items = DataCollector.GetWares();
            SelectedItem = Items.FirstOrDefault();
        }

        #region Props
        public ObservableCollection<Ware> Items { get; set; }
        public Ware SelectedItem { get; set; }
        public int Index { get; set; }
        public int ItemsCount => Items?.Count ?? 0;
        public ICommand CommandSelectItem { get; set; }
        public ICommand CommandLongTap { get; set; }
        public ICommand CommandAddWeight { get; set; }
        public ICommand CommandRemoveWeight { get; set; }
        public ICommand CommandOpenWare { get; set; }
        public ICommand CommandAddItem { get; set; }
        public ICommand CommandInsertItem { get; set; }
        public ICommand CommandAddItems { get; set; }
        public ICommand CommandRemoveItem { get; set; }
        public override Page View { get; set; } = new Views.DevView();
        #endregion

        #region Actions
        private void ActionSelectedItem(object param)
        {
            if (param is Ware ware)
            {
                if (lastWare != null)
                    lastWare.IsSelected = false;
                lastWare = ware;
                lastWare.IsSelected = true;

                int pos = Items.IndexOf(ware);
                View.DisplayAlert("Select", $"You are selected {pos} {ware.Name}", "OK");
            }
        }

        private void ActionLongTap(object param)
        {
            if (param is Ware ware)
            {
                SelectedItem = ware;
                int pos = Items.IndexOf(ware);
                View.DisplayAlert("Long tap", $"You are selected {pos} {ware.Name}", "OK");
            }
        }

        private void ActionOpenWare(object param)
        {
            var edit = new Views.WareEdit();
            edit.BindingContext = new ViewModels.WareDetailVm(edit, SelectedItem);
            View.Navigation.PushAsync(edit);
        }

        private void ActionAddItem(object obj)
        {
            Items.Add(new Ware
            {
                Name = "Food jar lcc-a",
                Price = 159.56f,
                Weight = 0.0f,
                Need = 100,
            });

            (View as Views.DevView).ScrollToIndex(Items.Count);
        }

        private void ActionInsertItem(object obj)
        {
            if (Index > Items.Count - 1)
                Index = Items.Count - 1;
            else if (Index < 0)
                Index = 0;

            Items.Insert(Index, new Ware
            {
                Name = "Food jar lcc-a",
                Price = 159.56f,
                Weight = 0.0f,
                Need = 100,
            });
            (View as Views.DevView).ScrollToIndex(Index + 1);
        }

        private void ActionAddItems(object obj)
        {
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

        private void ActionAddWeight(object obj)
        {
            if (SelectedItem != null)
            {
                SelectedItem.Weight += 10;
            }
        }

        private void ActionRemoveWeight(object obj)
        {
            if (SelectedItem != null)
            {
                var res = SelectedItem.Weight - 10;
                if (res < 0)
                    SelectedItem.Weight = 0;
                else
                    SelectedItem.Weight = res;
            }
        }
        #endregion
    }
}
