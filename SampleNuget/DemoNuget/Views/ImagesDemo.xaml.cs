using DemoNuget.Core;
using DemoNuget.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoNuget.Views
{
    public partial class ImagesDemo : ContentPage
    {
        public ImagesDemo()
        {
            InitializeComponent();
            BindingContext = this;

            Items = DataCollector.GetActors();
        }

        public ObservableCollection<Actor> Items { get; set; }
    }
}