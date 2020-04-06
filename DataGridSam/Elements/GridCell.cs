using DataGridSam.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal class GridCell
    {
        internal AutoNumberType AutoNumber;
        internal bool IsCustomTemplate;
        internal DataGridColumn Column;
        internal RequestBox Wrapper;
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
            Wrapper = new RequestBox(row);
            Wrapper.BackgroundColor = Color.Transparent;
            //Wrapper.HeightRequest = 1.0;
            Wrapper.InputTransparent = true;

            //if (row is RowCustom)
            //    Wrapper.SizeChanged += Wrapper_SizeChanged;

            // Create custom template
            if (column.CellTemplate != null)
            {
                IsCustomTemplate = true;
                Content = column.CellTemplate.CreateContent() as View;
                Content.BindingContext = row.Context;
                Content.VerticalOptions = LayoutOptions.FillAndExpand;
                Content.HorizontalOptions = LayoutOptions.FillAndExpand;
                //Content.HeightRequest = 1.0;

                if (Content is Layout layout)
                {
                    layout.IsClippedToBounds = true;
                    layout.InputTransparent = true;
                    layout.CascadeInputTransparent = true;
                }
            }
            // Create standart cell
            else
            {
                Label = new Label();
                Label.Margin = host.CellPadding;
                Label.HorizontalOptions = LayoutOptions.FillAndExpand;
                Label.VerticalOptions = LayoutOptions.FillAndExpand;

                if (column.PropertyName != null)
                    Label.SetBinding(Label.TextProperty, new Binding(
                        column.PropertyName,
                        BindingMode.Default,
                        stringFormat: column.StringFormat,
                        source: row.Context));
            }

            Wrapper.Content = Content;

            // Set started column visible
            Content.IsVisible = column.IsVisible;

            // Detect auto number cell
            AutoNumber = column.AutoNumber;
        }

        private void Wrapper_SizeChanged(object sender, EventArgs e)
        {
            var row = Row as RowCustom;
            if (row.isSolve)
                row.UpdateHeight(Wrapper.Height);
        }
    }
}
