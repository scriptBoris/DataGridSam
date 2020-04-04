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
    internal sealed class GridRow : Grid
    {
        internal readonly DataGrid DataGrid;
        internal int index;
        internal TouchBox touchBox;
        internal BoxView selectionBox;
        internal BoxView line;
        internal bool isSelected;
        internal List<GridCell> cells = new List<GridCell>();
        internal RowTrigger enableTrigger;
        internal bool isStyleDefault = true;

        public GridRow(object context, DataGrid host, int id, int itemsCount)
        {
            BindingContext = context;
            DataGrid = host;
            index = id;

            RowSpacing = 0;
            ColumnSpacing = 0;
            IsClippedToBounds = true;
            BackgroundColor = Color.Transparent;

            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Star },
                //new RowDefinition { Height = DataGrid.BorderWidth },
                new RowDefinition { Height = GridLength.Auto },
            };

            // Triggers event
            if (context is INotifyPropertyChanged model)
                model.PropertyChanged += (obj, e) => RowTrigger.SetTriggerStyle(this, e.PropertyName);

            // Selection box
            selectionBox = new BoxView();
            selectionBox.HeightRequest = 1.0;
            selectionBox.BackgroundColor = Color.Transparent;
            selectionBox.VerticalOptions = LayoutOptions.FillAndExpand;
            selectionBox.HorizontalOptions = LayoutOptions.FillAndExpand;
            SetColumnSpan(selectionBox, DataGrid.Columns.Count);
            Children.Add(selectionBox);

            //// Add tap system event
            //Touch.SetSelect(touchBox, new Command(ActionRowSelect));

            //if (DataGrid.TapColor != Color.Default)
            //    Touch.SetColor(touchBox, DataGrid.TapColor);

            //if (DataGrid.CommandSelectedItem != null)
            //    Touch.SetTap(touchBox, DataGrid.CommandSelectedItem);

            //if (DataGrid.CommandLongTapItem != null)
            //    Touch.SetLongTap(touchBox, DataGrid.CommandLongTapItem);

            // Init cells
            int i = 0;
            foreach (var column in DataGrid.Columns)
            {
                ColumnDefinitions.Add(new ColumnDefinition() { Width = column.CalcWidth });

                var cell = new GridCell(column, this, DataGrid);
                cells.Add(cell);

                // add wrapper
                Children.Add(cell.Wrapper, i, 0);

                // add content
                Children.Add(cell.Content, i, 0);

                i++;
            }

            // Create touch box
            touchBox = new TouchBox(BindingContext, DataGrid, ActionRowSelect);
            SetColumnSpan(touchBox, DataGrid.Columns.Count);
            Children.Add(touchBox);

            // Create horizontal line table
            line = CreateHorizontalLine();
            SetRow(line, 1);
            SetColumnSpan(line, DataGrid.Columns.Count);
            Children.Add(line);

            // Auto number
            if (DataGrid.IsAutoNumberCalc)
                UpdateAutoNumeric(index + 1, itemsCount);

            // Started find FIRST active trigger
            if (this.DataGrid.RowTriggers.Count > 0)
                foreach (var trigg in this.DataGrid.RowTriggers)
                    if (trigg.CheckTriggerActivated(BindingContext))
                    {
                        this.enableTrigger = trigg;
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
                lastTapped.isSelected = false;
                lastTapped.UpdateStyle();
            }

            // GUI Selected row
            if (lastTapped != rowTapped)
            {
                DataGrid.SelectedRow = rowTapped;
                DataGrid.SelectedItem = BindingContext;

                rowTapped.isSelected = true;
                rowTapped.UpdateStyle();
            }
        }

        internal void UpdateStyle()
        {
            // Selected
            if (isSelected)
            {
                var color = ValueSelector.GetSelectedColor(
                    DataGrid.VisualSelectedRowFromStyle.BackgroundColor,
                    DataGrid.VisualSelectedRow.BackgroundColor);

                selectionBox.BackgroundColor = color;
            }
            else
            {
                selectionBox.BackgroundColor = Color.Transparent;
            }

            // row background
            if (enableTrigger != null)
            {
                // row background
                BackgroundColor = ValueSelector.GetBackgroundColor(
                    enableTrigger.VisualContainerStyle.BackgroundColor,
                    enableTrigger.VisualContainer.BackgroundColor,

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
            foreach (var cell in cells)
            {
                if (cell.IsCustomTemplate)
                    continue;

                if (isSelected)
                {
                    // SELECT
                    if (enableTrigger == null)
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
                            enableTrigger.VisualContainerStyle,
                            enableTrigger.VisualContainer,
                            cell.Column.VisualCellFromStyle,
                            cell.Column.VisualCell,
                            DataGrid.VisualRowsFromStyle,
                            DataGrid.VisualRows);
                    }

                    cell.Wrapper.BackgroundColor = Color.Transparent;
                }
                // TRIGGER
                else if (enableTrigger != null)
                {
                    cell.Wrapper.BackgroundColor = Color.Transparent;

                    MergeVisual(cell.Label,
                        enableTrigger.VisualContainerStyle,
                        enableTrigger.VisualContainer,
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

        internal void UpdateAutoNumeric(int num, int itemsCount)
        {
            // Auto numeric
            foreach(var cell in cells)
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
    }
}
