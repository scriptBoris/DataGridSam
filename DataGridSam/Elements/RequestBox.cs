using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal class RequestBox : ContentView
    {
        private GridCell Cell;
        public RequestBox(GridCell cell)
        {
            Cell = cell;
        }

        //protected override void OnSizeAllocated(double width, double height)
        //{
        //    base.OnSizeAllocated(width, height);

        //    if (width > 0 && height > 0)
        //    {
        //        if (Cell.Content is Image img)
        //        {
        //            var h = img.HeightRequest;
        //        }

        //        if (Cell.Row is Row2 r && r.isSolve)
        //        {
        //            Cell.IsSizeDone = true;
        //            r.UpdateHeight(Cell, height);
        //        }
        //    }
        //}
    }
}
