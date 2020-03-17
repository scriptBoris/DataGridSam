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
    public class TriggersDemoVm : BaseViewModel
    {
        public TriggersDemoVm()
        {
            CommandLongTap = new Command(ActionLongTap);

            Items = new ObservableCollection<User>
            {
                new User
                {
                    BirthDate = new DateTime(1992, 7, 28),
                    FirstName = "Diana",
                    LastName = "Roseborough",
                    PhotoUrl = "https://randomuser.me/api/portraits/women/19.jpg",
                    Rank = Ranks.OfficePlankton,
                },
                new User
                {
                    BirthDate = new DateTime(1989, 9, 29),
                    FirstName = "Carmen",
                    LastName = "Speights",
                    PhotoUrl = "https://randomuser.me/api/portraits/men/85.jpg",
                    Rank = Ranks.OfficePlankton,
                },
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
                },
                new User
                {
                    BirthDate = new DateTime(1996, 8, 8),
                    FirstName = "Tommy",
                    LastName = "Mcsherry",
                    PhotoUrl = "https://randomuser.me/api/portraits/men/46.jpg",
                    Rank = Ranks.Manager,
                },
                new User
                {
                    BirthDate = new DateTime(2001, 1, 27),
                    FirstName = "Candie",
                    LastName = "Hopping",
                    PhotoUrl = "https://randomuser.me/api/portraits/women/26.jpg",
                    Rank = Ranks.OfficePlankton,
                },
                new User
                {
                    BirthDate = new DateTime(1985, 10, 3),
                    FirstName = "Vincent",
                    LastName = "Ruvalcaba",
                    PhotoUrl = "https://randomuser.me/api/portraits/men/15.jpg",
                    Rank = Ranks.OfficePlankton,
                },
                new User
                {
                    BirthDate = new DateTime(1988, 1, 13),
                    FirstName = "Jeffry",
                    LastName = "Wehner",
                    PhotoUrl = "https://randomuser.me/api/portraits/men/64.jpg",
                    Rank = Ranks.OfficePlankton,
                },
            };
        }

        #region Props
        public ICommand CommandLongTap { get; set; }
        public ObservableCollection<User> Items { get; set; }
        public override Page View { get; set; } = new Views.TriggersDemoView();
        #endregion

        #region Actions
        private async void ActionLongTap(object param)
        {
            if (param is User user)
            {
                const string v1 = "Set rank";
                const string v2 = "Delete";
                const string v3 = "Cancel";
                string res = await View.DisplayActionSheet("Select action", null, null, new string[]
                { 
                    v1, v2, v3,
                });

                if (res == v1)
                    SetUserRank(user);
                else if (res == v2)
                    DeleteUser(user);
            }
        }
        #endregion

        #region Methods
        private async void DeleteUser(object param)
        {
            if (param is User user)
            {
                bool res = await View.DisplayAlert("Delete",$"Delete user {user.FirstName} {user.LastName}?",
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
