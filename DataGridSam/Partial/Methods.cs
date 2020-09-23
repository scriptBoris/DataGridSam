using DataGridSam.Elements;
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
        private void Init()
        {
            InitHeaderView();
            UpdateRowTriggers();
        }

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
                maskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = col.CalcWidth });

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
            bool isUp = false;
            bool isDown = false;

            // Clear old GUI elements and values
            AutoNumberStrategy = Enums.AutoNumberStrategyType.None;
            headGrid.Children.Clear();
            headGrid.ColumnDefinitions.Clear();
            UpdateHeadHeight(HeaderHeight);

            if (Columns == null)
                return;

            int i = 0;
            foreach (var col in Columns)
            {
                col.OnAttached(this, i);

                // Header table
                headGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = col.CalcWidth });

                var headCell = CreateColumnHeader(col);
                headCell.IsVisible = col.IsVisible;

                Grid.SetColumn(headCell, i);
                headGrid.Children.Add(headCell);

                // Detect auto number
                if (col.AutoNumber == Enums.AutoNumberType.Up)
                    isUp = true;
                else if (col.AutoNumber == Enums.AutoNumberType.Down)
                    isDown = true;

                i++;
            }

            if (isUp)
                AutoNumberStrategy = Enums.AutoNumberStrategyType.Up;
            else if (isDown)
                AutoNumberStrategy = Enums.AutoNumberStrategyType.Down;
            else if (isUp && isDown)
                AutoNumberStrategy = Enums.AutoNumberStrategyType.Both;
        }

        internal void UpdateHeadHeight(int height)
        {
            var row = RowDefinitions.First();

            if (height < 0)
                row.Height = GridLength.Auto;
            else
                row.Height = new GridLength(height);
        }

        internal void UpdateColumnVisibile(DataGridColumn col, bool isVisible)
        {
            void SolveWidth(ColumnDefinition target, bool flag)
            {
                if (target == null)
                    return;

                if (flag)
                    target.Width = col.Width;
                else
                    target.Width = new GridLength(0.0);
            }

            int i = col.Index;
            col.HeaderWrapper.IsVisible = isVisible;

            SolveWidth(headGrid?.ColumnDefinitions[i], isVisible);
            SolveWidth(maskGrid?.ColumnDefinitions[i], isVisible);
            SolveWidth(maskHeadGrid?.ColumnDefinitions[i], isVisible);
                
            foreach (var item in stackList.Children)
            {
                var row = item as GridRow;
                row?.UpdateCellVisibility(i, isVisible);
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
                maskHeadGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
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
                maskHeadGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = col.CalcWidth });

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
            };
            Grid.SetColumnSpan(row, i);
            Grid.SetRow(row, 1);
            maskHeadGrid.Children.Add(row);
        }

        private void UpdateRowTriggers()
        {
            int i = 0;
            foreach (var item in RowTriggers)
            {
                item.Priority = i++;
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
            column.HeaderWrapper.Content = column.HeaderLabel;

            return column.HeaderWrapper;
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

        /// <summary>
        /// Called every time an ItemsSource changes
        /// Вызывается каждый раз, когда меняется ItemsSource;
        /// </summary>
        internal void OnChangeItemsBindingContext(Type newTypeItems)
        {
            foreach (var trigger in RowTriggers)
            {
                trigger.OnSourceTypeChanged(newTypeItems);
            }
        }

        private static void UpdateSelectedItem(BindableObject b, object o, object n)
        {
            var self = (DataGrid)b;

            var lastRow = self.SelectedRow;

            if (n == null && lastRow != null)
            {
                lastRow.IsSelected = false;
                lastRow.UpdateStyle();

                self.SelectedRow = null;
            }
            else if (self.stackList.ItemsSource is IList list)
            {
                var match = list.IndexOf(n);

                if (match >= 0 && self.stackList.Children.Count > 0)
                {
                    var row = self.stackList.Children[match] as GridRow;
                    row.IsSelected = true;
                    row.UpdateStyle();

                    self.SelectedRow = row;
                }
            }
        }
    }
}
