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
    public class MainPageVm : BaseNotify
    {
        private readonly Page view;

        [Obsolete("Only for IntelliSense XAML helper")]
        public MainPageVm() { }
        public MainPageVm(Page view)
        {
            this.view = view;
            CommandSelectItem = new SimpleCommand(ActionSelectedItem);
            CommandDoubleClick = new SimpleCommand(ActionDoubleClick);
            CommandLongTap = new SimpleCommand(ActionLongTap);
            CommandOpenWare = new SimpleCommand(ActionOpenWare);
            CommandAddItem = new SimpleCommand(ActionAddItem);
            CommandAddItems = new SimpleCommand(ActionAddItems);
            CommandRemoveItem = new SimpleCommand(ActionRemoveItem);
            CommandAddWeight = new SimpleCommand(ActionAddWeight);
            CommandRemoveWeight = new SimpleCommand(ActionRemoveWeight);
            var temp = new ObservableCollection<Ware>();

            temp.Add(new Ware
            {
                Pos = 1,
                Name = "Stainless steel bottle MBI-A",
                Price = 47.1f,
                Weight = 0.0f,
                Need = 100,
            });
            temp.Add(new Ware
            {
                Pos = 2,
                Name = "Toaster oven kaj-B",
                Price = 87.4f,
                Weight = 0f,
                Need = 150,
            });
            temp.Add(new Ware
            {
                Pos = 3,
                Name = "Thermal magic cooker NFI-A",
                Price = 159.56f,
                Weight = 100.00f,
                Need = 100,
            });

            Items = temp;
            SelectedItem = Items[0];

            temp.CollectionChanged += (o, n) =>
            {
                OnPropertyChanged(nameof(ItemsCount));
                int i = 1;
                foreach (var item in Items)
                {
                    item.Pos = i++;
                }
            };
        }

        #region Props
        public ObservableCollection<Ware> Items { get; set; }
        public Ware SelectedItem { get; set; }
        public int SelectedIndex { get; set; }
        public int ItemsCount => Items.Count;
        public ICommand CommandSelectItem { get; set; }
        public ICommand CommandDoubleClick { get; set; }
        public ICommand CommandLongTap { get; set; }
        public ICommand CommandAddWeight { get; set; }
        public ICommand CommandRemoveWeight { get; set; }
        public ICommand CommandOpenWare { get; set; }
        public ICommand CommandAddItem { get; set; }
        public ICommand CommandAddItems { get; set; }
        public ICommand CommandRemoveItem { get; set; }
        #endregion

        #region Actions
        private void ActionSelectedItem(object param)
        {
            if (param is Ware ware)
            {
                view.DisplayAlert("Select", $"You are selected {ware.Pos} {ware.Name}", "OK");
            }
        }

        private void ActionDoubleClick(object param)
        {
            if (param is Ware ware)
            {
                view.DisplayAlert("Double click", $"You are selected {ware.Pos} {ware.Name}", "OK");
            }
        }

        private void ActionLongTap(object param)
        {
            if (param is Ware ware)
            {
                SelectedItem = ware;
                view.DisplayAlert("Long tap", $"You are selected {ware.Pos} {ware.Name}", "OK");
            }
        }

        private void ActionOpenWare(object param)
        {
            var edit = new Views.WareEdit();
            edit.BindingContext = new ViewModels.WareDetailVm(edit, SelectedItem);
            view.Navigation.PushAsync(edit);
        }

        private void ActionAddItem(object obj)
        {
            Items.Add(new Ware
            {
                Pos = Items.Count + 1,
                Name = "Food jar lcc-a",
                Price = 159.56f,
                Weight = 0.0f,
                Need = 100,
            });
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
                    Items.RemoveAt(SelectedIndex);
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
