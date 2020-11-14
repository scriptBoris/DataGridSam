using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Sample.Core
{
    public class NumberNothingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string none = "none";
            if (value is int INT)
            {
                if (INT == 0)
                    return none;
            }
            else if (value is decimal DEC)
            {
                if (DEC == 0)
                    return none;
            }
            else if (value is float FLT)
            {
                if (FLT == 0)
                    return none;
            }
            else if (value is double DBL)
            {
                if (DBL == 0)
                    return none;
            }
            else if (value is string STR)
            {
                if (STR == "0")
                    return none;
            }


            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
