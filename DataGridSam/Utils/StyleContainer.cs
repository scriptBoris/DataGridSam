using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    internal class StyleContainer
    {
        internal Color? BackgroundColor;
        internal Color? TextColor;
        internal FontAttributes? FontAttribute;
        internal string FontFamily;
        internal double FontSize;
        internal LineBreakMode? LineBreakMode;
        internal TextAlignment? HorizontalTextAlignment;
        internal TextAlignment? VerticalTextAlignment;

        internal static void MergeVisual(Label label, params StyleContainer[] styles)
        {
            label.TextColor = ValueSelector.GetTextColor(styles);
            label.FontAttributes = ValueSelector.FontAttribute(styles);
            label.FontFamily = ValueSelector.FontFamily(styles);
            label.FontSize = ValueSelector.FontSize(styles);

            label.LineBreakMode = ValueSelector.GetLineBreakMode(styles);
            label.VerticalTextAlignment = ValueSelector.GetVerticalAlignment(styles);
            label.HorizontalTextAlignment = ValueSelector.GetHorizontalAlignment(styles);
        }

        internal void OnUpdateStyle(Style style)
        {
            TextColor = null;
            FontAttribute = null;
            FontFamily = null;
            FontSize = 0;
            LineBreakMode = null;
            HorizontalTextAlignment = null;
            VerticalTextAlignment = null;

            if (style == null)
                return;

            foreach (var item in style.Setters)
            {
                if (item.Property == Label.TextColorProperty)
                {
                    TextColor = ValueSelector.GetValueFromStyle<Color>(item);
                }
                else if (item.Property == Label.FontAttributesProperty)
                {
                    FontAttribute = ValueSelector.GetValueFromStyle<FontAttributes>(item);
                }
                else if (item.Property == Label.FontFamilyProperty)
                {
                    FontFamily = ValueSelector.GetValueFromStyle<string>(item);
                }
                else if (item.Property == Label.FontSizeProperty)
                {
                    FontSize = ValueSelector.GetValueFromStyle<double>(item);
                }
                else if (item.Property == Label.VerticalTextAlignmentProperty)
                {
                    VerticalTextAlignment = ValueSelector.GetValueFromStyle<TextAlignment>(item);
                }
                else if (item.Property == Label.HorizontalTextAlignmentProperty)
                {
                    HorizontalTextAlignment = ValueSelector.GetValueFromStyle<TextAlignment>(item);
                }
            }
        }
    }
}
