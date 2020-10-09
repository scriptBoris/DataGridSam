using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal class GridBody : Layout<View>
    {
        internal DataGrid DataGrid;
        internal StackList StackList;
        internal View EmptyView;


        internal View lineLeft = new BoxView();
        internal View lineRight = new BoxView();
        internal View lineBottom = new BoxView();
        private List<View> allLines = new List<View>();
        private List<View> colLines = new List<View>();

        public GridBody(DataGrid grid, StackList stack /*, Grid mask*/)
        {
            DataGrid = grid;
            StackList = stack;

            Children.Add(stack);
            AddStaticLines();

            foreach (var line in allLines)
            {
                line.InputTransparent = true;
                line.BackgroundColor = DataGrid.BorderColor;

                Children.Add(line);
            }
        }

        internal void UpdateColumns()
        {
            allLines.Clear();
            colLines.Clear();

            AddStaticLines();
            if (DataGrid.Columns != null)
                foreach (var col in DataGrid.Columns)
                    CreateVLine();
        }

        private void AddStaticLines()
        {
            allLines.Add(lineRight);
            allLines.Add(lineLeft);
            allLines.Add(lineBottom);
        }

        internal void UpdateBorderColor()
        {
            foreach (var line in allLines)
                line.BackgroundColor = DataGrid.BorderColor;

            foreach (var row in StackList.Children)
                row.Line.BackgroundColor = DataGrid.BorderColor;
        }

        internal void Redraw(bool isNeedMeasure = true)
        {
            if (isNeedMeasure)
                InvalidateMeasure();

            InvalidateLayout();
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            double wrap = DataGrid.IsWrapped ? DataGrid.BorderWidth : 0;
            double height = 0;

            var stackRes = StackList.Measure(widthConstraint, double.PositiveInfinity, MeasureFlags.IncludeMargins);
            height += stackRes.Request.Height;

            if (EmptyView != null && EmptyView.IsVisible)
            {
                var emptyRes = EmptyView.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
                height += emptyRes.Request.Height;
            }

            height += wrap;

            return new SizeRequest(new Size(widthConstraint, height));
        }


        private double lastWidth = -1;
        private double lastHeight = -1;

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (lastWidth != width || lastHeight != height)
            {
                //InvalidateMeasure();
                StackList.Redraw(true, false);
                lastHeight = height;
                lastWidth = width;
            }

            double wrap = DataGrid.IsWrapped ? DataGrid.BorderWidth : 0;
            double bw = DataGrid.BorderWidth;
            var rect = new Rectangle(wrap, 0, width - wrap * 2, height - wrap);

            if (wrap > 0)
            {
                // left
                var rectLeft = new Rectangle(0, 0, wrap, height);
                LayoutChildIntoBoundingRegion(lineLeft, rectLeft);


                // right
                var rectRight = new Rectangle(width - wrap, 0, wrap, height);
                LayoutChildIntoBoundingRegion(lineRight, rectRight);

                // bottom
                var rectBottom = new Rectangle(0, height - wrap, width, wrap);
                LayoutChildIntoBoundingRegion(lineBottom, rectBottom);
            }
            else
            {
                LayoutChildIntoBoundingRegion(lineLeft, Rectangle.Zero);
                LayoutChildIntoBoundingRegion(lineRight, Rectangle.Zero);
                LayoutChildIntoBoundingRegion(lineBottom, Rectangle.Zero);
            }

            // Column lines
            if (DataGrid.Columns != null && StackList.ItemsCount > 0)
            {
                double lastX = wrap;
                for (int i = 0; i < DataGrid.Columns.Count-1; i++)
                {
                    var col = DataGrid.Columns[i];
                    var line = colLines[i];

                    if (col.IsVisible)
                    {
                        lastX += col.ActualWidth;
                        var rectCol = new Rectangle(lastX, 0, bw, height);
                        LayoutChildIntoBoundingRegion(line, rectCol);
                        lastX += bw;
                    }
                    else
                        LayoutChildIntoBoundingRegion(line, Rectangle.Zero);
                }
            }
            else 
            {
                foreach (var col in colLines)
                    LayoutChildIntoBoundingRegion(col, Rectangle.Zero);
            }

            // Empty Content
            if (EmptyView != null && EmptyView.IsVisible)
                LayoutChildIntoBoundingRegion(EmptyView, rect);

            // Stack list
            LayoutChildIntoBoundingRegion(StackList, rect);
        }

        private View CreateVLine()
        {
            var res = new BoxView();
            res.InputTransparent = true;
            res.BackgroundColor = DataGrid.BorderColor;
            
            Children.Add(res);
            allLines.Add(res);
            colLines.Add(res);
            
            return res;
        }
    }
}
