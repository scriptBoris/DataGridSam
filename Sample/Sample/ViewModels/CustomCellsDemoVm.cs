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
    public class CustomCellsDemoVm : BaseViewModel
    {
        public CustomCellsDemoVm()
        {
            CommandLongTap = new Command(ActionLongTap);
            CommandGiveOskar = new Command(ActionGiveOskar);
            CommandGiveRaspberry = new Command(ActionGiveRaspberry);
            CommandDelete = new Command(ActionDelete);

            Items = DataCollector.GetActors();
        }

        #region Props
        public ICommand CommandLongTap { get; set; }
        public ICommand CommandGiveOskar { get; set; }
        public ICommand CommandGiveRaspberry { get; set; }
        public ICommand CommandDelete { get; set; }
        public ObservableCollection<Actor> Items { get; set; }
        public override Page View { get; set; } = new Views.CustomCellsDemoView();
        #endregion

        #region Actions
        private async void ActionLongTap(object param)
        {
            if (param is Actor actor)
            {
                const string v1 = "Give Oskar";
                const string v2 = "Give gold Raspberry";
                const string v3 = "Delete";
                const string v4 = "Cancel";
                string res = await View.DisplayActionSheet("Select action", null, null, new string[]
                { 
                    v1, 
                    v2, 
                    v3,
                    v4,
                });

                switch (res)
                {
                    case v1:
                        CommandGiveOskar.Execute(actor);
                        break;
                    case v2:
                        CommandGiveRaspberry.Execute(actor);
                        break;
                    case v3:
                        CommandDelete.Execute(actor);
                        break;
                }

                //if (res == v1)
                //    SetUserRank(actor);
                //else if (res == v2)
                //    DeleteActor(actor);
            }
        }

        private async void ActionGiveOskar(object param)
        {
            if (param is Actor actor)
            {
                await View.DisplayAlert("TODO", "Not implement", "Sorry");
            }
        }
        
        private async void ActionGiveRaspberry(object param)
        {
            if (param is Actor actor)
            {
                await View.DisplayAlert("TODO", "Not implement", "Sorry");
            }
        }

        private async void ActionDelete(object param)
        {
            if (param is Actor actor)
            {
                bool res = await View.DisplayAlert("Delete", $"Delete actor {actor.Name}?",
                    "Delete", "Cancel");
                if (res)
                    Items.Remove(actor);
            }
        }

        #endregion

        #region Methods
        //private async void SetUserRank(object param)
        //{
        //    //if (param is Actor actor)
        //    //{
        //    //    const string v1 = "OfficePlankton";
        //    //    const string v2 = "Manager";
        //    //    const string v3 = "Admin";
        //    //    string[] ranks = new string[]
        //    //    {
        //    //        v1,
        //    //        v2,
        //    //        v3,
        //    //    };

        //    //    var res = await View.DisplayActionSheet("Set new rank", null, null, ranks);
        //    //    if (res == v1)
        //    //        actor.Rank = Ranks.OfficePlankton;
        //    //    else if (res == v2)
        //    //        actor.Rank = Ranks.Manager;
        //    //    else if (res == v3)
        //    //        actor.Rank = Ranks.Admin;
        //    //}
        //}
        #endregion
    }
}
