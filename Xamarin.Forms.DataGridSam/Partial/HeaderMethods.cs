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

            //_headerView.ColumnSpacing = 0;

            if (Columns != null)
            {
                foreach (var col in Columns)
                {
                    _headerView.ColumnDefinitions.Add(new ColumnDefinition { Width = col.Width });

                    var cell = GetHeaderViewForColumn(col);

                    _headerView.Children.Add(cell);
                    Grid.SetColumn(cell, Columns.IndexOf(col));
                }
            }
        }

        private View GetHeaderViewForColumn(DataGridColumn column)
        {
            column.HeaderLabel.Style = (Style)_headerView.Resources["HeaderDefaultStyle"];

            var container = new StackLayout();

            container.Children.Add(column.HeaderLabel);

            return container;
        }

        private void SetColumnsBindingContext()
        {
            if (Columns != null)
                foreach (var c in Columns)
                    c.BindingContext = BindingContext;
        }
    }
}
