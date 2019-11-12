using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace DataGridSam
{
    public partial class DataGrid
    {
        private void InitHeaderView()
        {
            SetColumnsBindingContext();
            _headerView.Children.Clear();
            _headerView.ColumnDefinitions.Clear();
            _sortingOrders.Clear();

            _headerView.Padding = new Thickness(BorderThickness.Left, BorderThickness.Top, BorderThickness.Right, 0);
            _headerView.ColumnSpacing = BorderThickness.HorizontalThickness / 2;

            if (Columns != null)
            {
                foreach (var col in Columns)
                {
                    _headerView.ColumnDefinitions.Add(new ColumnDefinition { Width = col.Width });

                    var cell = GetHeaderViewForColumn(col);

                    _headerView.Children.Add(cell);
                    Grid.SetColumn(cell, Columns.IndexOf(col));

                    _sortingOrders.Add(Columns.IndexOf(col), SortingOrder.None);
                }
            }
        }

        private void SetColumnsBindingContext()
        {
            if (Columns != null)
                foreach (var c in Columns)
                    c.BindingContext = BindingContext;
        }
    }
}
