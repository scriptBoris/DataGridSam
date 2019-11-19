using DataGridSam.Utils;
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
            
            // Set vertical thickness
            verticalLines.ColumnSpacing = BorderWidth;
            _headerView.ColumnSpacing = BorderWidth;

            _headerView.Children.Clear();
            _headerView.ColumnDefinitions.Clear();

            verticalLines.Children.Clear();
            verticalLines.ColumnDefinitions.Clear();

            // Imitation header lines and color
            _headerView.BackgroundColor = BorderColor;

            if (Columns != null)
            {
                int i = 0;
                foreach (var col in Columns)
                {
                    // Header table
                    _headerView.ColumnDefinitions.Add(new ColumnDefinition { Width = col.Width });
                    var cell = CreateColumnHeader(col);
                    _headerView.Children.Add(cell);
                    Grid.SetColumn(cell, i);

                    // Create vertical lines (Table)
                    verticalLines.ColumnDefinitions.Add(new ColumnDefinition { Width = col.Width });

                    if (i < Columns.Count - 1)
                    {
                        var line = CreateColumnLine();
                        verticalLines.Children.Add(line);
                        Grid.SetColumn(line, i);
                        Grid.SetRow(line, 0);
                    }

                    i++;
                }
            }
        }

        /// <summary>
        /// Create header label over column
        /// </summary>
        /// <param name="column">Source label column</param>
        private View CreateColumnHeader(DataGridColumn column)
        {
            // Set header text color & font size
            column.HeaderLabel.TextColor = HeaderTextColor;
            column.HeaderLabel.FontSize = HeaderFontSize;

            // Detect styles (if has - override latest parameters)
			column.HeaderLabel.Style = column.HeaderLabelStyle ?? this.HeaderLabelStyle ?? (Style)_headerView.Resources["HeaderDefaultStyle"];

            // Drop in wrap container
            var container = new StackLayout();
            container.BackgroundColor = HeaderBackgroundColor;
            container.Children.Add(column.HeaderLabel);

            return container;
        }

        /// <summary>
        /// Create vertical linse aka Column
        /// </summary>
        private View CreateColumnLine()
        {
            var line = new BoxView
            {
                WidthRequest = BorderWidth + 0.1,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = BorderColor,
                TranslationX = BorderWidth + 0.2,
            };
            return line;
        }

        private void SetColumnsBindingContext()
        {
            if (Columns != null)
                foreach (var c in Columns)
                    c.BindingContext = BindingContext;
        }

        internal void ShowPaginationBackButton(bool isVisible)
        {
            buttonLatest.IsVisible = isVisible;
        }
        internal void ShowPaginationNextButton(bool isVisible)
        {
            buttonNext.IsVisible = isVisible;
        }

        private void OnButtonLatestClicked(object sender, EventArgs e)
        {
            stackList.RedrawForPage(PaginationItemCount, selectPage: PaginationCurrentPage-1);
            scrollTable.ScrollToAsync(0, stackList.Height, false);
        }

        private void OnButtonNextClicked(object sender, EventArgs e)
        {
            stackList.RedrawForPage(PaginationItemCount, selectPage: PaginationCurrentPage+1);
            scrollTable.ScrollToAsync(0, 0, false);
        }
    }
}
