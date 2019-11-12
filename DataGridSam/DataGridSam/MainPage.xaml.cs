using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sample
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

        public MainPage()
        {
            InitializeComponent();

            Items.Add(new Item 
            { 
                Pos = 1,
                Name = "Сладкое мороженное",
                Price = 47.1f,
                Weight = 0.3f,
            });
            Items.Add(new Item
            {
                Pos = 2,
                Name = "Жареные гренки",
                Price = 87.4f,
                Weight = 0.15f,
            });
            Items.Add(new Item
            {
                Pos = 3,
                Name = "Хлебцы в кисло-сладком соусе ВЕСОВЫЕ",
                Price = 159.56f,
                Weight = 0.86f,
            });
            BindingContext = this;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Items.Add(new Item
            {
                Pos = 4,
                Name = "Хлебцы в кисло-сладком соусе ВЕСОВЫЕ",
                Price = 159.56f,
                Weight = 0.86f,
            });
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            if (Items != null && Items.Count > 0)
            {
                Items.Remove(Items.LastOrDefault());
            }
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            int rand = new Random().Next(0, Items.Count - 1);
            var item = new Item
            {
                Pos = rand,
                Name = Guid.NewGuid().ToString(),
                Price = 159.56f,
                Weight = 0.86f,
            };

            Items.Insert(rand, item);
        }

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
    }

    public class Item
    {
        public int Pos { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public float Weight { get; set; }
    }
}
