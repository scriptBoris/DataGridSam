using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal class BorderWrapperOld
    {
        private DataGrid DataGrid;

        internal BoxView headLeft;
        internal BoxView headTop;
        internal BoxView headRight;
        internal BoxView absoluteBottom;
        //internal BoxView bottom;
        //internal BoxView leftScroll;
        //internal BoxView rightScroll;

        public BorderWrapperOld(DataGrid parent)
        {
            DataGrid = parent;
            headLeft = new BoxView();
            headLeft.HorizontalOptions = LayoutOptions.StartAndExpand;
            headLeft.VerticalOptions = LayoutOptions.FillAndExpand;
            headLeft.SetBinding(BoxView.WidthRequestProperty, new Binding(nameof(DataGrid.BorderWidth), source: DataGrid));
            headLeft.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(DataGrid.BorderColor), source: DataGrid));

            headTop = new BoxView();
            headTop.HorizontalOptions = LayoutOptions.FillAndExpand;
            headTop.VerticalOptions = LayoutOptions.StartAndExpand;
            headTop.SetBinding(BoxView.HeightRequestProperty, new Binding(nameof(DataGrid.BorderWidth), source: DataGrid));
            headTop.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(DataGrid.BorderColor), source: DataGrid));

            headRight = new BoxView();
            headRight.HorizontalOptions = LayoutOptions.EndAndExpand;
            headRight.VerticalOptions = LayoutOptions.FillAndExpand;
            headRight.SetBinding(BoxView.WidthRequestProperty, new Binding(nameof(DataGrid.BorderWidth), source: DataGrid));
            headRight.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(DataGrid.BorderColor), source: DataGrid));

            absoluteBottom = new BoxView();
            absoluteBottom.IsVisible = false;
            absoluteBottom.HorizontalOptions = LayoutOptions.FillAndExpand;
            absoluteBottom.VerticalOptions = LayoutOptions.EndAndExpand;
            absoluteBottom.SetBinding(BoxView.HeightRequestProperty, new Binding(nameof(DataGrid.BorderWidth), source: DataGrid));
            absoluteBottom.TranslationY = DataGrid.BorderWidth * 0.1;
            absoluteBottom.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(DataGrid.BorderColor), source: DataGrid));
            Grid.SetRow(absoluteBottom, 1);

            // Bottom line
            //bottom = new BoxView();
            //bottom.VerticalOptions = LayoutOptions.EndAndExpand;
            //bottom.HorizontalOptions = LayoutOptions.FillAndExpand;
            //bottom.SetBinding(BoxView.HeightRequestProperty, new Binding(nameof(DataGrid.BorderWidth), source: DataGrid));
            //bottom.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(DataGrid.BorderColor), source: DataGrid));

            // Wrapp borders
            //leftScroll = new BoxView();
            //leftScroll.VerticalOptions = LayoutOptions.FillAndExpand;
            //leftScroll.HorizontalOptions = LayoutOptions.StartAndExpand;
            //leftScroll.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(DataGrid.BorderColor), source: DataGrid));
            //leftScroll.SetBinding(BoxView.WidthRequestProperty, new Binding(nameof(DataGrid.BorderWidth), source: DataGrid));

            //rightScroll = new BoxView();
            //rightScroll.VerticalOptions = LayoutOptions.FillAndExpand;
            //rightScroll.HorizontalOptions = LayoutOptions.EndAndExpand;
            //rightScroll.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(DataGrid.BorderColor), source: DataGrid));
            //rightScroll.SetBinding(BoxView.WidthRequestProperty, new Binding(nameof(DataGrid.BorderWidth), source: DataGrid));
            //rightScroll.TranslationX = DataGrid.BorderWidth;
            //Grid.SetRow(rightScroll, 1);
        }

        internal void UpdateWrapper()
        {
            if (DataGrid.IsWrapped)
            {
                //double margin = DataGrid.BorderWidth;
                //DataGrid.stackList.Margin = new Thickness(margin, 0, margin, margin);

                // Header
                DataGrid.Children.Add(headLeft);
                DataGrid.Children.Add(headTop);
                DataGrid.Children.Add(headRight);
                DataGrid.Children.Add(absoluteBottom);

                // Body
                //DataGrid.maskBodyGrid.Children.Add(leftScroll);
                //DataGrid.maskBodyGrid.Children.Add(rightScroll);
                //DataGrid.maskBodyGrid.Children.Add(bottom);
                //Grid.SetColumnSpan(bottom, DataGrid.ColumnSpan);

                DataGrid.mainScroll.SizeChanged += DataGrid.CheckWrapperBottomVisible;
                DataGrid.stackList.SizeChanged += DataGrid.CheckWrapperBottomVisible;


                //int set = DataGrid.Columns?.Count - 1 ?? 0;
                //if (set < 0)
                //    set = 0;

                //Grid.SetColumn(rightScroll, set);
            }
            else
            {
                //DataGrid.stackList.Margin = 0;

                // Header
                DataGrid.Children.Remove(headLeft);
                DataGrid.Children.Remove(headTop);
                DataGrid.Children.Remove(headRight);
                DataGrid.Children.Remove(absoluteBottom);
                //host.bodyGrid.Children.Remove(bottom);

                // Body
                //DataGrid.maskBodyGrid.Children.Remove(bottom);
                //DataGrid.maskBodyGrid.Children.Remove(leftScroll);
                //DataGrid.maskBodyGrid.Children.Remove(rightScroll);
                DataGrid.mainScroll.SizeChanged -= DataGrid.CheckWrapperBottomVisible;
                DataGrid.stackList.SizeChanged -= DataGrid.CheckWrapperBottomVisible;
            }
        }

        //internal void UpdateBodyBottomLine()
        //{
        //    if (DataGrid.IsWrapped && (DataGrid.stackList.ItemsCount > 0 || DataGrid.ViewForEmpty != null))
        //    {

        //        DataGrid.wrapper.bottom.IsVisible = true;
        //    }
        //    else
        //    {
        //        DataGrid.wrapper.bottom.IsVisible = false;
        //    }
        //}
    }
}