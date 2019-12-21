using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam
{
    public partial class DataGrid
    {
        private static Style headerDefaultStyle;

        internal static Style HeaderDefaultStyle { 
            get
            {
                if (headerDefaultStyle == null)
                {
                    headerDefaultStyle = new Style(typeof(Label));
                    headerDefaultStyle.Setters.Add(new Setter
                    {
                        Property = Label.VerticalOptionsProperty,
                        Value = LayoutOptions.FillAndExpand
                    });
                    headerDefaultStyle.Setters.Add(new Setter
                    {
                        Property = Label.HorizontalOptionsProperty,
                        Value = LayoutOptions.FillAndExpand
                    });
                    headerDefaultStyle.Setters.Add(new Setter
                    {
                        Property = Label.VerticalTextAlignmentProperty,
                        Value = TextAlignment.Center,
                    });
                    headerDefaultStyle.Setters.Add(new Setter
                    {
                        Property = Label.HorizontalTextAlignmentProperty,
                        Value = TextAlignment.Center,
                    });
                    headerDefaultStyle.Setters.Add(new Setter
                    {
                        Property = Label.LineBreakModeProperty,
                        Value = LineBreakMode.WordWrap,
                    });
                }
                return headerDefaultStyle;
            } 
        }
    }
}
