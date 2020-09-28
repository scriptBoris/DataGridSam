using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DataGridSam
{
    internal class LayoutInformation
    {
        public Rectangle Bounds;
        public double CompressionSpace;
        public Size Constraint;
        public int Expanders;
        public Size MinimumSize;
        public Rectangle[] Plots;
        public SizeRequest[] Requests;
    }

    public partial class DataGrid
    {
        /// <summary>
        /// Childs where registry in stack render display
        /// </summary>
        internal List<View> ChildrenStack = new List<View>();
        private List<double> childsHeight = new List<double>();

        private double totalWidth = -1;
        private double totalHeight = -1;

        private double headHeight = -1;
        private double scrollHeight = -1;

        LayoutInformation _layoutInformation = new LayoutInformation();

        private void ChildAdd(View view)
        {
            ChildrenStack.Add(view);
            childsHeight.Add(-1);
            Children.Add(view);
        }

        private void ChildReset()
        {
            Children.Clear();
            childsHeight.Clear();
            ChildrenStack.Clear();
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (!HasVisibleChildren())
                return;

            double offset = IsWrapped ? BorderWidth : 0;
            width -= offset;

            //double calcHead = headHeight;
            //double calcScroll = scrollHeight - offset;
            //double calcScroll = height - (headHeight + offset * 2);


            // Header
            double hY = offset;
            double hH = childsHeight[0];

            // Content rows && scroll
            double sY = hH + offset;
            double sH = height - hH;

            double total = 0;
            total += offset;
            total += hH;
            total += sH;

            LayoutChildIntoBoundingRegion(mainScroll, new Rectangle(offset, sY, width, sH));
            LayoutChildIntoBoundingRegion(headRow, new Rectangle(offset, hY, width, hH));
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            double h = 0;
            int i = 0;
            foreach (var item in ChildrenStack)
            {
                var r = item.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
                h += r.Request.Height;
                childsHeight[i] = r.Request.Height;

                i++;
            }

            return new SizeRequest(new Size(widthConstraint, h));

            //if (double.IsInfinity(heightConstraint))
            //{
            //    double h = 0;
            //    foreach (var item in ChildrenStack)
            //    {
            //        var r = item.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
            //        h += r.Request.Height;
            //    }
             
            //    return new SizeRequest(new Size(widthConstraint, 0));
            //}
            //else
            //    return new SizeRequest(new Size(widthConstraint, heightConstraint));

            //Thickness padding = Padding;

            //CalculateLayout(_layoutInformation, padding.Left, padding.Top, widthConstraint, heightConstraint);
            //var result = new SizeRequest(_layoutInformation.Bounds.Size, _layoutInformation.MinimumSize);
            //return result;
        }

        void CalculateLayout(LayoutInformation layout, double x, double y, double widthConstraint, double heightConstraint)
        {
            layout.Constraint = new Size(widthConstraint, heightConstraint);
            layout.Expanders = 0;
            layout.CompressionSpace = 0;
            layout.Plots = new Rectangle[ChildrenStack.Count];
            layout.Requests = new SizeRequest[ChildrenStack.Count];


            CalculateNaiveLayout(layout, x, y, widthConstraint, heightConstraint);
            CompressNaiveLayout(layout, widthConstraint, heightConstraint);

            //if (processExpanders)
            //{
            //    AlignOffAxis(layout, orientation, widthConstraint, heightConstraint);
            //    ProcessExpanders(layout, orientation, x, y, widthConstraint, heightConstraint);
            //}
        }

        void CalculateNaiveLayout(LayoutInformation layout, double x, double y, double widthConstraint, double heightConstraint)
        {
            layout.CompressionSpace = 0;

            double xOffset = x;
            double yOffset = y;
            double boundsWidth = 0;
            double boundsHeight = 0;
            double minimumWidth = 0;
            double minimumHeight = 0;
            //double spacing = Spacing;
             
            //View expander = null;

            for (var i = 0; i < ChildrenStack.Count; i++)
            {
                var child = ChildrenStack[i];
                if (!child.IsVisible)
                    continue;

                //if (child.VerticalOptions.Expands)
                //{
                //    layout.Expanders++;
                //    if (expander != null)
                //    {
                //        // we have multiple expanders, make sure previous expanders are reset to not be fixed because they no logner are
                //        //ComputeConstraintForView(child, false);
                //    }
                //    expander = child;
                //}
                SizeRequest request = child.Measure(widthConstraint, double.PositiveInfinity, MeasureFlags.IncludeMargins);

                childsHeight[i] = request.Request.Height;

                var bounds = new Rectangle(x, yOffset, request.Request.Width, request.Request.Height);
                layout.Plots[i] = bounds;
                layout.Requests[i] = request;
                layout.CompressionSpace += Math.Max(0, request.Request.Height - request.Minimum.Height);
                //yOffset = bounds.Bottom + spacing;

                boundsWidth = Math.Max(boundsWidth, request.Request.Width);
                boundsHeight = bounds.Bottom - y;
                //minimumHeight += request.Minimum.Height + spacing;
                minimumWidth = Math.Max(minimumWidth, request.Minimum.Width);
            }
            //minimumHeight -= spacing;
            //if (expander != null)
                //ComputeConstraintForView(expander, layout.Expanders == 1); // warning : slightly obtuse, but we either need to setup the expander or clear the last one

            layout.Bounds = new Rectangle(x, y, boundsWidth, boundsHeight);
            layout.MinimumSize = new Size(minimumWidth, minimumHeight);
        }

        void CompressNaiveLayout(LayoutInformation layout, double widthConstraint, double heightConstraint)
        {
            if (layout.CompressionSpace <= 0)
                return;

            CompressVerticalLayout(layout, widthConstraint, heightConstraint);
        }

        void CompressVerticalLayout(LayoutInformation layout, double widthConstraint, double heightConstraint)
        {
            double yOffset = 0;

            if (heightConstraint >= layout.Bounds.Height)
            {
                // no need to compress
                return;
            }

            double requiredCompression = layout.Bounds.Height - heightConstraint;
            double compressionSpace = layout.CompressionSpace;
            double compressionPressure = (requiredCompression / layout.CompressionSpace).Clamp(0, 1);

            for (var i = 0; i < layout.Plots.Length; i++)
            {
                var child = ChildrenStack[i];
                if (!child.IsVisible)
                    continue;

                Size minimum = layout.Requests[i].Minimum;

                layout.Plots[i].Y -= yOffset;

                Rectangle plot = layout.Plots[i];
                double availableSpace = plot.Height - minimum.Height;
                if (availableSpace <= 0)
                    continue;

                compressionSpace -= availableSpace;

                double compression = availableSpace * compressionPressure;
                yOffset += compression;

                double newHeight = plot.Height - compression;
                SizeRequest newRequest = child.Measure(widthConstraint, newHeight, MeasureFlags.IncludeMargins);

                layout.Requests[i] = newRequest;

                plot.Width = newRequest.Request.Width;

                if (newRequest.Request.Height < newHeight)
                {
                    double delta = newHeight - newRequest.Request.Height;
                    newHeight = newRequest.Request.Height;
                    yOffset += delta;
                    requiredCompression = requiredCompression - yOffset;
                    compressionPressure = (requiredCompression / compressionSpace).Clamp(0, 1);
                }
                plot.Height = newHeight;

                layout.Bounds.Width = Math.Max(layout.Bounds.Width, plot.Width);

                layout.Plots[i] = plot;
            }
        }

        private bool HasVisibleChildren()
        {
            if (Columns.Count == 0)
                return false;

            int visibleColumns = Columns.Count;

            foreach (var col in Columns)
            {
                if (!col.IsVisible)
                    visibleColumns--;
            }

            if (visibleColumns == 0)
                return false;

            return true;
        }

        //protected override SizeRequest OnMeasure(double w, double h)
        //{
        //    if (WidthRequest > 0)
        //        w = Math.Min(w, WidthRequest);
        //    if (HeightRequest > 0)
        //        h = Math.Min(h, HeightRequest);

        //    //if (totalWidth == w && totalHeight == h || Columns.Count == 0)
        //    //    return new SizeRequest(new Size(w, h));


        //    if (double.IsPositiveInfinity(w) && double.IsPositiveInfinity(h))
        //    {
        //        return new SizeRequest(Size.Zero, Size.Zero);
        //    }

        //    double height = 0;
        //    double minimumHeight = 0;

        //    var headRes = headRow.Measure(w, h, MeasureFlags.IncludeMargins);
        //    height += headRes.Request.Height;
        //    minimumHeight += headRes.Minimum.Height;
        //    headHeight = headRes.Request.Height;

        //    if (Columns.Count == 0)
        //        return new SizeRequest(new Size(w, headHeight));

        //    var scrollRes = mainScroll.Measure(w, h, MeasureFlags.IncludeMargins);
        //    height += scrollRes.Request.Height;
        //    minimumHeight += scrollRes.Minimum.Height;
        //    scrollHeight = scrollRes.Request.Height;


        //    totalHeight = height;
        //    totalWidth = w;
        //    return new SizeRequest
        //    {
        //        Request = new Size(w, height),
        //        Minimum = new Size(w, minimumHeight),
        //    };
        //}

        //protected override void LayoutChildren(double x, double y, double width, double height)
        //{
        //    double offset = IsWrapped ? BorderWidth : 0;
        //    width = width - offset * 2;

        //    //double calcHead = headHeight;
        //    double calcScroll = scrollHeight - offset;            
        //    //double calcScroll = height - (headHeight + offset * 2);


        //    LayoutChildIntoBoundingRegion(headRow, new Rectangle(offset, offset, width, headHeight));
        //    //LayoutChildIntoBoundingRegion(mainScroll, new Rectangle(offset, headHeight, width, calcScroll));
        //    LayoutChildIntoBoundingRegion(mainScroll, new Rectangle(offset, headHeight, width, calcScroll));
        //}
    }
}
