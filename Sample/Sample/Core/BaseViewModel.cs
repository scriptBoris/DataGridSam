using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sample.Core
{
    public abstract class BaseViewModel : BaseNotify
    {
        public BaseViewModel()
        {
            View.BindingContext = this;
        }

        #region Props
        public abstract Page View { get; set; }
        #endregion

        #region Methods
        public async Task NavigationGoAsync(BaseViewModel vm)
        {
            await View.Navigation.PushAsync(vm.View);
        }
        public void NavigationGo(BaseViewModel vm)
        {
            View.Navigation.PushAsync(vm.View);
        }
        #endregion
    }
}
