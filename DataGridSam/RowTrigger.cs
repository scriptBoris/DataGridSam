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

        // Row text attribute (bold, italic)
        public static readonly BindableProperty RowTextAttributeProperty =
            BindableProperty.Create(nameof(RowTextAttribute), typeof(FontAttributes), typeof(DataGrid), null);
        public FontAttributes RowTextAttribute
        {
            get { return (FontAttributes)GetValue(RowTextAttributeProperty); }
            set { SetValue(RowTextAttributeProperty, value); }
        }
        #endregion

        #region Methods
        internal static RowTrigger TrySetTriggerStyleRow(Row row, string propName, bool isNeedUpdate = true)
        {
            if (row.DataGrid.RowTriggers.Count == 0)
                return null;

            // Any trigger is activated
            RowTrigger activeTrigger = null;
            bool isTriggerActive = false;

            foreach (var trigger in row.DataGrid.RowTriggers)
            {
                if (propName == trigger.PropertyTrigger)
                {
                    var matchProperty = row.bindingTypeModel.GetProperty(trigger.PropertyTrigger);
                    if (matchProperty == null)
                        continue;

                    activeTrigger = trigger;
                    var value = matchProperty.GetValue(row.BindingContext);
                    if (value is IComparable valueComparable && trigger.Value is IComparable tvalueComparable)
                    {
                        if (valueComparable.CompareTo(tvalueComparable) == 0)
                        {
                            isTriggerActive = true;
                            if (!isNeedUpdate) 
                                return activeTrigger;
                        }
                    }
                    break;
                }
            }

            if (!isNeedUpdate)
                return null;

            if (activeTrigger == null)
                return null;

            if (activeTrigger != null && (row.enableTrigger==activeTrigger || row.enableTrigger==null) )
            {
                if (isTriggerActive)
                {
                    row.enableTrigger = activeTrigger;
                    row.UpdateStyle();
                }
                else
                {
                    row.enableTrigger = TrySetTriggerStyleRow(row);
                    row.UpdateStyle();
                }
            }

            return activeTrigger;
        }

        private static RowTrigger TrySetTriggerStyleRow(Row row)
        {
            foreach (var trigger in row.DataGrid.RowTriggers)
            {
                var matchProperty = row.bindingTypeModel.GetProperty(trigger.PropertyTrigger);
                if (matchProperty == null)
                    continue;

                var value = matchProperty.GetValue(row.BindingContext);
                if (value is IComparable valueComparable && trigger.Value is IComparable tvalueComparable)
                {
                    if (valueComparable.CompareTo(tvalueComparable) == 0)
                    {
                        row.enableTrigger = trigger;
                        row.UpdateStyle();

                        return trigger;
                    }
                }
            }
            return null;
        }
        #endregion
    }
}
