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
        internal ContentView Wrapper;
        internal View Content;
        internal Label Label {
            get         => Content as Label;
            private set => Content = value;
        }

        public GridCell(DataGridColumn column, GridRow row, DataGrid host)
        {
            Column = column;

            // Create wrapper
            Wrapper = new ContentView();
            Wrapper.BackgroundColor = Color.Transparent;
            //Wrapper.HeightRequest = 1.0;
            Wrapper.InputTransparent = true;

            // Create custom template
            if (column.CellTemplate != null)
            {
                IsCustomTemplate = true;
                Content = column.CellTemplate.CreateContent() as View;
                Content.BindingContext = row.BindingContext;
                //Content.HeightRequest = 1.0;
                //Content.VerticalOptions = LayoutOptions.StartAndExpand;

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
                        source: row.BindingContext));
            }

            Wrapper.Content = Content;

            // Set started column visible
            Content.IsVisible = column.IsVisible;

            // Detect auto number cell
            AutoNumber = column.AutoNumber;
        }
    }
}
