using DataGridSam.Platform;
using DataGridSam.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
//using TouchSam;
//using DataGridSam.Platform;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    internal sealed class GridHeadRow : Layout<View>
    {
        private bool isLineVisible;
        
        internal double RowHeight = -1;
        internal DataGrid DataGrid;
        internal int Index;
        internal object Context;
        internal View SelectionBox;
        internal View Line;
        internal List<GridHeadCell> Cells;

        public GridHeadRow(object context, DataGrid host, bool isLineVisible) 
        {
            Context = context;
            BindingContext = context;
            DataGrid = host;
            Index = 0;
            this.isLineVisible = isLineVisible;

            IsClippedToBounds = true;
            VerticalOptions = LayoutOptions.Start;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Cells = new List<GridHeadCell>();

            // Selection box
            SelectionBox = new BoxView();
            SelectionBox.InputTransparent = true;
            SelectionBox.BackgroundColor = Color.Transparent;
            Children.Add(SelectionBox);

            // Init cells
            foreach (var column in DataGrid.Columns)
            {
                var cell = new GridHeadCell(this, column);

                Children.Add(cell.BackgroundBox);
                Children.Add(cell.Content);
                Cells.Add(cell);
            }

            // Add tap system event
            //Touch.SetSelect(this, new Command(ActionRowSelect));

            //if (DataGrid.TapColor != Color.Default)
            //    Touch.SetColor(this, DataGrid.TapColor);

            //if (DataGrid.CommandSelectedItem != null)
            //    Touch.SetTap(this, DataGrid.CommandSelectedItem);

            //if (DataGrid.CommandLongTapItem != null)
            //    Touch.SetLongTap(this, DataGrid.CommandLongTapItem);


            // Create horizontal line table
            Line = new BoxView();
            Line.BackgroundColor = DataGrid.BorderColor;
            Line.HeightRequest = DataGrid.BorderWidth;
            Line.InputTransparent = true;
            Children.Add(Line);

            // Render style
            UpdateVisual();
        }

        public void UpdateVisual()
        {
        }

        public void UpdateLineVisibility(bool isVisible)
        {
            if (isLineVisible == isVisible)
                return;

            if (isVisible)
            {
                double height = Height + DataGrid.BorderWidth;
                HeightRequest = height;
                LayoutChildIntoBoundingRegion(Line, new Rectangle(0, height - DataGrid.BorderWidth, Width, DataGrid.BorderWidth));
            }
            else
            {
                HeightRequest = Height - DataGrid.BorderWidth;
                LayoutChildIntoBoundingRegion(Line, new Rectangle(0, 0, 0, 0));
            }

            isLineVisible = isVisible;
        }

        public void UpdateCellVisibility(int cellId, bool isVisible)
        {
            var cell = Cells[cellId];

            cell.BackgroundBox.IsVisible = isVisible;
            cell.Content.IsVisible = isVisible;
        }

        private void MergeVisual(Label label, params VisualCollector[] styles)
        {
            label.TextColor = ValueSelector.GetTextColor(styles);
            label.FontAttributes = ValueSelector.FontAttribute(styles);
            label.FontFamily = ValueSelector.FontFamily(styles);
            label.FontSize = ValueSelector.FontSize(styles);

            label.LineBreakMode = ValueSelector.GetLineBreakMode(styles);
            label.VerticalTextAlignment = ValueSelector.GetVerticalAlignment(styles);
            label.HorizontalTextAlignment = ValueSelector.GetHorizontalAlignment(styles);
        }

        #region Layot calculation
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            // Render cells
            foreach (var cell in Cells)
            {
                if (!cell.Column.IsVisible)
                    continue;

                RenderCellOnLayout(cell, width, height);
            }

            // Selection box
            var rectSelection = new Rectangle(0, 0, width, height + DataGrid.BorderWidth);
            LayoutChildIntoBoundingRegion(SelectionBox, rectSelection);

            // Render line
            if (isLineVisible)
            {
                var rect = new Rectangle(0, height - DataGrid.BorderWidth, width, height);
                LayoutChildIntoBoundingRegion(Line, rect);
            }
        }

        internal void RenderRow(double x, double y, double width, double height)
        {
            LayoutChildren(x, y, width, height);
            HeightRequest = RowHeight;
        }

        protected override SizeRequest OnMeasure(double width, double height)
        {
            if (Cells.Count == 0 || !IsVisible)
                return new SizeRequest(new Size(width, 0));

            double actualHeight = 0.0;
            foreach (var cell in Cells)
            {
                if (!cell.Column.IsVisible)
                    continue;

                double cellHeight = CalculateCellHeight(cell, width);

                if (actualHeight < cellHeight)
                    actualHeight = cellHeight;
            }

            // Position for line
            if (isLineVisible)
                actualHeight += DataGrid.BorderWidth;

            RowHeight = actualHeight;

            return new SizeRequest(new Size(width, actualHeight));
        }

        private double CalculateCellHeight(GridHeadCell cell, double rowWidth)
        {
            double width = CalcWidth(rowWidth, cell.Column);
            var cellSize = cell.Content.Measure(width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
            return cellSize.Request.Height;
        }

        private void RenderCellOnLayout(GridHeadCell cell, double rowWidth, double rowHeight)
        {
            double w = CalcWidth(rowWidth, cell.Column);
            double x = CalcX(rowWidth, cell.Column);
            var rect = new Rectangle(x, 0, w, rowHeight);

            LayoutChildIntoBoundingRegion(cell.BackgroundBox, rect);
            LayoutChildIntoBoundingRegion(cell.Content, rect);
        }

        // TODO Upgrade performance
        private double CalcWidth(double rowWidth, DataGridColumn col)
        {
            if (!col.IsVisible)
                return 0.0;

            if (col.Width.IsAbsolute)
            {
                return col.Width.Value;
            }
            else
            {
                double com = 0;
                double dif = 0;
                foreach (var c in DataGrid.Columns)
                {
                    if (!c.IsVisible)
                        continue;

                    if (c.Width.IsStar)
                        com += c.Width.Value;
                    else
                        dif += c.Width.Value;
                }

                return (rowWidth - dif) * (col.Width.Value / com);
            }
        }

        // TODO Upgrade performance
        private double CalcX(double rowWidth, DataGridColumn col)
        {
            double sum = 0;
            for (int i = col.Index-1; i >= 0; i--)
            {
                if (DataGrid.Columns[i].IsVisible)
                    sum += DataGrid.Columns[i].ActualWidth;
            }

            if (!col.IsVisible)
            {
                return sum;
            }
            else if (col.Width.IsStar)
            {
                double dif = 0;
                double com = 0;
                foreach (var c in DataGrid.Columns)
                {
                    if (!c.IsVisible)
                        continue;

                    if (c.Width.IsStar)
                        com += c.Width.Value;
                    else
                        dif += c.Width.Value;
                }

                double final = col.Width.Value / com;
                col.ActualWidth = (rowWidth - dif) * final;

                if (col.Index == 0)
                    return 0;

                return sum;
            }
            else
            {
                col.ActualWidth = col.Width.Value;

                if (col.Index == 0)
                    return 0;

                return sum;
            }
        }
        #endregion Layout calculation
    }
}
