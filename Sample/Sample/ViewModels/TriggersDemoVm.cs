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

            Items = DataCollector.GetActors();
        }

        #region Props
        public ICommand CommandLongTap { get; set; }
        public ObservableCollection<Actor> Items { get; set; }
        public override Page View { get; set; } = new Views.TriggersDemoView();
        #endregion

        #region Actions
        private async void ActionLongTap(object param)
        {
            if (param is Actor actor)
            {
                const string v1 = "Set rank";
                const string v2 = "Delete";
                const string v3 = "Cancel";
                string res = await View.DisplayActionSheet("Select action", null, null, new string[]
                { 
                    //v1, 
                    v2, 
                    v3,
                });

                switch (res)
                {
                    //case v1:
                    //    SetUserRank(actor);
                    //    break;
                    case v2:
                        DeleteActor(actor);
                        break;
                }

                //if (res == v1)
                //    SetUserRank(actor);
                //else if (res == v2)
                //    DeleteActor(actor);
            }
        }
        #endregion

        #region Methods
        private async void DeleteActor(object param)
        {
            if (param is Actor actor)
            {
                bool res = await View.DisplayAlert("Delete",$"Delete actor {actor.Name}?",
                    "Delete", "Cancel");
                if (res)
                    Items.Remove(actor);
            }
        }

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
