using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    internal class BorderWrapper
    {
        private DataGrid host;

        internal BoxView left;
        internal BoxView top;
        internal BoxView right;
        internal BoxView bottom;
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

            bottom = new BoxView();
            bottom.IsVisible = false;
            bottom.HorizontalOptions = LayoutOptions.FillAndExpand;
            bottom.VerticalOptions = LayoutOptions.EndAndExpand;
            bottom.SetBinding(BoxView.HeightRequestProperty, new Binding(nameof(host.BorderWidth), source: host));
            bottom.SetBinding(BoxView.BackgroundColorProperty, new Binding(nameof(host.BorderColor), source: host));

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
            if (host.Columns?.Count == 0)
                Grid.SetColumn(rightScroll, 0);
            else
                Grid.SetColumn(rightScroll, host.Columns.Count);

        }
    }
}
