using DataGridSam.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal class GridCell
    {
        internal DataGridColumn Column;
        internal BoxView Wrapper;
        internal View Content;
        internal GridRow Row;
        internal Label Label {
            get         => Content as Label;
            private set => Content = value;
        }

        public GridCell(GridRow row, DataGridColumn column)
        {
            Row = row;
            Column = column;

            // Create wrapper
            Wrapper = new BoxView();
            Wrapper.BackgroundColor = Color.Transparent;
            Wrapper.InputTransparent = true;

            // Create custom template
            if (column.CellTemplate != null)
            {
                Content = column.CellTemplate.CreateContent() as View;
                Content.BindingContext = row.Context;
                //Content.InputTransparent = true;
                //Content.VerticalOptions = LayoutOptions.FillAndExpand;

                if (CheckInput(Content))
                {
                    //Content.InputTransparent = false;
                }
                //if (Content is Layout layout)
                //{
                //    layout.CascadeInputTransparent = true;
                //    layout.InputTransparent = true;
                //}
            }
            // Create standart cell
            else
            {
                Label = new Label();
                Label.Margin = column.DataGrid.CellPadding;

                Label.InputTransparent = true;

                if (column.PropertyName != null)
                    Label.SetBinding(Label.TextProperty, new Binding(
                        column.PropertyName,
                        BindingMode.Default,
                        stringFormat: column.StringFormat,
                        source: row.Context));
            }

            // Set started column visible
            Content.IsVisible = column.IsVisible;
        }

        private bool CheckInput(Element element)
        {
            if (element is Layout layout)
            {
                layout.CascadeInputTransparent = false;
                layout.InputTransparent = true;
                foreach (var item in layout.Children)
                {
                    bool res = CheckInput(item);
                    if (res) return true;
                }
                return false;
            }
            else if (element is Button button)
            {
                button.InputTransparent = false;
                return true;
            }
            else if (element is Entry entry)
            {
                entry.InputTransparent = false;
                return true;
            }
            else if (element is View view)
            {
                view.InputTransparent = true;
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
