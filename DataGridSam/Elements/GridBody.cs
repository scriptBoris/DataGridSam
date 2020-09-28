using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    //internal class GridBody : StackLayout
    //{
    //    internal readonly DataGrid DataGrid;
    //    internal readonly StackList StackList;

    //    //internal View OtherContent;

    //    public GridBody(DataGrid host)
    //    {
    //        StackList = new StackList(host);

    //        DataGrid = host;
    //        DataGrid.stackList = StackList;

    //        Children.Add(StackList);
    //    }

    //    internal double lastWidth = 0;
    //    internal double lastHeight = 0;

    //    //protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
    //    //{
    //    //    return StackList.Measure(widthConstraint, heightConstraint);
    //    //    if (StackList.Children.Count == 0)
    //    //    {
    //    //        lastWidth = widthConstraint;
    //    //        lastHeight = 0;

    //    //        //DataGrid?.mainScroll.ContentSize = new Size(widthConstraint, 0);
    //    //        return new SizeRequest(new Size(widthConstraint, 0));
    //    //    }

    //    //    //if (lastHeight == heightConstraint && lastWidth == widthConstraint)
    //    //    //    return new SizeRequest(new Size(widthConstraint, heightConstraint));

    //    //    var result = StackList.Measure(widthConstraint, double.PositiveInfinity);
    //    //    double height = StackList.HeightMeasure;

    //    //    lastWidth = widthConstraint;
    //    //    lastHeight = height;

    //    //    return new SizeRequest(new Size(widthConstraint, height));
    //    //}

    //    //protected override void LayoutChildren(double x, double y, double width, double height)
    //    //{
    //    //    //if (!double.IsInfinity(lastHeight))
    //    //    //    height = lastHeight;
    //    //    //else
    //    //    //    height = StackList.Measure(width, double.PositiveInfinity).Request.Height;

    //    //    var rect = new Rectangle(0, 0, width, height);

    //    //    LayoutChildIntoBoundingRegion(StackList, rect);

    //    //    //if (OtherContent != null)
    //    //    //    LayoutChildIntoBoundingRegion(OtherContent, rect);
    //    //}
    //}
}
