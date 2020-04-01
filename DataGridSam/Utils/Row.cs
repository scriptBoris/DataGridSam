using DataGridSam.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
//using TouchSam;
//using DataGridSam.Platform;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    internal sealed class Row : Grid
    {
        internal readonly DataGrid DataGrid;
        internal int index;
        internal BoxView selectionContainer;
        internal BoxView line;
        internal bool isSelected;
        internal List<GridCell> cells = new List<GridCell>();
        internal RowTrigger enableTrigger;
        internal bool isStyleDefault = true;

        public Row(object context, DataGrid host, int id, int itemsCount)
        {
            BindingContext = context;
            DataGrid = host;
            index = id;

            RowSpacing = 0;
            ColumnSpacing = 0;
            //IsClippedToBounds = true;
            BackgroundColor = Color.Transparent;

            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Auto },
            };

            // Triggers event
            if (context is INotifyPropertyChanged model)
                model.PropertyChanged += (obj, e) => RowTrigger.SetTriggerStyle(this, e.PropertyName);

            // Create selected container
            selectionContainer = CreateSelectionContainer();
            SetColumnSpan(selectionContainer, DataGrid.Columns.Count);
            Children.Add(selectionContainer);

            // Init cells
            int cellsCount = 0;
            foreach (var column in DataGrid.Columns)
            {
                ColumnDefinitions.Add(new ColumnDefinition() { Width = column.CalcWidth });

                var cell = new GridCell { Column = column };

                // Create custom template
                if (column.CellTemplate != null)
                {
                    cell.View = column.CellTemplate.CreateContent() as View;
                    cell.View.IsVisible = column.IsVisible;
                    cell.View.InputTransparent = true;
                    cell.View.BindingContext = context;
                    cell.IsCustomTemplate = true;

                    if (cell.View is Layout layout)
                    {
                        layout.IsClippedToBounds = true;
                        layout.InputTransparent = true;
                        layout.CascadeInputTransparent = true;
                    }
                }
                // Create standart cell
                else
                {
                    var label = new Label
                    {
                        IsVisible = column.IsVisible,
                        Margin = DataGrid.CellPadding,
                        InputTransparent = true,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                    };

                    if (column.PropertyName != null)
                        label.SetBinding(Label.TextProperty, new Binding(
                            column.PropertyName, 
                            BindingMode.Default,
                            stringFormat: column.StringFormat, 
                            source: context));

                    cell.View = label;

                }

                // Detect auto number cell
                cell.AutoNumber = column.AutoNumber;

                SetColumn(cell.View, cellsCount);
                SetRow(cell.View, 0);
                Children.Add(cell.View);
                cells.Add(cell);

                cellsCount++;
            }

            // Create horizontal line table
            line = CreateHorizontalLine();
            SetRow(line, 1);
            SetColumn(line, 0);
            SetColumnSpan(line, cellsCount);
            Children.Add(line);

            // Add tap system event
            Touch.SetSelect(this, new Command(ActionRowSelect));

            if (DataGrid.TapColor != Color.Default)
                Touch.SetColor(this, DataGrid.TapColor);

            if (DataGrid.CommandSelectedItem != null)
                Touch.SetTap(this, DataGrid.CommandSelectedItem);

            if (DataGrid.CommandLongTapItem != null)
                Touch.SetLongTap(this, DataGrid.CommandLongTapItem);

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

                selectionContainer.BackgroundColor = color;
            }
            else
            {
                selectionContainer.BackgroundColor = Color.Transparent;
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
                }
                // TRIGGER
                else if (enableTrigger != null)
                {
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
            };
            return line;
        }

        private BoxView CreateSelectionContainer()
        {
            var line = new BoxView()
            {
                BackgroundColor = Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
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
