using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    internal sealed class Row : Grid
    {
        internal Type bindingTypeModel;
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
            // Get binding model type
            bindingTypeModel = BindingContext.GetType();


            // Triggers event
            if (BindingContext is INotifyPropertyChanged model)
                model.PropertyChanged += (obj, e) => RowTrigger.TrySetTriggerStyleRow(this, e.PropertyName);

            // Started find FIRST active trigger
            if (this.DataGrid.RowTriggers.Count > 0)
            {
                foreach (var trigg in this.DataGrid.RowTriggers)
                {
                    var trigger = RowTrigger.TrySetTriggerStyleRow(this, trigg.PropertyTrigger, false);
                    if (trigger != null)
                    {
                        this.enableTrigger = trigger;
                        break;
                    }
                }
            }

            // Set text value for standart cell
            foreach (var item in cells)
            {
                if (item.IsCustomTemplate)
                    continue;

                item.Label.SetBinding(Label.TextProperty, new Binding(item.Column.PropertyName, BindingMode.Default, 
                    stringFormat: item.Column.StringFormat, source: BindingContext));
            }

            // Add command parameter
            var click = (TapGestureRecognizer)this.GestureRecognizers.FirstOrDefault();
            click.CommandParameter = BindingContext;

            // Render first style
            UpdateStyle();
        }

        private void CreateRow()
        {
            RowSpacing = 0;
            ColumnSpacing = DataGrid.BorderWidth;

            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = new GridLength(DataGrid.BorderWidth) },
            };

            int index = 0;

            foreach (var column in DataGrid.Columns)
            {
                ColumnDefinitions.Add(new ColumnDefinition() { Width = column.Width });

                var cell = new GridCell
                {
                    Column = column,
                };

                // Create custom template
                if (column.CellTemplate != null)
                {
                    cell.Wrapper = new ContentView() { Content = column.CellTemplate.CreateContent() as View };
                    cell.IsCustomTemplate = true;
                }
                // Create standart cell
                else
                {
                    var label = new Label
                    {
                        FontSize = DataGrid.RowsFontSize,
                        HorizontalOptions = column.HorizontalContentAlignment,
                        VerticalOptions = column.VerticalContentAlignment,
                        HorizontalTextAlignment = column.HorizontalTextAlignment,
                        VerticalTextAlignment = column.VerticalTextAlignment,
                        LineBreakMode = LineBreakMode.WordWrap,
                    };

                    cell.Wrapper = new ContentView
                    {
                        Padding = DataGrid.CellPadding,
                        Content = label,
                    };
                    cell.Label = label;
                }

                SetColumn(cell.Wrapper, index);
                SetRow(cell.Wrapper, 0);
                Children.Add(cell.Wrapper);
                cells.Add(cell);

                index++;
            }

            // Create horizontal line table
            var line = CreateHorizontalLine();
            SetRow(line, 1);
            SetColumn(line, 0);
            SetColumnSpan(line, DataGrid.Columns.Count);
            Children.Add(line);

            // Add tap event
            // Set only tap command, setting CommandParameter - after changed "RowContext" :)
            var tapControll = new TapGestureRecognizer();
            tapControll.Tapped += RowTapped;
            GestureRecognizers.Add(tapControll);
        }

        private void RowTapped(object sender, EventArgs e)
        {
            var rowTapped = (Row)sender;
            // Avoid useless actions
            if (DataGrid.SelectedRow != this)
            {
                // GUI Unselected last row
                var lastRow = DataGrid.SelectedRow;
                if (lastRow != null)
                {
                    lastRow.isSelected = false;
                    lastRow.UpdateStyle();
                }

                // system block prop changed
                //DataGrid.blockThrowPropChanged = true;

                // GUI Selected row
                DataGrid.SelectedRow = rowTapped;
                DataGrid.SelectedItem = BindingContext;

                rowTapped.isSelected = true;
                rowTapped.UpdateStyle();

                // return enable system block prop changed
                //DataGrid.blockThrowPropChanged = false;
            }

            // Run ICommand selected item
            DataGrid.CommandSelectedItem?.Execute(BindingContext);
        }

        internal void UpdateStyle()
        {
            // priority:
            // selected
            // trigger row
            // cell
            // common
            foreach (var item in cells)
            {
                if (item.IsCustomTemplate)
                    continue;

                if (isSelected)
                {
                    // SELECT
                    if (enableTrigger == null)
                    {
                        // row background
                        item.Wrapper.BackgroundColor = ValueSelector.Color(DataGrid.SelectedRowColor, DataGrid.RowsColor);

                        // text color
                        item.Label.TextColor = ValueSelector.Color(DataGrid.SelectedRowTextColor, DataGrid.RowsTextColor);

                        // font attribute
                        item.Label.FontAttributes = ValueSelector.FontAttribute(DataGrid.SelectedRowAttribute, DataGrid.RowsFontAttribute);
                    }
                    // SELECT with TRIGGER
                    else
                    {
                        // row background
                        item.Wrapper.BackgroundColor = ValueSelector.Color(
                            DataGrid.SelectedRowColor,
                            enableTrigger.RowBackgroundColor,  
                            DataGrid.RowsColor);

                        // text color
                        item.Label.TextColor = ValueSelector.Color(
                            DataGrid.SelectedRowTextColor,
                            enableTrigger.RowTextColor,
                            DataGrid.RowsTextColor);

                        // font attribute
                        item.Label.FontAttributes = ValueSelector.FontAttribute(
                            DataGrid.SelectedRowAttribute, 
                            enableTrigger.RowTextAttribute,
                            DataGrid.RowsFontAttribute);
                    }
                }
                // TRIGGER
                else if (enableTrigger != null)
                {
                    // row background
                    item.Wrapper.BackgroundColor = ValueSelector.Color(
                        enableTrigger.RowBackgroundColor,
                        DataGrid.RowsColor);

                    // text color
                    item.Label.TextColor = ValueSelector.Color(
                        enableTrigger.RowTextColor,
                        DataGrid.RowsTextColor);

                    // font attribute
                    item.Label.FontAttributes = ValueSelector.FontAttribute(
                        enableTrigger.RowTextAttribute,
                        DataGrid.RowsFontAttribute);
                }
                // DEFAULT
                else
                {
                    // row background
                    item.Wrapper.BackgroundColor = DataGrid.RowsColor;
                    // text color
                    item.Label.TextColor = DataGrid.RowsTextColor;
                    // font attribute
                    item.Label.FontAttributes = DataGrid.RowsFontAttribute;
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
            };
            return line;
        }
    }
}
