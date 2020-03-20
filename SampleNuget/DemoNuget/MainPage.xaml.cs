using DemoNuget.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoNuget
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void ButtonImages(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ImagesDemo());
        }

        private void ButtonTriggers(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TriggersDemo());
        }
    }
}