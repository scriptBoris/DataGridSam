using DataGridSam.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    internal class GridCell : GridCellBase
    {
        protected override void BuildContent()
        {
            // Create custom template
            if (Column.CellTemplate != null)
            {
                Content = Column.CellTemplate.CreateContent() as View;
                Content.BindingContext = Row.Context;
                CheckInput(Content);
            }
            // Create standart cell
            else
            {
                Label = new Label();
                Label.Margin = Column.DataGrid.CellPadding;

                Label.InputTransparent = true;

                if (Column.PropertyName != null)
                    Label.SetBinding(Label.TextProperty, new Binding(
                        Column.PropertyName,
                        BindingMode.Default,
                        stringFormat: Column.StringFormat,
                        source: Row.Context));
                else
                    Label.RemoveBinding(Label.TextProperty);
            }
        }
    }
}
