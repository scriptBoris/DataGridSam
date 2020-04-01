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

            // Init cells
            int i = 0;
            foreach (var column in DataGrid.Columns)
            {
                ColumnDefinitions.Add(new ColumnDefinition() { Width = column.CalcWidth });

                var cell = new GridCell { Column = column };

                // Create custom template
                if (column.CellTemplate != null)
                {
                    cell.Wrapper = new ContentView();
                    cell.Wrapper.IsVisible = column.IsVisible;
                    cell.Wrapper.IsClippedToBounds = true;
                    cell.Wrapper.InputTransparent = true;
                    cell.Wrapper.CascadeInputTransparent = true;
                    cell.Wrapper.Content = column.CellTemplate.CreateContent() as View;
                    cell.Wrapper.Content.InputTransparent = true;
                    cell.Wrapper.BindingContext = context;
                    cell.IsCustomTemplate = true;
                }
                // Create standart cell
                else
                {
                    var label = new Label
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                    };

                    if (column.PropertyName != null)
                        label.SetBinding(Label.TextProperty, new Binding(
                            column.PropertyName, 
                            BindingMode.Default,
                            stringFormat: column.StringFormat, 
                            source: context));

                    var wrapper = new ContentView
                    {
                        IsVisible = column.IsVisible,
                        Padding = DataGrid.CellPadding,
                        IsClippedToBounds = true,
                        Content = label,
                    };

                    cell.Wrapper = wrapper;
                    cell.Wrapper.InputTransparent = true;
                    cell.Label = label;

                }

                // Detect auto number cell
                cell.AutoNumber = column.AutoNumber;

                SetColumn(cell.Wrapper, i);
                SetRow(cell.Wrapper, 0);
                Children.Add(cell.Wrapper);
                cells.Add(cell);

                i++;
            }

            // Create horizontal line table
            line = CreateHorizontalLine();
            SetRow(line, 1);
            SetColumn(line, 0);
            SetColumnSpan(line, DataGrid.Columns.Count);
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
            // Priority:
            // 1) selected
            // 2) trigger
            // 3) column
            // 4) default
            foreach (var cell in cells)
            {
                if (isSelected)
                {
                    // SELECT
                    if (enableTrigger == null)
                    {
                        // row background
                        cell.Wrapper.BackgroundColor = ValueSelector.GetBackgroundColor(
                            DataGrid.VisualSelectedRowFromStyle.BackgroundColor,
                            DataGrid.VisualSelectedRow.BackgroundColor,

                            cell.Column.VisualCellFromStyle.BackgroundColor,
                            cell.Column.VisualCell.BackgroundColor,

                            DataGrid.VisualRowsFromStyle.BackgroundColor,
                            DataGrid.VisualRows.BackgroundColor);

                        if (!cell.IsCustomTemplate)
                        {
                            MergeVisual(cell.Label,
                                DataGrid.VisualSelectedRowFromStyle,
                                DataGrid.VisualSelectedRow,
                                cell.Column.VisualCellFromStyle,
                                cell.Column.VisualCell,
                                DataGrid.VisualRowsFromStyle,
                                DataGrid.VisualRows);
                        }
                    }
                    // SELECT with TRIGGER
                    else
                    {
                        // row background
                        cell.Wrapper.BackgroundColor = ValueSelector.GetBackgroundColor(
                            DataGrid.VisualSelectedRowFromStyle.BackgroundColor,
                            DataGrid.VisualSelectedRow.BackgroundColor,

                            enableTrigger.VisualContainerStyle.BackgroundColor,
                            enableTrigger.VisualContainer.BackgroundColor,

                            cell.Column.VisualCellFromStyle.BackgroundColor,
                            cell.Column.VisualCell.BackgroundColor,

                            DataGrid.VisualRowsFromStyle.BackgroundColor,
                            DataGrid.VisualRows.BackgroundColor);

                        if (!cell.IsCustomTemplate)
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
                }
                // TRIGGER
                else if (enableTrigger != null)
                {
                    // row background
                    cell.Wrapper.BackgroundColor = ValueSelector.GetBackgroundColor(
                        enableTrigger.VisualContainerStyle.BackgroundColor,
                        enableTrigger.VisualContainer.BackgroundColor,
                        
                        cell.Column.VisualCellFromStyle.BackgroundColor,
                        cell.Column.VisualCell.BackgroundColor,

                        DataGrid.VisualRowsFromStyle.BackgroundColor,
                        DataGrid.VisualRows.BackgroundColor);

                    if (!cell.IsCustomTemplate)
                    {
                        MergeVisual(cell.Label,
                            enableTrigger.VisualContainerStyle,
                            enableTrigger.VisualContainer,
                            cell.Column.VisualCellFromStyle,
                            cell.Column.VisualCell,
                            DataGrid.VisualRowsFromStyle,
                            DataGrid.VisualRows);
                    }
                }
                // DEFAULT
                else
                {
                    // row background
                    cell.Wrapper.BackgroundColor = ValueSelector.GetBackgroundColor(
                        cell.Column.VisualCellFromStyle.BackgroundColor,
                        cell.Column.VisualCell.BackgroundColor,

                        DataGrid.VisualRowsFromStyle.BackgroundColor,
                        DataGrid.VisualRows.BackgroundColor);

                    if (!cell.IsCustomTemplate)
                    {
                        MergeVisual(cell.Label,
                            cell.Column.VisualCellFromStyle,
                            cell.Column.VisualCell,
                            DataGrid.VisualRowsFromStyle,
                            DataGrid.VisualRows);
                    }
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
            var line = new BoxView
            {
                BackgroundColor = DataGrid.BorderColor,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = DataGrid.BorderWidth,
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
