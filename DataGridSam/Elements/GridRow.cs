﻿using DataGridSam.Platform;
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
    internal sealed class GridRow : Layout<View>, IGridRow
    {
        private StackList stackList;
        private bool isLineVisible;
        private double rowHeight = -1;

        public DataGrid DataGrid { get; set; }
        public int Index { get; set; }
        public bool IsSelected { get; set; }
        public object Context { get; set; }
        public View SelectionBox { get; set; }
        public View TouchBox { get => this; set { } }
        public View Line { get; set; }
        public List<GridCell> Cells { get; set; }
        public RowTrigger EnabledTrigger { get; set; }

        public GridRow(object context, StackList host, int id, int itemsCount, bool isLineVisible)
        {
            Context = context;
            BindingContext = context;
            DataGrid = host.DataGrid;
            Index = id;
            this.isLineVisible = isLineVisible;
            stackList = host;

            IsClippedToBounds = true;
            VerticalOptions = LayoutOptions.Start;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Cells = new List<GridCell>();

            // Triggers event
            if (context is INotifyPropertyChanged model)
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
                var cell = new GridCell(column, this, DataGrid);

                Children.Add(cell.Wrapper);
                Children.Add(cell.Content);
                Cells.Add(cell);
                i++;
            }

            // Add tap system event
            Touch.SetSelect(this, new Command(ActionRowSelect));

            if (DataGrid.TapColor != Color.Default)
                Touch.SetColor(this, DataGrid.TapColor);

            if (DataGrid.CommandSelectedItem != null)
                Touch.SetTap(this, DataGrid.CommandSelectedItem);

            if (DataGrid.CommandLongTapItem != null)
                Touch.SetLongTap(this, DataGrid.CommandLongTapItem);


            // Create horizontal line table
            Line = new BoxView();
            Line.BackgroundColor = DataGrid.BorderColor;
            Line.HeightRequest = DataGrid.BorderWidth;
            Line.InputTransparent = true;
            Children.Add(Line);

            // Auto number
            if (DataGrid.IsAutoNumberCalc)
                UpdateAutoNumeric(Index + 1, itemsCount);

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

        private void ActionRowSelect(object param)
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
                if (cell.IsCustomTemplate)
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

        public void UpdateAutoNumeric(int num, int itemsCount)
        {
            // Auto numeric
            foreach(var cell in Cells)
            {
                switch (cell.Column.AutoNumber)
                {
                    case Enums.AutoNumberType.Up:
                        cell.Label.Text = (itemsCount + 1 - num).ToString(cell.Column.StringFormat);
                        break;
                    case Enums.AutoNumberType.Down:
                        cell.Label.Text = num.ToString(cell.Column.StringFormat);
                        break;
                }
            }
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
            var currentCell = Cells[cellId];

            if (isVisible)
            {
                Children.Add(currentCell.Wrapper);
                Children.Add(currentCell.Content);
            }
            else
            {
                Children.Remove(currentCell.Wrapper);
                Children.Remove(currentCell.Content);
            }

            double height = 0.0;
            foreach (var cell in Cells)
            {
                if (cell.Column.IsVisible)
                {
                    double h = CalculateCellHeight(cell, Width);
                    if (height < h)
                        height = h;
                }
            }

            LayoutChildren(0, 0, Width, height);
            //foreach (var cell in Cells)
            //{
            //    if (cell.Column.IsVisible)
            //        RenderCellOnLayout(cell, Width, height);
            //}
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
            if (rowHeight > 0)
                height = rowHeight;

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

            if (rowHeight > 0)
                HeightRequest = height;
        }

        protected override SizeRequest OnMeasure(double width, double height)
        {
            if (Cells.Count == 0)
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

            rowHeight = actualHeight;
            return new SizeRequest(new Size(width, actualHeight));
        }

        private double CalculateCellHeight(GridCell cell, double rowWidth)
        {
            double width = CalcWidth(rowWidth, cell.Column);
            var cellSize = cell.Content.Measure(width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
            return cellSize.Request.Height;
        }

        private void RenderCellOnLayout(GridCell cell, double rowWidth, double rowHeight)
        {
            double w = CalcWidth(rowWidth, cell.Column);
            double x = CalcX(rowWidth, cell.Column);
            var rect = new Rectangle(x, 0, w, rowHeight);

            LayoutChildIntoBoundingRegion(cell.Wrapper, rect);
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
