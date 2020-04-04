using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal class BorderWrapper
    {
        private DataGrid host;

        internal BoxView left;
        internal BoxView top;
        internal BoxView right;
        internal BoxView bottom;
        internal BoxView absoluteBottom;
        internal BoxView leftScroll;
        internal BoxView rightScroll;

        public BorderWrapper(DataGrid parent)
        {
            host = parent;
            left = new BoxView();
            left.HorizontalOptions = LayoutOptions.StartAndExpand;
            left.VerticalOptions = LayoutOptions.FillAndExpand;
            left.SetBinding(BoxView.WidthRequestProperty, new Binding(nameof(host.BorderWidth), source: host));
            left.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(host.BorderColor), source: host));

            top = new BoxView();
            top.HorizontalOptions = LayoutOptions.FillAndExpand;
            top.VerticalOptions = LayoutOptions.StartAndExpand;
            top.SetBinding(BoxView.HeightRequestProperty, new Binding(nameof(host.BorderWidth), source: host));
            top.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(host.BorderColor), source: host));

            right = new BoxView();
            right.HorizontalOptions = LayoutOptions.EndAndExpand;
            right.VerticalOptions = LayoutOptions.FillAndExpand;
            right.SetBinding(BoxView.WidthRequestProperty, new Binding(nameof(host.BorderWidth), source: host));
            right.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(host.BorderColor), source: host));

            absoluteBottom = new BoxView();
            absoluteBottom.IsVisible = false;
            absoluteBottom.HorizontalOptions = LayoutOptions.FillAndExpand;
            absoluteBottom.VerticalOptions = LayoutOptions.EndAndExpand;
            absoluteBottom.SetBinding(BoxView.HeightRequestProperty, new Binding(nameof(host.BorderWidth), source: host));
            absoluteBottom.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(host.BorderColor), source: host));
            Grid.SetRow(absoluteBottom, 1);

            // Bottom line
            bottom = new BoxView();
            bottom.VerticalOptions = LayoutOptions.End;
            bottom.HorizontalOptions = LayoutOptions.FillAndExpand;
            bottom.SetBinding(BoxView.HeightRequestProperty, new Binding(nameof(host.BorderWidth), source: host));
            bottom.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(host.BorderColor), source: host));
            Grid.SetRow(bottom, 1);

            // Wrapp borders
            leftScroll = new BoxView();
            leftScroll.VerticalOptions = LayoutOptions.FillAndExpand;
            leftScroll.HorizontalOptions = LayoutOptions.StartAndExpand;
            leftScroll.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(host.BorderColor), source: host));
            leftScroll.SetBinding(BoxView.WidthRequestProperty, new Binding(nameof(host.BorderWidth), source: host));

            rightScroll = new BoxView();
            rightScroll.VerticalOptions = LayoutOptions.FillAndExpand;
            rightScroll.HorizontalOptions = LayoutOptions.EndAndExpand;
            rightScroll.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(host.BorderColor), source: host));
            rightScroll.SetBinding(BoxView.WidthRequestProperty, new Binding(nameof(host.BorderWidth), source: host));
        }

        internal void Update()
        {
            if (host.IsWrapped)
            {
                // Header
                host.Children.Add(left);
                host.Children.Add(top);
                host.Children.Add(right);
                host.Children.Add(absoluteBottom);
                host.maskGrid.Children.Add(leftScroll);
                host.maskGrid.Children.Add(rightScroll);
                host.bodyGrid.Children.Add(bottom);
                host.stackList.SizeChanged += host.CheckWrapperBottomVisible;
                host.mainScroll.SizeChanged += host.CheckWrapperBottomVisible;

                Grid.SetColumn(rightScroll, host.Columns?.Count-1 ?? 0);
            }
            else
            {
                host.Children.Remove(left);
                host.Children.Remove(top);
                host.Children.Remove(right);
                host.Children.Remove(absoluteBottom);
                host.bodyGrid.Children.Remove(bottom);
                host.maskGrid.Children.Remove(leftScroll);
                host.maskGrid.Children.Remove(rightScroll);
                host.stackList.SizeChanged -= host.CheckWrapperBottomVisible;
                host.mainScroll.SizeChanged -= host.CheckWrapperBottomVisible;
            }
        }
    }
}
