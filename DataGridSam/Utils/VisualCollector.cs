using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    internal class VisualCollector
    {
        internal Color? BackgroundColor;
        internal Color? TextColor;
        internal FontAttributes? FontAttribute;
        internal string FontFamily;
        internal double FontSize;
        internal LineBreakMode? LineBreakMode;
        internal TextAlignment? HorizontalTextAlignment;
        internal TextAlignment? VerticalTextAlignment;

        internal void OnUpdateStyle(Style style)
        {
            BackgroundColor = null;
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
                if (item.Property == Label.BackgroundColorProperty)
                {
                    BackgroundColor = ValueSelector.GetValueFromStyle<Color>(item);
                    BackgroundColor.Value.MultiplyAlpha(0.5);
                }
                else if (item.Property == Label.TextColorProperty)
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
