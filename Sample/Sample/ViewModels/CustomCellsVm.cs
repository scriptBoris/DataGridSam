using Sample.Core;
using Sample.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sample.ViewModels
{
    public class CustomCellsVm : BaseViewModel
    {
        public CustomCellsVm()
        {
            CommandActionTap = new Command(ActionTap);
            CommandSetRank = new Command(ActionSetRank);

            Items = new ObservableCollection<User>
            {
                new User
                {
                    BirthDate = new DateTime(1991, 2, 20),
                    FirstName = "Boris",
                    LastName = "Krit",
                    PhotoUrl = "https://randomuser.me/api/portraits/men/72.jpg",
                    Rank = Ranks.OfficePlankton,
                },
                new User
                {
                    BirthDate = new DateTime(1979, 1, 12),
                    FirstName = "Anna",
                    LastName = "Abraham",
                    PhotoUrl = "https://randomuser.me/api/portraits/women/85.jpg",
                    Rank = Ranks.Manager,
                },
                new User
                {
                    BirthDate = new DateTime(1996, 12, 13),
                    FirstName = "Sam",
                    LastName = "Super",
                    PhotoUrl = "https://randomuser.me/api/portraits/men/55.jpg",
                    Rank = Ranks.Admin,
                }
            };
        }

        #region Props
        public ICommand CommandActionTap { get; set; }
        public ICommand CommandSetRank { get; set; }
        public ObservableCollection<User> Items { get; set; }
        public override Page View { get; set; } = new Views.CustomCellsView();
        #endregion

        #region Actions
        private void ActionTap(object param)
        {
            if (param is User user)
                View.DisplayAlert("Action",$"{user.FirstName}","OK");
        }

        private async void ActionSetRank(object param)
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

                var res = await View.DisplayActionSheet("Set new rank", null, null, ranks);
                if (res == v1)
                    user.Rank = Ranks.OfficePlankton;
                else if (res == v2)
                    user.Rank = Ranks.Manager;
                else if (res == v3)
                    user.Rank = Ranks.Admin;
            }
        }
        #endregion
    }
}
