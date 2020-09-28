using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DataGridSam
{
    public partial class DataGrid
    {
        public void ScrollToElement(int id, bool isAnimated)
        {
            if (id < 0)
                id = 0;
            else if (id > stackList.Children.Count - 1)
                id = stackList.Children.Count - 1;

            var element = stackList.Children[id];
            mainScroll.ScrollToAsync(element, ScrollToPosition.MakeVisible, isAnimated);
        }

        public async Task ScrollToElementAsync(int id, bool isAnimated)
        {
            if (id < 0)
                id = 0;
            else if (id > stackList.Children.Count - 1)
                id = stackList.Children.Count - 1;

            var element = stackList.Children[id];
            await mainScroll.ScrollToAsync(element, ScrollToPosition.MakeVisible, isAnimated);
        }
    }
}
