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
    internal sealed class Row2 : AbsoluteLayout, IGridRow
    {
        public DataGrid DataGrid { get; set; }
        public int Index { get; set; }
        public bool IsSelected { get; set; }
        public object Context { get; set; }
        public View SelectionBox { get; set; }
        public View TouchBox { get; set; }
        public View Line { get; set; }
        public List<GridCell> Cells { get; set; }
        public RowTrigger EnabledTrigger { get; set; }

        public Row2(object context, DataGrid host, int id, int itemsCount)
        {
            Context = context;
            BindingContext = context;
            DataGrid = host;
            Index = id;

            //HeightRequest = 50;
            IsClippedToBounds = true;
            BackgroundColor = Color.Transparent;
            VerticalOptions = LayoutOptions.Start;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Cells = new List<GridCell>();

            // Triggers event
            if (context is INotifyPropertyChanged model)
                model.PropertyChanged += (obj, e) => RowTrigger.SetTriggerStyle(this, e.PropertyName);

            // Selection box
            SelectionBox = new BoxView();
            SelectionBox.HeightRequest = 1.0;
            SelectionBox.BackgroundColor = Color.Transparent;
            SelectionBox.VerticalOptions = LayoutOptions.FillAndExpand;
            SelectionBox.HorizontalOptions = LayoutOptions.FillAndExpand;
            //Children.Add(SelectionBox);

            

            // Init cells
            int i = 0;
            foreach (var column in DataGrid.Columns)
            {
                var cell = new GridCell(column, this, DataGrid);
                Children.Add(cell.Wrapper);
                Cells.Add(cell);
                i++;
            }

            // Create touch box
            TouchBox = new TouchBox(BindingContext, DataGrid, ActionRowSelect);
            //Children.Add(TouchBox);

            // Create horizontal line table
            Line = CreateHorizontalLine();
            //SetRow(Line, 1);
            //SetColumnSpan(Line, DataGrid.Columns.Count);
            //Children.Add(Line);

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

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (!isSolve)
                return;

            foreach (var cell in Cells)
            {
                double x = CalcX(Width, cell.Index);
                double y = 0;
                double w = CalcWidthPercent(Width, cell.Index);
                double h = AutoSize;

                SetLayoutBounds(cell.Wrapper, new Rectangle(x, y, w, h));
                //Children.Add(cell.Wrapper);
            }
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

                    cell.Wrapper.BackgroundColor = Color.Transparent;
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
                    // row background
                    cell.Wrapper.BackgroundColor = ValueSelector.GetSelectedColor(
                        cell.Column.VisualCellFromStyle.BackgroundColor,
                        cell.Column.VisualCell.BackgroundColor);

                    MergeVisual(cell.Label,
                        cell.Column.VisualCellFromStyle,
                        cell.Column.VisualCell,
                        DataGrid.VisualRowsFromStyle,
                        DataGrid.VisualRows);
                }
            }
        }

        private double height = -1;
        internal bool isSolve = true;
        public void UpdateHeight(GridCell from, double tryHeight)
        {
            if (tryHeight == height)
                return;

            double h = -1;
            foreach (var item in Cells)
            {
                if (!item.IsSizeDone)
                    return;

                if (item.Wrapper.Height > h)
                    h = item.Wrapper.Height;
            }

            isSolve = false;
            foreach (var item in Cells)
            {
                item.Wrapper.HeightRequest = h;
                item.IsSizeDone = false;
            }
            HeightRequest = h;
            isSolve = true;

            height = h;
        }

        public void UpdateAutoNumeric(int num, int itemsCount)
        {
            // Auto numeric
            foreach(var cell in Cells)
            {
                switch (cell.AutoNumber)
                {
                    case Enums.AutoNumberType.Up:
                        cell.Label.Text = (itemsCount + 1 - num).ToString(cell.Column.StringFormat);
                        break;
                    case Enums.AutoNumberType.Down:
                        cell.Label.Text = num.ToString(cell.Column.StringFormat);
                        break;
                    //default:
                    //    break;
                }
            }
        }

        public void UpdateLineVisibility(bool isVisible)
        {
            //if (Line.IsVisible != isVisible)
            //{
            //    if (isVisible)
            //    {
            //        RowDefinitions[1].Height = DataGrid.BorderWidth;
            //    }
            //    else
            //    {
            //        RowDefinitions[1].Height = new GridLength(0.0);
            //    }
            //}

            //Line.IsVisible = isVisible;
        }

        public void UpdateCellVisibility(int cellId, bool isVisible)
        {
            //ColumnDefinitions[cellId].Width = Cells[cellId].Column.CalcWidth;
            //Cells[cellId].Wrapper.IsVisible = isVisible;
        }

        private BoxView CreateHorizontalLine()
        {
            var line = new BoxView()
            {
                BackgroundColor = DataGrid.BorderColor,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = DataGrid.BorderWidth,
                InputTransparent = true,
            };
            return line;
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

        private double CalcWidthPercent(double width, int colId)
        {
            var col = DataGrid.Columns[colId];

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
                    if (c.Width.IsStar)
                        com += c.Width.Value;
                    else
                        dif += c.Width.Value;

                return (width - dif) * (col.Width.Value / com);
            }
        }

        private double CalcX(double width, int colId)
        {
            var col = DataGrid.Columns[colId];
            double sum = 0;
            for (int i = (colId - 1); i >= 0; i--)
                sum += DataGrid.Columns[i].FacticalWidth;

            if (!col.IsVisible)
            {
                return sum;
            }
            else if (col.Width.IsStar)
            {
                double dif = 0;
                double com = 0;
                foreach (var c in DataGrid.Columns)
                    if (c.Width.IsStar)
                        com += c.Width.Value;
                    else
                        dif += c.Width.Value;

                double final = col.Width.Value / com;
                col.FacticalWidth = (width - dif) * final;

                if (colId == 0)
                    return 0;

                return sum;
            }
            else
            {
                col.FacticalWidth = col.Width.Value;

                if (colId == 0)
                    return 0;

                return sum;
            }
        }
    }
}
