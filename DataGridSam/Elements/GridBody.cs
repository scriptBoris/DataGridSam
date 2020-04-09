using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal class GridBody : Layout<View>
    {
        internal View StackList;
        internal View Mask;
        internal DataGrid DataGrid;
        public GridBody(DataGrid grid, StackList stack, Grid mask)
        {
            DataGrid = grid;
            StackList = stack;
            Mask = mask;

            Children.Add(stack);
            Children.Add(mask);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var h = StackList.Measure(widthConstraint, double.PositiveInfinity, MeasureFlags.IncludeMargins);
            //var m = Mask.Measure(widthConstraint, double.PositiveInfinity, MeasureFlags.None);
            double height = h.Request.Height;

            return new SizeRequest(new Size(widthConstraint, height));
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            var rect = new Rectangle(0, 0, width, height);
            LayoutChildIntoBoundingRegion(StackList, rect);
            LayoutChildIntoBoundingRegion(Mask, rect);
        }
    }
}
