using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    internal sealed class StackCell : Grid
    {
        private Color textColor = Color.Black;

        // Data grid sam
        public static readonly BindableProperty DataGridProperty =
            BindableProperty.Create(nameof(DataGrid), typeof(DataGrid), typeof(StackCell), null,
                propertyChanged: (b, o, n) =>
                {
                    (b as StackCell).CreateView();
                });
        public DataGrid DataGrid
        {
            get { return (DataGrid)GetValue(DataGridProperty); }
            set { SetValue(DataGridProperty, value); }
        }

        // Row context
        public static readonly BindableProperty RowContextProperty =
            BindableProperty.Create(nameof(RowContext), typeof(object), typeof(StackCell),
                propertyChanged: (b, o, n) =>
                {
                    var self = (StackCell)b;
                    var click = (TapGestureRecognizer)self.GestureRecognizers.FirstOrDefault();
                    click.CommandParameter = n;
                });
        public object RowContext
        {
            get { return GetValue(RowContextProperty); }
            set { SetValue(RowContextProperty, value); }
        }

        private void CreateView()
        {
            RowSpacing = 0;
            ColumnSpacing = DataGrid.LinesWidth;

            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = new GridLength(DataGrid.LinesWidth) },
            };

            int index = 0;
            foreach (var column in DataGrid.Columns)
            {
                ColumnDefinitions.Add(new ColumnDefinition() { Width = column.Width });
                ContentView cell;
                if (column.CellTemplate != null)
                {
                    cell = new ContentView() { Content = column.CellTemplate.CreateContent() as View };
                    //if (column.PropertyName != null)
                    //{
                    //    cell.SetBinding(BindingContextProperty, new Binding(column.PropertyName));
                    //}
                }
                else
                {
                    var label = new Label
                    {
                        TextColor = textColor,
                        HorizontalOptions = column.HorizontalContentAlignment,
                        VerticalOptions = column.VerticalContentAlignment,
                        HorizontalTextAlignment = column.HorizontalTextAlignment,
                        VerticalTextAlignment = column.VerticalTextAlignment,
                        LineBreakMode = LineBreakMode.WordWrap,
                    };
                    label.SetBinding(Label.TextProperty, new Binding(column.PropertyName, BindingMode.Default, stringFormat: column.StringFormat));
                    //text.SetBinding(Label.FontSizeProperty, new Binding(DataGrid.FontSizeProperty.PropertyName, BindingMode.Default, source: DataGrid));
                    //text.SetBinding(Label.FontFamilyProperty, new Binding(DataGrid.FontFamilyProperty.PropertyName, BindingMode.Default, source: DataGrid));

                    cell = new ContentView
                    {
                        Padding = DataGrid.CellPadding,
                        Content = label,
                    };
                }
                SetColumn(cell, index);
                SetRow(cell, 0);
                Children.Add(cell);
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
            var tapControll = new TapGestureRecognizer { Command = DataGrid.CommandSelectedItem };
            tapControll.Tapped += TapControll_Tapped;
            GestureRecognizers.Add(tapControll);
        }

        private void TapControll_Tapped(object sender, EventArgs e)
        {
            var self = (StackCell)sender;

            var last = self.DataGrid.SelectedItem;
            if (last != null)
            {
                foreach (var item in last.Children)
                {
                    if (item is BoxView == false)
                        item.BackgroundColor = self.DataGrid.RowsColor;
                }
            }

            self.DataGrid.SelectedItem = this;
            foreach (var item in self.Children)
            {
                if (item is BoxView == false)
                    item.BackgroundColor = self.DataGrid.SelectedRowColor;
            }
        }

        private View CreateHorizontalLine()
        {
            var line = new BoxView
            {
                BackgroundColor = DataGrid.LinesColor,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            return line;
        }
    }
}
