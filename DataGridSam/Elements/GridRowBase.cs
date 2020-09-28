using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal interface IGridRow
    {
        bool IsLineVisible { get; set; }
        double RowHeight { get; set; }
        DataGrid DataGrid { get; set; }
        int Index { get; set; }
        bool IsSelected { get; set; }
        object Context { get; set; }
        View SelectionBox { get; set; }
        View Line { get; set; }
        RowTrigger EnabledTrigger { get; set; }
    }

    internal abstract class GridRowBase<T> : Layout<View>, IGridRow where T : GridCellBase, new()
    {
        public bool IsLineVisible { get; set; } = true;
        public double RowHeight { get; set; } = -1;
        public DataGrid DataGrid { get; set; }
        public int Index { get; set; }
        public bool IsSelected { get; set; }
        public object Context { get; set; }
        public View SelectionBox { get; set; }
        public View Line { get; set; }
        public RowTrigger EnabledTrigger { get; set; }

        internal List<T> Cells;

        /// <summary>
        /// Default ctor for generir: new T()
        /// </summary>
        public GridRowBase()
        {
        }

        public GridRowBase(object context, DataGrid host, int id, bool isLineVisible)
        {
            DataGrid = host;
            Index = id;
            IsLineVisible = isLineVisible;

            BuildElements(context);
        }

        internal void BuildElements(object context)
        {
            Context = context;
            BindingContext = context;
            Cells = new List<T>();

            // Selection box
            SelectionBox = new BoxView();
            SelectionBox.InputTransparent = true;
            SelectionBox.BackgroundColor = Color.Transparent;
            Children.Add(SelectionBox);

            // Init cells
            foreach (var column in DataGrid.Columns)
            {
                var cell = new T();
                cell.Init(this, column);

                Children.Add(cell.BackgroundBox);
                Children.Add(cell.Content);
                Cells.Add(cell);
            }


            // Create horizontal line table
            Line = new BoxView();
            Line.BackgroundColor = DataGrid.BorderColor;
            Line.HeightRequest = DataGrid.BorderWidth;
            Line.InputTransparent = true;
            Children.Add(Line);

            RedrawElements(context);
        }

        protected abstract void RedrawElements(object context);

        internal void UpdateLineVisibility(bool isVisible)
        {
            if (IsLineVisible == isVisible)
                return;

            IsLineVisible = isVisible;
            Line.IsVisible = isVisible;
            InvalidateMeasure();

            //if (isVisible)
            //{
            //    double height = Height + DataGrid.BorderWidth;
            //    HeightRequest = height;
            //    LayoutChildIntoBoundingRegion(Line, new Rectangle(0, height - DataGrid.BorderWidth, Width, DataGrid.BorderWidth));
            //}
            //else
            //{
            //    HeightRequest = Height - DataGrid.BorderWidth;
            //    LayoutChildIntoBoundingRegion(Line, new Rectangle(0, 0, 0, 0));
            //}
        }

        internal void UpdateCellVisibility(int cellId, bool isVisible)
        {
            var cell = Cells[cellId];

            cell.BackgroundBox.IsVisible = isVisible;
            cell.Content.IsVisible = isVisible;
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
            if (IsLineVisible)
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
            if (IsLineVisible)
                actualHeight += DataGrid.BorderWidth;

            RowHeight = actualHeight;

            return new SizeRequest(new Size(width, actualHeight));
        }

        private double CalculateCellHeight(GridCellBase cell, double rowWidth)
        {
            double width = CalcWidth(rowWidth, cell.Column);
            var cellSize = cell.Content.Measure(width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
            return cellSize.Request.Height;
        }

        private void RenderCellOnLayout(GridCellBase cell, double rowWidth, double rowHeight)
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
            for (int i = col.Index - 1; i >= 0; i--)
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
