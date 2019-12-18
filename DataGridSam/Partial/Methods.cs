using DataGridSam.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
            maskGrid.ColumnSpacing = BorderWidth;
            headGrid.ColumnSpacing = BorderWidth;

            // Clear GUI header & mask
            headGrid.Children.Clear();
            headGrid.ColumnDefinitions.Clear();
            maskGrid.Children.Clear();
            maskGrid.ColumnDefinitions.Clear();

            if (Columns != null)
            {
                int i = 0;
                foreach (var col in Columns)
                {
                    // Header table
                    headGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = col.Width });
                    var headCell = CreateColumnHeader(col);
                    headGrid.Children.Add(headCell);
                    Grid.SetColumn(headCell, i);


                    // Create vertical lines (Table)
                    maskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = col.Width });

                    if (i < Columns.Count - 1)
                    {
                        var line = CreateColumnLine();
                        maskGrid.Children.Add(line);
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

            // TODO Add column header style
            // Detect styles (if has - override latest parameters)
			column.HeaderLabel.Style = column.HeaderLabelStyle ?? this.HeaderLabelStyle ?? HeaderDefaultStyle;

            // Drop in wrap container
            var container = new StackLayout();
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
                WidthRequest = BorderWidth * 1.05,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = BorderColor,
                TranslationX = BorderWidth * 1.10,
            };
            return line;
        }

        private void SetColumnsBindingContext()
        {
            if (Columns != null)
                foreach (var c in Columns)
                    c.BindingContext = BindingContext;
        }

        private void UpdateHeaderStyle(Style style)
        {
            if (headGrid == null)
                return;

            foreach (var col in headGrid.Children)
            {
                if (col is StackLayout stackLayout && stackLayout.Children.First() is Label label)
                {
                    label.Style = style ?? HeaderDefaultStyle;
                }
            }
        }

        internal void ShowPaginationBackButton(bool isVisible)
        {
            if (buttonLatest != null)
                buttonLatest.IsVisible = isVisible;
        }
        internal void ShowPaginationNextButton(bool isVisible)
        {
            if (buttonLatest != null)
                buttonNext.IsVisible = isVisible;
        }

        private void OnButtonLatestClicked(object sender, EventArgs e)
        {
            stackList.RedrawForPage(PaginationItemCount, selectPage: PaginationCurrentPage-1);
            scroll.ScrollToAsync(0, stackList.Height, false);
        }

        private void OnButtonNextClicked(object sender, EventArgs e)
        {
            stackList.RedrawForPage(PaginationItemCount, selectPage: PaginationCurrentPage+1);
            scroll.ScrollToAsync(0, 0, false);
        }
    }
}
