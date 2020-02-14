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
            
            // Set vertical thickness
            maskGrid.ColumnSpacing = 0;
            headGrid.ColumnSpacing = 0;

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
                    Grid.SetColumn(headCell, i);
                    headGrid.Children.Add(headCell);


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

        private void UpdateWrapper()
        {
            if (IsWrapped)
            {
                Children.Add(wrapper.left);
                Children.Add(wrapper.top);
                Children.Add(wrapper.right);
                Children.Add(wrapper.bottom);
                maskGrid.Children.Add(wrapper.leftScroll);
                maskGrid.Children.Add(wrapper.rightScroll);

                stackList.SizeChanged += CheckWrapperBottomVisible;
                mainScroll.SizeChanged += CheckWrapperBottomVisible;
            }
            else
            {
                Children.Remove(wrapper.left);
                Children.Remove(wrapper.top);
                Children.Remove(wrapper.right);
                Children.Remove(wrapper.bottom);
                maskGrid.Children.Remove(wrapper.leftScroll);
                maskGrid.Children.Remove(wrapper.rightScroll);
                stackList.SizeChanged -= CheckWrapperBottomVisible;
                mainScroll.SizeChanged -= CheckWrapperBottomVisible;
            }
        }

        private void CheckWrapperBottomVisible(object obj, EventArgs e)
        {
            if (mainScroll.Height > stackList.Height)
            {
                wrapper.bottom.IsVisible = false;
            }
            else
            {
                wrapper.bottom.IsVisible = true;
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

        private void BindLongTapCommand(ICommand command)
        {
            foreach (var item in stackList.Children)
            {
                var row = item as Row;
                row.BindLongTapCommand(command);
            }
        }
    }
}
