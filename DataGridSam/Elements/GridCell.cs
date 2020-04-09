using DataGridSam.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal class GridCell
    {
        internal bool IsCustomTemplate;
        internal DataGridColumn Column;
        internal BoxView Wrapper;
        internal View Content;
        internal IGridRow Row;
        internal Label Label {
            get         => Content as Label;
            private set => Content = value;
        }
        internal int Index => Column.Index;

        public GridCell(DataGridColumn column, IGridRow row, DataGrid host)
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
                IsCustomTemplate = true;
                Content = column.CellTemplate.CreateContent() as View;
                Content.BindingContext = row.Context;
                Content.VerticalOptions = LayoutOptions.FillAndExpand;

                if (Content is Layout layout)
                {
                    layout.InputTransparent = true;
                    layout.CascadeInputTransparent = true;
                }
            }
            // Create standart cell
            else
            {
                Label = new Label();
                Label.Margin = host.CellPadding;
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
    }
}
