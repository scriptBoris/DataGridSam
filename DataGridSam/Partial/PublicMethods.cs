using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DataGridSam
{
    public partial class DataGrid : Grid
    {
        public Task ScrollToElement(int id, ScrollToPosition scrollToPosition, bool isAnimated)
        {
            if (id < 0)
                id = 0;
            else if (id > stackList.Children.Count - 1)
                id = stackList.Children.Count - 1;

            var element = stackList.Children[id];
            Task.Delay(10).Wait();
            return mainScroll.ScrollToAsync(element, scrollToPosition, isAnimated);

            //if (id == stackList.Children.Count - 1)
            //{
            //    mainScroll.ScrollToAsync(0, stackList.Height, isAnimated);
            //}
            //else
            //{
            //    var element = stackList.Children[id];
            //    mainScroll.ScrollToAsync(element, ScrollToPosition.MakeVisible, isAnimated);
            //}
        }
    }
}
