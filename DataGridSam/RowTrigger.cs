using DataGridSam.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public sealed class RowTrigger : BindableObject
    {
        #region BindProps
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
        #endregion

        #region Methods
        internal static bool TrySetTriggerStyleRow(Row row, string propName)
        {
            if (row.DataGrid.RowTriggers.Count == 0)
                return false;

            // Any trigger is activated
            bool doneTriggerActivation = false;
            // Does this "propName" have anything to do with triggers
            bool isPropBeAnyTrigger = false;

            foreach (var trigger in row.DataGrid.RowTriggers)
            {
                if (propName == trigger.PropertyTrigger)
                {
                    isPropBeAnyTrigger = true;

                    var matchProperty = row.BindingContext.GetType().GetProperty(trigger.PropertyTrigger);
                    if (matchProperty == null)
                        continue;

                    var value = matchProperty.GetValue(row.BindingContext);

                    if (value is IComparable valueComparable && trigger.Value is IComparable tvalueComparable)
                    {
                        if (valueComparable.CompareTo(tvalueComparable) == 0)
                        {
                            doneTriggerActivation = true;
                            SetTriggerStyleRow(row, trigger);
                        }
                    }
                }
            }

            if (isPropBeAnyTrigger && !doneTriggerActivation && !row.isStyleDefault)
            {
                row.enableTrigger = null;
                row.SetStyleDefault();
            }

            return doneTriggerActivation;
        }

        internal static void SetTriggerStyleRow(Row row, RowTrigger trigger)
        {
            // Not change style Row
            // Selected row is has hight priority
            if (!row.isSelected)
            {
                foreach (var item in row.cells)
                {
                    if (item.IsCustomTemplate)
                        continue;

                    // Text color
                    item.Label.TextColor = ColorSelector.NoDefault(trigger.RowTextColor, row.DataGrid.RowsTextColor);

                    // Row background
                    item.Wrapper.BackgroundColor = ColorSelector.NoDefault(trigger.RowBackgroundColor, row.DataGrid.RowsColor);
                }
            }
            row.enableTrigger = trigger;
            row.isStyleDefault = false;
        }
        #endregion
    }
}
