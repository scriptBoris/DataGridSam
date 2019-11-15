using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public sealed class RowTrigger : BindableObject
    {
        // Property trigger
        public static readonly BindableProperty PropertyTriggerProperty =
            BindableProperty.Create(nameof(PropertyTrigger), typeof(string), typeof(RowTrigger), null);
        public string PropertyTrigger
        {
            get { return (string)GetValue(PropertyTriggerProperty); }
            set { SetValue(PropertyTriggerProperty, value); }
        }

        // Value
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(object), typeof(RowTrigger), null,
                propertyChanged: (b, o, n) =>
                {
                    var self = (RowTrigger)b;

                    if (n is string valueString)
                    {
                        if (valueString == "True")
                        {
                            self.SetValue(ValueProperty, true);
                            return;
                        }
                        else if (valueString == "False")
                        {
                            self.SetValue(ValueProperty, false);
                            return;
                        }
                        else if (int.TryParse(valueString, out int res))
                        {
                            self.SetValue(ValueProperty, res);
                            return;
                        }
                    }
                });
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Row background color
        public static readonly BindableProperty RowBackgroundColorProperty =
            BindableProperty.Create(nameof(RowBackgroundColor), typeof(Color), typeof(RowTrigger), null);
        public Color RowBackgroundColor
        {
            get { return (Color)GetValue(RowBackgroundColorProperty); }
            set { SetValue(RowBackgroundColorProperty, value); }
        }

        // Row text color
        public static readonly BindableProperty RowTextColorProperty =
            BindableProperty.Create(nameof(RowTextColor), typeof(Color), typeof(RowTrigger), null);
        public Color RowTextColor
        {
            get { return (Color)GetValue(RowTextColorProperty); }
            set { SetValue(RowTextColorProperty, value); }
        }
    }
}
