using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal class RequestBox : ContentView
    {
        private IGridRow Row;
        public RequestBox(IGridRow host)
        {
            Row = host;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            Row.UpdateHeight(width);
        }
    }
}
