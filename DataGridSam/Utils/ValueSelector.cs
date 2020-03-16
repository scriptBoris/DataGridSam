using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    internal static class ValueSelector
    {
        internal static Color GetBackgroundColor(params Color?[] colors)
        {
            foreach (var item in colors)
            {
                if (item == null)
                    continue;

                if (item.HasValue && !item.Value.IsDefault)
                    return item.Value;
            }

            return Xamarin.Forms.Color.White;
        }

        internal static Color GetTextColor(StyleContainer[] styles)
        {
            foreach (var item in styles)
            {
                if (item == null)
                    continue;

                if (item.TextColor.HasValue && !item.TextColor.Value.IsDefault)
                    return item.TextColor.Value;
            }

            return Xamarin.Forms.Color.Black;
        }

        internal static LineBreakMode GetLineBreakMode(StyleContainer[] styles)
        {
            foreach (var item in styles)
            {
                if (item?.LineBreakMode.HasValue ?? false)
                    return item.LineBreakMode.Value;
            }

            return LineBreakMode.WordWrap;
        }

        internal static TextAlignment GetVerticalAlignment(StyleContainer[] styles)
        {
            foreach (var item in styles)
            {
                if (item?.VerticalTextAlignment.HasValue ?? false)
                    return item.VerticalTextAlignment.Value;
            }

            return TextAlignment.Start;
        }

        internal static TextAlignment GetHorizontalAlignment(StyleContainer[] styles)
        {
            foreach (var item in styles)
            {
                if (item?.HorizontalTextAlignment.HasValue ?? false)
                    return item.HorizontalTextAlignment.Value;
            }

            return TextAlignment.Start;
        }

        internal static FontAttributes FontAttribute(StyleContainer[] styles)
        {
            foreach (var item in styles)
            {
                if (item == null)
                    continue;

                if (item.FontAttribute.HasValue && item.FontAttribute.Value != FontAttributes.None)
                    return item.FontAttribute.Value;
            }

            return FontAttributes.None;
        }

        internal static double FontSize(StyleContainer[] styles)
        {
            foreach (var item in styles)
            {
                if (item == null)
                    continue;

                if (item.FontSize > 0.1)
                    return item.FontSize;
            }

            return 0.0;
        }

        internal static string FontFamily(StyleContainer[] styles)
        {
            foreach (var item in styles)
            {
                if (item == null)
                    continue;

                if (item.FontFamily != null)
                    return item.FontFamily;
            }

            return null;
        }

        

        internal static T GetValueFromStyle<T>(Setter setter)
        {
            if (setter.Value is OnPlatform<T> onPlatform)
            {
                if (onPlatform.Platforms.Count > 0)
                {
                    foreach (var platform in onPlatform.Platforms)
                    {
                        var match = platform.Platform.FirstOrDefault(x => x == Device.RuntimePlatform);
                        if (match != null)
                            return (T)platform.Value;
                    }
                    return default;
                }
                else
                    return default;
            }
            // TODO Add support for OnIdiom values
            else if (setter.Value is OnIdiom<T> onIdiom)
            {
                switch (Device.Idiom)
                {
                    case TargetIdiom.Desktop:
                        return onIdiom.Desktop;

                    case TargetIdiom.Phone:
                        return onIdiom.Phone;

                    case TargetIdiom.Tablet:
                        return onIdiom.Tablet;

                    case TargetIdiom.TV:
                        return onIdiom.TV;

                    case TargetIdiom.Watch:
                        return onIdiom.Watch;
                    
                    case TargetIdiom.Unsupported:
                        return default;
                    
                    default:
                        return default;
                }
            }
            else
                return (T)setter.Value;
        }
    }
}
