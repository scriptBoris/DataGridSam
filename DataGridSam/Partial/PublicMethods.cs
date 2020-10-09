using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DataGridSam
{
    public partial class DataGrid : Grid
    {
#if __ios__
[Obsolete("iOS not supported")]
#endif
        /// <summary>
        /// Scroll to element Id (iOS not supported)
        /// </summary>
        /// <param name="id">id element</param>
        /// <param name="scrollToPosition">rect element scroll</param>
        /// <param name="isAnimated">animation</param>
        /// <returns></returns>
        public Task ScrollToElement(int id, ScrollToPosition scrollToPosition, bool isAnimated)
        {
            if (id < 0)
                id = 0;
            else if (id > stackList.Children.Count - 1)
                id = stackList.Children.Count - 1;

            //var element = stackList.Children[id];
            //return mainScroll.ScrollToAsync(element, scrollToPosition, isAnimated);

            if (id == stackList.Children.Count - 1)
            {
                return mainScroll.ScrollToAsync(0, stackList.StackHeight, isAnimated);
            }
            else
            {
                var element = stackList.Children[id];
                return mainScroll.ScrollToAsync(element, scrollToPosition, isAnimated);
            }
        }
    }
}
