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
    public class StylesVm : BaseViewModel
    {
        public StylesVm()
        {
            CommandActionTap = new Command(ActionTap);
            CommandSetRank = new Command(ActionSetRank);

            Items = DataCollector.GetUsers();
        }

        #region Props
        public ICommand CommandActionTap { get; set; }
        public ICommand CommandSetRank { get; set; }
        public ObservableCollection<User> Items { get; set; }
        public override Page View { get; set; } = new Views.StylesView ();
        #endregion

        #region Actions
        private async void ActionTap(object param)
        {
            if (param is User user)
            {
                bool res = await View.DisplayAlert("Delete",$"Delete user {user.FirstName} {user.LastName}?",
                    "Delete", "Cancel");
                if (res)
                    Items.Remove(user);
            }
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
