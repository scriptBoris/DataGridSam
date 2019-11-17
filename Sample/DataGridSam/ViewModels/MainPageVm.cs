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
        private ICommand commandSelectItem;
        private ICommand commandOpenWare;
        private ICommand commandAddItem;
        private ICommand commandRemoveItem;
        private ObservableCollection<Ware> items;
        private Ware selectedItem;
        private readonly Page view;

        [Obsolete("Only for IntelliSense XAML helper")]
        public MainPageVm() { }
        public MainPageVm(Page view)
        {
            this.view = view;
            CommandSelectItem = new SimpleCommand(ActionSelectedItem);
            CommandOpenWare = new SimpleCommand(ActionOpenWare);
            CommandAddItem = new SimpleCommand(ActionAddItem);
            commandRemoveItem = new SimpleCommand(ActionRemoveItem);
            var temp = new ObservableCollection<Ware>();

            temp.Add(new Ware
            {
                Pos = 1,
                Name = "Stainless steel bottle MBI-A",
                Price = 47.1f,
                Weight = 0.0f,
                IsCompleted = true,
            });
            //temp.Add(new Ware
            //{
            //    Pos = 2,
            //    Name = "Toaster oven kaj-B",
            //    Price = 87.4f,
            //    Weight = 0f,
            //});
            //temp.Add(new Ware
            //{
            //    Pos = 3,
            //    Name = "Thermal magic cooker NFI-A",
            //    Price = 159.56f,
            //    Weight = 14.00f,
            //});

            Items = temp;
            SelectedItem = Items[0];
        }

        #region Props
        public ObservableCollection<Ware> Items
        {
            get => items;
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
        public ICommand CommandSelectItem
        {
            get => commandSelectItem;
            set
            {
                commandSelectItem = value;
                OnPropertyChanged(nameof(CommandSelectItem));
            }
        }

        public ICommand CommandOpenWare
        {
            get => commandOpenWare;
            set
            {
                commandOpenWare = value;
                OnPropertyChanged(nameof(CommandOpenWare));
            }
        }

        public ICommand CommandAddItem
        {
            get => commandAddItem;
            set
            {
                commandAddItem = value;
                OnPropertyChanged(nameof(CommandAddItem));
            }
        }

        public ICommand CommandRemoveItem
        {
            get => commandRemoveItem;
            set
            {
                commandRemoveItem = value;
                OnPropertyChanged(nameof(CommandRemoveItem));
            }
        }

        public Ware SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        #endregion

        #region Actions
        private void ActionSelectedItem(object param)
        {
            if (param is Ware ware)
            {
                //view.DisplayAlert("Select", $"You are selected {ware.Pos} {ware.Name}", "OK");
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

        private void ActionRemoveItem(object obj)
        {
            if (Items != null && Items.Count > 0)
            {
                Items.Remove(Items.LastOrDefault());
            }
        }
        #endregion

    }
}
