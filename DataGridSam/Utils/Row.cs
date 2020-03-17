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
    internal sealed class Row : Grid, IDefinition
    {
        internal BoxView line;
        internal bool isSelected;
        internal List<GridCell> cells = new List<GridCell>();
        internal RowTrigger enableTrigger;
        internal bool isStyleDefault = true;

        // Data grid (host)
        public static readonly BindableProperty DataGridProperty =
            BindableProperty.Create(nameof(DataGrid), typeof(DataGrid), typeof(Row), null,
                propertyChanged: (b, o, n) =>
                {
                    (b as Row).CreateRow();
                });
        public DataGrid DataGrid
        {
            get { return (DataGrid)GetValue(DataGridProperty); }
            set { SetValue(DataGridProperty, value); }
        }

        protected override void OnBindingContextChanged()
        {
            // Triggers event
            if (BindingContext is INotifyPropertyChanged model)
                model.PropertyChanged += (obj, e) => RowTrigger.SetTriggerStyle(this, e.PropertyName);

            // Set binding context for custom cells
            foreach (var item in cells)
            {
                if (item.IsCustomTemplate && item.Wrapper?.Content != null)
                    item.Wrapper.Content.BindingContext = BindingContext;
            }

            // Started find FIRST active trigger
            if (this.DataGrid.RowTriggers.Count > 0)
            {
                foreach (var trigg in this.DataGrid.RowTriggers)
                {
                    if (trigg.CheckTriggerActivated(BindingContext))
                    {
                        this.enableTrigger = trigg;
                        break;
                    }
                    //var trigger = RowTrigger.SetTriggerStyle(this, trigg.PropertyTrigger, false);
                    //if (trigger != null)
                    //{
                    //    this.enableTrigger = trigger;
                    //    break;
                    //}
                }
            }

            // Set text value for standart cell
            foreach (var item in cells)
            {
                if (item.IsCustomTemplate || item.IsAutoNumber)
                    continue;

                item.Label.SetBinding(Label.TextProperty, new Binding(item.Column.PropertyName, BindingMode.Default, 
                    stringFormat: item.Column.StringFormat, source: BindingContext));
            }

            // Render first style
            UpdateStyle();
        }

        private void CreateRow()
        {
            RowSpacing = 0;
            ColumnSpacing = 0;
            IsClippedToBounds = true;
            BackgroundColor = Color.Transparent;

            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = GridLength.Auto },
            };

            int index = 0;

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
                if (column.IsAutoNumber)
                    cell.IsAutoNumber = true;

                SetColumn(cell.Wrapper, index);
                SetRow(cell.Wrapper, 0);
                Children.Add(cell.Wrapper);
                cells.Add(cell);


                index++;
            }


            // Create horizontal line table
            line = CreateHorizontalLine();
            SetRow(line, 1);
            SetColumn(line, 0);
            SetColumnSpan(line, DataGrid.Columns.Count);
            Children.Add(line);

            // Add tap system event
            // Set only tap command, setting CommandParameter - after changed "RowContext" :)
            Touch.SetSelect(this, new Command(ActionRowSelect));
            Touch.SetColor(this, DataGrid.TapColor);

            // Add tap event
            if (DataGrid.CommandSelectedItem != null)
                Touch.SetTap(this, DataGrid.CommandSelectedItem);

            // Add long tap event
            if (DataGrid.CommandLongTapItem != null)
                Touch.SetLongTap(this, DataGrid.CommandLongTapItem);
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

        internal void UpdateAutoNumeric(int num)
        {
            // Auto numeric
            foreach(var cell in cells)
            {
                if (cell.IsAutoNumber)
                    cell.Label.Text = num.ToString(cell.Column.StringFormat);
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
