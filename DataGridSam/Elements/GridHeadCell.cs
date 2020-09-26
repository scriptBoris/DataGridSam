using DataGridSam.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal class GridHeadCell
    {
        internal DataGridColumn Column;
        internal BoxView BackgroundBox;
        internal View Content;
        internal GridHeadRow Row;
        internal Label Label {
            get         => Content as Label;
            private set => Content = value;
        }

        public GridHeadCell(GridHeadRow row, DataGridColumn column)
        {
            Row = row;
            Column = column;

            // Create wrapper
            BackgroundBox = new BoxView();
            BackgroundBox.BackgroundColor = Color.Transparent;
            BackgroundBox.InputTransparent = true;

            // TODO Create column header custom template
            // Create custom template
            //if (column.Hea != null)
            //{
            //    Content = column.CellTemplate.CreateContent() as View;
            //    Content.BindingContext = row.Context;
            //    CheckInput(Content);
            //}
            //// Create standart cell
            //else
            //{
            //    Label = new Label();
            //    Label.Text = column.Title;
            //}
            Label = new Label();
            Label.Text = column.Title;

            // Set started column visible
            Content.IsVisible = column.IsVisible;
        }

        //private bool CheckInput(Element element)
        //{
        //    if (element is Layout layout)
        //    {
        //        layout.CascadeInputTransparent = false;
        //        layout.InputTransparent = true;
        //        foreach (var item in layout.Children)
        //        {
        //            bool res = CheckInput(item);
        //            if (res) return true;
        //        }
        //        return false;
        //    }
        //    else if (element is Button button)
        //    {
        //        button.InputTransparent = false;
        //        return true;
        //    }
        //    else if (element is Entry entry)
        //    {
        //        entry.InputTransparent = false;
        //        return true;
        //    }
        //    else if (element is View view)
        //    {
        //        view.InputTransparent = true;
        //        return false;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
