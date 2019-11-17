using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    internal static class ColorSelector
    {
        internal static Color NoDefault(params Color[] colors)
        {
            foreach (var item in colors)
            {
                if (!item.IsDefault)
                    return item;
            }
            return colors.LastOrDefault();
        }

        internal static Color NoTransperent(params Color[] colors)
        {
            foreach (var item in colors)
            {
                if (item.A != 1)
                    return item;
            }
            return colors.LastOrDefault();
        }
    }
}
