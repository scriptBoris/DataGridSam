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
    public partial class TriggersDemo : ContentPage
    {
        public TriggersDemo()
        {
            InitializeComponent();
            BindingContext = this;

            CommandLongTap = new Command(ActionLongTap);
            Items = DataCollector.GetUsers();
        }

        public ICommand CommandLongTap { get; set; }
        public ObservableCollection<User> Items { get; set; }

        private async void ActionLongTap(object param)
        {
            if (param is User user)
            {
                const string v1 = "Set rank";
                const string v2 = "Delete";
                const string v3 = "Cancel";
                string res = await DisplayActionSheet("Select action", null, null, new string[]
                {
                    v1, v2, v3,
                });

                if (res == v1)
                    SetUserRank(user);
                else if (res == v2)
                    DeleteUser(user);
            }
        }

        private async void DeleteUser(object param)
        {
            if (param is User user)
            {
                bool res = await DisplayAlert("Delete", $"Delete user {user.FirstName} {user.LastName}?",
                    "Delete", "Cancel");
                if (res)
                    Items.Remove(user);
            }
        }

        private async void SetUserRank(object param)
        {
            if (param is User user)
            {
                const string v1 = "OfficePlankton";
                const string v2 = "Manager";
                const string v3 = "Admin";
                string[] ranks = new string[]
                {
                    v1,
                    v2,
                    v3,
                };

                var res = await DisplayActionSheet("Set new rank", null, null, ranks);
                if (res == v1)
                    user.Rank = Ranks.OfficePlankton;
                else if (res == v2)
                    user.Rank = Ranks.Manager;
                else if (res == v3)
                    user.Rank = Ranks.Admin;
            }
        }
    }
}