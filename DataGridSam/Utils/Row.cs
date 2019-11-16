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
        internal bool isSelected;
        internal List<GridCell> cells = new List<GridCell>();
        internal RowTrigger enableTrigger;
        internal bool isStyleDefault = true;

        // Data grid sam
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
            var click = (TapGestureRecognizer)this.GestureRecognizers.FirstOrDefault();
            // Triggers
            if (BindingContext is INotifyPropertyChanged model)
                model.PropertyChanged += (obj, e) => RowTrigger.TrySetTriggerStyleRow(this, e.PropertyName);

            if (this.DataGrid.RowTriggers.Count > 0)
            {
                foreach (var trigg in this.DataGrid.RowTriggers)
                {
                    RowTrigger.TrySetTriggerStyleRow(this, trigg.PropertyTrigger);
                }
            }

            foreach (var item in cells)
            {
                if (item.IsCustomTemplate)
                    continue;

                item.Label.SetBinding(Label.TextProperty, new Binding(item.Column.PropertyName, BindingMode.Default, 
                    stringFormat: item.Column.StringFormat, source: BindingContext));
            }
            click.CommandParameter = BindingContext;
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

                if (column.CellTemplate != null)
                {
                    cell.Wrapper = new ContentView() { Content = column.CellTemplate.CreateContent() as View };
                    cell.IsCustomTemplate = true;
                }
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
                    //label.SetBinding(Label.TextProperty, new Binding(column.PropertyName, BindingMode.Default, stringFormat: column.StringFormat));
                    //text.SetBinding(Label.FontSizeProperty, new Binding(DataGrid.FontSizeProperty.PropertyName, BindingMode.Default, source: DataGrid));
                    //text.SetBinding(Label.FontFamilyProperty, new Binding(DataGrid.FontFamilyProperty.PropertyName, BindingMode.Default, source: DataGrid));

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

            // Set default style
            SetStyleDefault();

            // Create horizontal line table
            var line = CreateHorizontalLine();
            SetRow(line, 1);
            SetColumn(line, 0);
            SetColumnSpan(line, DataGrid.Columns.Count);
            Children.Add(line);

            // Add tap event
            // Set only tap command, setting CommandParameter - after changed "RowContext" :)
            var tapControll = new TapGestureRecognizer();
            tapControll.Tapped += RowSelected;
            GestureRecognizers.Add(tapControll);
        }

        private void RowSelected(object sender, EventArgs e)
        {
            var self = (Row)sender;
            // Avoid useless actions
            if (self.DataGrid.SelectedRow != this)
            {
                // GUI Unselected last row
                var lastRow = self.DataGrid.SelectedRow;
                if (lastRow != null)
                {
                    lastRow.isSelected = false;
                    if (lastRow.enableTrigger == null)
                    {
                        lastRow.SetStyleDefault();
                    }
                    // If last row has trigger
                    else
                    {
                        RowTrigger.SetTriggerStyleRow(lastRow, lastRow.enableTrigger);
                    }
                }

                // GUI Selected row
                self.DataGrid.SelectedRow = self;
                self.DataGrid.SelectedItem = self.BindingContext;
                self.isSelected = true;
                self.SetStyleSelected();
            }

            // Run ICommand selected item
            self.DataGrid.CommandSelectedItem?.Execute(self.BindingContext);
        }

        internal void SetStyleDefault()
        {
            isStyleDefault = true;
            foreach (var item in cells)
            {
                if (item.IsCustomTemplate)
                    continue;

                item.Label.TextColor = item.Column.CellTextColorNullable ?? DataGrid.RowsTextColor;
                item.Wrapper.BackgroundColor = item.Column.CellBackgroundColorNullable ?? DataGrid.RowsColor;
            }
        }

        internal void SetStyleSelected()
        {
            if (enableTrigger != null)
                SetStyleDefault();

            foreach (var item in cells)
            {
                if (item.IsCustomTemplate)
                    continue;

                item.Wrapper.BackgroundColor = DataGrid.SelectedRowColor;
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
