using DataGridSam.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal abstract class GridCellBase
    {
        internal DataGridColumn Column;
        internal IGridRow Row;
        internal BoxView BackgroundBox;

        internal View Content;
        internal Label Label
        {
            get => Content as Label;
            set => Content = value;
        }

        public GridCellBase()
        {
        }

        internal void Init(IGridRow row, DataGridColumn column)
        {
            Column = column;
            Row = row;

            // Create wrapper
            BackgroundBox = new BoxView();
            BackgroundBox.BackgroundColor = Color.Transparent;
            BackgroundBox.InputTransparent = true;

            //// Create custom template
            //if (column.CellTemplate != null)
            //{
            //    Content = column.CellTemplate.CreateContent() as View;
            //    Content.BindingContext = row.Context;
            //    CheckInput(Content);
            //}
            //// Create standart cell
            //else
            //{
            //    Label = new Label();
            //    Label.Margin = column.DataGrid.CellPadding;

            //    Label.InputTransparent = true;

            //    if (column.PropertyName != null)
            //        Label.SetBinding(Label.TextProperty, new Binding(
            //            column.PropertyName,
            //            BindingMode.Default,
            //            stringFormat: column.StringFormat,
            //            source: row.Context));
            //    else
            //        Label.RemoveBinding(Label.TextProperty);
            //}

            BuildContent();

            // Set started column visible
            Content.IsVisible = column.IsVisible;
        }

        protected virtual void BuildContent()
        {

        }

        protected bool CheckInput(Element element)
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
