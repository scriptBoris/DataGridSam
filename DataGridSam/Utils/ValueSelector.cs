using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Utils
{
    internal static class ValueSelector
    {
        internal static Color Color(params Color[] colors)
        {
            foreach (var item in colors)
            {
                if (!item.IsDefault)
                    return item;
            }
            return colors.LastOrDefault();
        }

        internal static FontAttributes FontAttribute(params FontAttributes[] values)
        {
            foreach (var item in values)
            {
                if (item != FontAttributes.None)
                    return item;
            }
            return values.LastOrDefault();
        }
    }
}
