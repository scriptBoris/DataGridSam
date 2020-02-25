using DataGridSam.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DataGridSam
{
    public partial class DataGrid
    {
        private void InitHeaderView()
        {
            SetColumnsBindingContext();

            UpdateHeaderCells();
            UpdateBodyMaskBorders();
            UpdateHeadMaskBorders();
            wrapper.Update();
        }

        private void UpdateBodyMaskBorders()
        {
            // Clear GUI mask
            maskGrid.Children.Clear();
            maskGrid.ColumnDefinitions.Clear();

            if (Columns == null)
                return;

            int i = 0;
            foreach (var col in Columns)
            {
                // Create vertical borders (Table)
                maskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = col.Width });

                if (i < Columns.Count - 1)
                {
                    var line = CreateColumnLine();
                    Grid.SetColumn(line, i);
                    Grid.SetRow(line, 0);
                    maskGrid.Children.Add(line);
                }

                i++;
            }
        }

        private void UpdateHeaderCells()
        {
            // Clear old GUI elements
            headGrid.Children.Clear();
            headGrid.ColumnDefinitions.Clear();

            if (Columns == null)
                return;

            int i = 0;
            foreach (var col in Columns)
            {
                // Header table
                headGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = col.Width });
                var headCell = CreateColumnHeader(col);
                Grid.SetColumn(headCell, i);
                headGrid.Children.Add(headCell);

                i++;
            }
        }

        private void UpdateHeadMaskBorders()
        {
            if (HeaderHasBorder && maskHeadGrid == null)
            {
                maskHeadGrid = new Grid();
                maskHeadGrid.ColumnSpacing = 0;
                maskHeadGrid.RowSpacing = 0;
                maskHeadGrid.BackgroundColor = Color.Transparent;
                maskHeadGrid.InputTransparent = true;
                maskHeadGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                SetRow(maskHeadGrid, 0);
                Children.Add(maskHeadGrid);
            }
            else if (!HeaderHasBorder)
            {
                maskHeadGrid.Children.Clear();
                Children.Remove(maskHeadGrid);
                maskHeadGrid = null;
                return;
            }

            if (Columns == null)
                return;

            // Columns
            int i = 0;
            foreach (var col in Columns)
            {
                // Create vertical borders (Table)
                maskHeadGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = col.Width });

                if (i < Columns.Count - 1)
                {
                    var line = CreateColumnLine();
                    Grid.SetColumn(line, i);
                    Grid.SetRow(line, 0);
                    maskHeadGrid.Children.Add(line);
                }

                i++;
            }

            // Row
            var row = new BoxView
            {
                HeightRequest = BorderWidth,
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = BorderColor,
                //TranslationY = BorderWidth,
            };
            Grid.SetColumnSpan(row, i);
            Grid.SetRow(row, 1);
            maskHeadGrid.Children.Add(row);
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
                WidthRequest = BorderWidth,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = BorderColor,
                TranslationX = BorderWidth,
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

        internal void CheckWrapperBottomVisible(object obj, EventArgs e)
        {
            if (mainScroll.Height > stackList.Height)
            {
                wrapper.absoluteBottom.IsVisible = false;
            }
            else
            {
                wrapper.absoluteBottom.IsVisible = true;
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
            mainScroll.ScrollToAsync(0, stackList.Height, false);
        }

        private void OnButtonNextClicked(object sender, EventArgs e)
        {
            stackList.RedrawForPage(PaginationItemCount, selectPage: PaginationCurrentPage+1);
            mainScroll.ScrollToAsync(0, 0, false);
        }

        private void BindTapCommand(ICommand command)
        {
            foreach (var item in stackList.Children)
            {
                var row = item as Row;
                DataGridSam.Platform.Touch.SetTap(row, command);
            }
        }

        private void BindLongTapCommand(ICommand command)
        {
            foreach (var item in stackList.Children)
            {
                var row = item as Row;
                DataGridSam.Platform.Touch.SetLongTap(row, command);
            }
        }
    }
}
