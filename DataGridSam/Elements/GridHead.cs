using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal class GridHead : Layout<View>
    {
        private DataGrid dataGrid;
        private List<DataGridColumn> cells => dataGrid.InternalColumns;
        private List<View> allLines = new List<View>();
        private List<View> colLines = new List<View>();

        private View left = new BoxView();
        private View right = new BoxView();
        private View top = new BoxView();
        private View split = new BoxView();

        public GridHead(DataGrid host)
        {
            dataGrid = host;
            SetBinding(Grid.BackgroundColorProperty, new Binding(nameof(host.HeaderBackgroundColor), source: host));

            AddStaticLines();

            foreach (var line in allLines)
            {
                //line.BackgroundColor = Color.Red;
                line.SetBinding(Grid.BackgroundColorProperty, new Binding(nameof(host.BorderColor), source: host));
                Children.Add(line);
            }
        }

        private void AddStaticLines()
        {
            allLines.Add(left);
            allLines.Add(right);
            allLines.Add(top);
            allLines.Add(split);
        }

        private void CalculateActualColumnsWidth(double widthConstraint)
        {
            if (cells == null)
                return;

            int countVisibleCells = 0;
            foreach (var cell in cells)
                if (cell.IsVisible) countVisibleCells++;

            double fixWidth = widthConstraint;
            if (dataGrid.IsWrapped)
                fixWidth -= dataGrid.BorderWidth * (countVisibleCells + 1);
            else
                fixWidth -= dataGrid.BorderWidth * (countVisibleCells - 1);

            foreach (var cell in cells)
                cell.ActualWidth = GridRow.CalcWidth(fixWidth, cell, cells);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            double wrap = dataGrid.IsWrapped ? dataGrid.BorderWidth : 0;
            double bw = dataGrid.BorderWidth;

            if (cells?.Count == 0)
                return new SizeRequest(new Size(widthConstraint, 0));

            CalculateActualColumnsWidth(widthConstraint);


            double height = 0;
            foreach (var cell in cells)
            {
                if (!cell.IsVisible)
                    continue;

                // get height header cell
                var cellSize = cell.Cell.Measure(cell.ActualWidth, double.PositiveInfinity, MeasureFlags.IncludeMargins);

                if (height < cellSize.Request.Height)
                    height = cellSize.Request.Height;
            }

            // Top line
            height += wrap;

            // Split line
            height += bw;

            return new SizeRequest(new Size(widthConstraint, height));
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            CalculateActualColumnsWidth(width);

            double wrap = dataGrid.IsWrapped ? dataGrid.BorderWidth : 0;
            double bw = dataGrid.BorderWidth;

            double lastXCell = wrap;
            foreach (var col in cells)
            {
                // Cells
                if (col.IsVisible)
                {
                    var rect = new Rectangle(lastXCell, wrap, col.ActualWidth, height - wrap - bw);
                    LayoutChildIntoBoundingRegion(col.Cell, rect);
                    lastXCell += bw + col.ActualWidth;
                }
                else
                {
                    LayoutChildIntoBoundingRegion(col.Cell, Rectangle.Zero);
                }
            }

            if (dataGrid.IsWrapped)
            {
                var rt = new Rectangle(0, 0, width, bw);
                var rl = new Rectangle(0, 0, bw, height);
                var rr = new Rectangle(width-bw, 0, bw, height);
                LayoutChildIntoBoundingRegion(left, rl);
                LayoutChildIntoBoundingRegion(right, rr);
                LayoutChildIntoBoundingRegion(top, rt);
            }
            else
            {
                LayoutChildIntoBoundingRegion(left, Rectangle.Zero);
                LayoutChildIntoBoundingRegion(right, Rectangle.Zero);
                LayoutChildIntoBoundingRegion(top, Rectangle.Zero);
            }

            // Split line
            var rb = new Rectangle(0, height - bw, width, bw);
            LayoutChildIntoBoundingRegion(split, rb);

            // Column borders line
            if (dataGrid.HeaderHasBorder)
            {
                double lastXLine = 0;
                for (int i = 0; i < cells.Count - 1; i++)
                {
                    var col = cells[i];

                    if (col.IsVisible)
                    {
                        lastXLine += col.ActualWidth + (bw);
                        var rect = new Rectangle(lastXLine, 0, bw, height);
                        LayoutChildIntoBoundingRegion(colLines[i], rect);
                    }
                    else
                    {
                        LayoutChildIntoBoundingRegion(colLines[i], Rectangle.Zero);
                    }
                }
            }
            else
            {
                foreach (var col in colLines)
                    LayoutChildIntoBoundingRegion(col, Rectangle.Zero);
            }
        }

        internal void UpdateColumns()
        {
            // Clear old GUI elements and values
            dataGrid.AutoNumberStrategy = Enums.AutoNumberStrategyType.None;
            allLines.Clear();
            colLines.Clear();

            AddStaticLines();

            if (cells != null)
            {
                bool isUp = false;
                bool isDown = false;
                int id = 0;
                foreach (var col in cells)
                {
                    // Create vertical border
                    CreateVLine();

                    // Set label header cell
                    InitColumnCell(col);

                    //Detect auto number
                    if (col.AutoNumber == Enums.AutoNumberType.Up)
                        isUp = true;
                    else if (col.AutoNumber == Enums.AutoNumberType.Down)
                        isDown = true;

                    id++;
                }

                if (isUp)
                    dataGrid.AutoNumberStrategy = Enums.AutoNumberStrategyType.Up;
                else if (isDown)
                    dataGrid.AutoNumberStrategy = Enums.AutoNumberStrategyType.Down;
                else if (isUp && isDown)
                    dataGrid.AutoNumberStrategy = Enums.AutoNumberStrategyType.Both;
            }
        }

        internal void UpdateBorderColor()
        {
            foreach (var line in allLines)
                line.BackgroundColor = dataGrid.BorderColor;
        }

        internal void Redraw(bool isNeedMeasure = true)
        {
            if (isNeedMeasure)
                InvalidateMeasure();

            InvalidateLayout();
        }

        private View CreateVLine()
        {
            var res = new BoxView();
            res.InputTransparent = true;
            res.BackgroundColor = dataGrid.BorderColor;

            Children.Add(res);
            allLines.Add(res);
            colLines.Add(res);

            return res;
        }

        private void InitColumnCell(DataGridColumn column)
        {
            // Set header text color & font size
            column.Label.TextColor = dataGrid.HeaderTextColor;
            column.Label.FontSize = dataGrid.HeaderFontSize;
            column.Label.HorizontalTextAlignment = dataGrid.HeaderHorizontalTextAlignment;
            column.Label.VerticalTextAlignment = dataGrid.HeaderVerticalTextAlignment;
            column.Label.Padding = dataGrid.HeaderTextPadding;
            column.Label.Style = column.HeaderLabelStyle ?? dataGrid.HeaderLabelStyle ?? null;

            // Drop in wrap container
            column.Cell.Content = column.Label;
            Children.Add(column.Cell);
        }

        internal void UpdateTextAlign()
        {
            if (cells == null) return;

            foreach (var cell in cells)
            {
                cell.Label.HorizontalTextAlignment = dataGrid.HeaderHorizontalTextAlignment;
                cell.Label.VerticalTextAlignment = dataGrid.HeaderVerticalTextAlignment;
            }
        }

        internal void UpdateTextPadding()
        {
            if (cells == null) return;

            foreach (var cell in cells)
                cell.Label.Padding = dataGrid.HeaderTextPadding;
        }

        internal void UpdateFontSize()
        {
            if (cells == null) return;

            foreach (var cell in cells)
                cell.Label.FontSize = dataGrid.HeaderFontSize;
        }
    }
}
