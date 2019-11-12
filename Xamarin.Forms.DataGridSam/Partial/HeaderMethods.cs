using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam
{
    public partial class DataGrid
    {
        private void InitHeaderView()
        {
            SetColumnsBindingContext();
            _headerView.Children.Clear();
            _headerView.ColumnDefinitions.Clear();

            verticalLines.Children.Clear();
            verticalLines.ColumnDefinitions.Clear();

            //_headerView.ColumnSpacing = 0;

            if (Columns != null)
            {
                int i = 0;
                foreach (var col in Columns)
                {
                    // Header table
                    _headerView.ColumnDefinitions.Add(new ColumnDefinition { Width = col.Width });
                    var cell = GetHeaderViewForColumn(col);
                    _headerView.Children.Add(cell);
                    Grid.SetColumn(cell, i);

                    // vertical lines (Table)
                    verticalLines.ColumnDefinitions.Add(new ColumnDefinition { Width = col.Width });

                    if (i < Columns.Count - 1)
                    {
                        var line = CreateLine();
                        verticalLines.Children.Add(line);
                        Grid.SetColumn(line, i);
                    }

                    i++;
                }
            }
        }

        private View GetHeaderViewForColumn(DataGridColumn column)
        {
			column.HeaderLabel.Style = column.HeaderLabelStyle ?? this.HeaderLabelStyle ?? (Style)_headerView.Resources["HeaderDefaultStyle"];

            var container = new StackLayout();
            container.Children.Add(column.HeaderLabel);

            return container;
        }

        private View CreateLine()
        {
            var line = new BoxView
            {
                WidthRequest = LinesWidth,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.Blue,
                TranslationX = 5,
            };
            return line;
        }

        private void SetColumnsBindingContext()
        {
            if (Columns != null)
                foreach (var c in Columns)
                    c.BindingContext = BindingContext;
        }
    }
}
