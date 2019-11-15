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

namespace Sample
{
    public partial class MainPage : ContentPage
    {
        private ICommand commandSelectItem;
        private ObservableCollection<Ware> items;
        private Ware selectedItem;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            CommandSelectItem = new CommandVm(ActionSelectedItem);
            Items = new ObservableCollection<Ware>();

            Items.Add(new Ware 
            { 
                Pos = 1,
                Name = "Сладкое мороженное",
                Price = 47.1f,
                Weight = 0.3f,
            });
            Items.Add(new Ware
            {
                Pos = 2,
                Name = "Жареные гренки",
                Price = 87.4f,
                Weight = 0.15f,
                IsCompleted = true,
            });
            Items.Add(new Ware
            {
                Pos = 3,
                Name = "Хлебцы в кисло-сладком соусе ВЕСОВЫЕ",
                Price = 159.56f,
                Weight = 0.86f,
            });
        }

        public ObservableCollection<Ware> Items {
            get => items;
            set {
                items = value;
                OnPropertyChanged(nameof(Items));
            } 
        }
        public ICommand CommandSelectItem { 
            get => commandSelectItem;
            set { 
                commandSelectItem = value;
                OnPropertyChanged(nameof(CommandSelectItem));
            } 
        }

        public Ware SelectedItem
        {
            get => selectedItem;
            set {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private void ActionSelectedItem(object param)
        {
            if (param is Ware ware)
            {
                ware.IsCompleted = true;
                //DisplayAlert("Action param",$"№{ware.Pos} {ware.Name}","OK");
                //DisplayAlert("Selected item",$"№{SelectedItem?.Pos} {SelectedItem?.Name}","OK");
            }
        }

        // Add
        private void Button_Clicked(object sender, EventArgs e)
        {
            Items.Add(new Ware
            {
                Pos = 4,
                Name = "Хлебцы в кисло-сладком соусе ВЕСОВЫЕ",
                Price = 159.56f,
                Weight = 100.86f,
                IsCompleted = true,
            });
        }

        // Remove last
        private void Button_Clicked_1(object sender, EventArgs e)
        {
            if (Items != null && Items.Count > 0)
            {
                Items.Remove(Items.LastOrDefault());
            }
        }

        // Add random
        private void Button_Clicked_2(object sender, EventArgs e)
        {
            int rand = new Random().Next(0, Items.Count - 1);
            var item = new Ware
            {
                Pos = rand,
                Name = Guid.NewGuid().ToString(),
                Price = 159.56f,
                Weight = 0.86f,
            };

            Items.Insert(rand, item);
        }

        // Remove random
        private void Button_Clicked_3(object sender, EventArgs e)
        {
            if (Items == null && Items.Count == 0)
            {
                return;
            }

            int rand = new Random().Next(0, Items.Count - 1);
            var item = Items[rand];

            Items.Remove(item);

        }

        // Set completed
        private void Button_Clicked_4(object sender, EventArgs e)
        {
            if (Items == null && Items.Count == 0)
            {
                return;
            }

            int rand = new Random().Next(0, Items.Count - 1);
            var item = Items[rand];

            if (item.IsCompleted)
                item.IsCompleted = false;
            else
                item.IsCompleted = true;
        }
    }
}
