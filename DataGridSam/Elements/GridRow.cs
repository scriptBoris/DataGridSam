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
    internal sealed class GridRow : Layout<View>
    {
        private bool isLineVisible;

        internal double RowHeight = -1;
        internal DataGrid DataGrid;
        internal int Index;
        internal bool IsSelected;
        internal object Context;
        internal View SelectionBox;
        internal View Line;
        internal List<GridCell> Cells;
        internal RowTrigger EnabledTrigger;

        public GridRow(object context, StackList host, int id, bool showBottomLine, bool isAutoNumber)
        {
            Context = context;
            BindingContext = context;
            DataGrid = host.DataGrid;
            Index = id;
            isLineVisible = showBottomLine;

            IsClippedToBounds = true;
            VerticalOptions = LayoutOptions.Start;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Cells = new List<GridCell>();

            // Triggers event
            if (context is INotifyPropertyChanged model && DataGrid.RowTriggers.Count > 0)
                model.PropertyChanged += (obj, e) => RowTrigger.SetTriggerStyle(this, e.PropertyName);

            // Selection box
            SelectionBox = new BoxView();
            SelectionBox.InputTransparent = true;
            SelectionBox.BackgroundColor = Color.Transparent;
            Children.Add(SelectionBox);

            // Init cells
            int i = 0;
            foreach (var column in DataGrid.Columns)
            {
                var cell = new GridCell(this, column);

                Children.Add(cell.Wrapper);
                Children.Add(cell.Content);
                Cells.Add(cell);
                i++;
            }

            // Add tap system event
            if (DataGrid.TapColor != Color.Default ||
                DataGrid.CommandLongTapItem != null ||
                DataGrid.CommandSelectedItem != null)
                Touch.SetHost(this, DataGrid);

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

            // Auto number
            if (DataGrid.IsAutoNumberCalc && isAutoNumber)
                UpdateAutoNumeric(Index);

            // find FIRST active trigger
            if (this.DataGrid.RowTriggers.Count > 0)
                foreach (var trigg in this.DataGrid.RowTriggers)
                    if (trigg.CheckTriggerActivated(BindingContext))
                    {
                        this.EnabledTrigger = trigg;
                        break;
                    }

            // Render style
            UpdateStyle();
        }

        internal void SelectRow()
        {
            var rowTapped = this;
            var lastTapped = DataGrid.SelectedRow;

            // GUI Unselected last row
            if (lastTapped != null && lastTapped != rowTapped)
            {
                lastTapped.IsSelected = false;
                lastTapped.UpdateStyle();
            }

            // GUI Selected row
            if (lastTapped != rowTapped)
            {
                DataGrid.SelectedRow = rowTapped;
                DataGrid.SelectedItem = BindingContext;

                rowTapped.IsSelected = true;
                rowTapped.UpdateStyle();
            }
        }

        public void UpdateStyle()
        {
            // Selected
            if (IsSelected)
            {
                var color = ValueSelector.GetSelectedColor(
                    DataGrid.VisualSelectedRowFromStyle.BackgroundColor,
                    DataGrid.VisualSelectedRow.BackgroundColor);

                SelectionBox.BackgroundColor = color;
            }
            else
            {
                SelectionBox.BackgroundColor = Color.Transparent;
            }

            // row background
            if (EnabledTrigger != null)
            {
                // row background
                BackgroundColor = ValueSelector.GetBackgroundColor(
                    EnabledTrigger.VisualContainerStyle.BackgroundColor,
                    EnabledTrigger.VisualContainer.BackgroundColor,

                    DataGrid.VisualRowsFromStyle.BackgroundColor,
                    DataGrid.VisualRows.BackgroundColor);
            }
            else
            {
                // Row color 
                BackgroundColor = ValueSelector.GetBackgroundColor(
                    DataGrid.VisualRowsFromStyle.BackgroundColor,
                    DataGrid.VisualRows.BackgroundColor);
            }

            // Priority:
            // 1) selected
            // 2) trigger
            // 3) column
            // 4) default
            foreach (var cell in Cells)
            {
                if (cell.Column.CellTemplate != null)
                    continue;

                if (IsSelected)
                {
                    cell.Wrapper.BackgroundColor = Color.Transparent;

                    // SELECT
                    if (EnabledTrigger == null)
                    {
                        MergeVisual(cell.Label,
                            DataGrid.VisualSelectedRowFromStyle,
                            DataGrid.VisualSelectedRow,
                            cell.Column.VisualCellFromStyle,
                            cell.Column.VisualCell,
                            DataGrid.VisualRowsFromStyle,
                            DataGrid.VisualRows);
                    }
                    // SELECT with TRIGGER
                    else
                    {
                        MergeVisual(cell.Label,
                            DataGrid.VisualSelectedRowFromStyle,
                            DataGrid.VisualSelectedRow,
                            EnabledTrigger.VisualContainerStyle,
                            EnabledTrigger.VisualContainer,
                            cell.Column.VisualCellFromStyle,
                            cell.Column.VisualCell,
                            DataGrid.VisualRowsFromStyle,
                            DataGrid.VisualRows);
                    }
                }
                // TRIGGER
                else if (EnabledTrigger != null)
                {
                    cell.Wrapper.BackgroundColor = Color.Transparent;

                    MergeVisual(cell.Label,
                        EnabledTrigger.VisualContainerStyle,
                        EnabledTrigger.VisualContainer,
                        cell.Column.VisualCellFromStyle,
                        cell.Column.VisualCell,
                        DataGrid.VisualRowsFromStyle,
                        DataGrid.VisualRows);
                }
                // DEFAULT
                else
                {
                    // cell background
                    var color = ValueSelector.GetSelectedColor(
                        cell.Column.VisualCellFromStyle.BackgroundColor,
                        cell.Column.VisualCell.BackgroundColor);
                    color = color.MultiplyAlpha(0.5);
                    cell.Wrapper.BackgroundColor = color;

                    MergeVisual(cell.Label,
                        cell.Column.VisualCellFromStyle,
                        cell.Column.VisualCell,
                        DataGrid.VisualRowsFromStyle,
                        DataGrid.VisualRows);
                }
            }
        }

        public void UpdateAutoNumeric(int index)
        {
            Index = index;

            // Auto numeric
            foreach(var cell in Cells)
            {
                switch (cell.Column.AutoNumber)
                {
                    case Enums.AutoNumberType.Up:
                        //cell.Label.Text = (itemsCount + 1 - num).ToString(cell.Column.StringFormat);
                        cell.Label.Text = (DataGrid.stackList.ItemsCount - index).ToString(cell.Column.StringFormat);
                        break;
                    case Enums.AutoNumberType.Down:
                        cell.Label.Text = (index + 1).ToString(cell.Column.StringFormat);
                        break;
                }
            }
        }

        public void UpdateLineVisibility(bool isVisible)
        {
            if (isLineVisible == isVisible)
                return;

            InvalidateMeasure();

            isLineVisible = isVisible;
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
        internal void RenderRow(double x, double y, double width, double height)
        {
            LayoutChildren(x, y, width, height);
        }

        internal void CallInvalidateMeasure()
        {
            InvalidateMeasure();
            //double result = 0;
            //foreach (var cell in Cells)
            //    result += CalculateCellHeight(cell);

            //return result;
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            double wrap = DataGrid.IsWrapped ? DataGrid.BorderWidth : 0;
            double bw = DataGrid.BorderWidth;

            // Render cells
            double lastX = 0;
            foreach (var cell in Cells)
            {
                if (cell.Column.IsVisible)
                {
                    var rect = new Rectangle(lastX, 0, cell.Column.ActualWidth, height);
                    LayoutChildIntoBoundingRegion(cell.Wrapper, rect);
                    LayoutChildIntoBoundingRegion(cell.Content, rect);
                    lastX += cell.Column.ActualWidth + bw;
                }
                else
                {
                    LayoutChildIntoBoundingRegion(cell.Wrapper, Rectangle.Zero);
                    LayoutChildIntoBoundingRegion(cell.Content, Rectangle.Zero);
                }
            }

            // Selection box
            var rectSelection = new Rectangle(0, 0, width, height + bw);
            LayoutChildIntoBoundingRegion(SelectionBox, rectSelection);

            // Render line
            if (isLineVisible)
            {
                var rect = new Rectangle(0, height - bw, width, height);
                LayoutChildIntoBoundingRegion(Line, rect);
            }
        }

        protected override SizeRequest OnMeasure(double width, double height)
        {
            if (Cells.Count == 0)
                return new SizeRequest();

            RowHeight = 0;
            foreach (var cell in Cells)
            {
                if (!cell.Column.IsVisible)
                    continue;

                double cellHeight = CalculateCellHeight(cell);

                if (RowHeight < cellHeight)
                    RowHeight = cellHeight;
            }

            // Position for line
            if (isLineVisible)
                RowHeight += DataGrid.BorderWidth;

            return new SizeRequest(new Size(width, RowHeight));
        }

        private double CalculateCellHeight(GridCell cell)
        {
            double width = cell.Column.ActualWidth;
            var cellSize = cell.Content.Measure(width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
            return cellSize.Request.Height;
        }

        internal static double CalcWidth(double rowWidth, DataGridColumn col, List<DataGridColumn> cols)
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
                foreach (var c in cols)
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
        #endregion Layout calculation
    }
}
