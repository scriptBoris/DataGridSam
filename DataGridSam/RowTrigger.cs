using DataGridSam.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public sealed class RowTrigger : BindableObject
    {
        // System
        private DataGrid grid;
        private Type sourceType;
        private PropertyInfo targetProp;

        // Fast variables
        private string valueString;
        private object valueTrigger;

        // Fast style variables
        internal VisualCollector VisualContainerStyle = new VisualCollector();
        internal VisualCollector VisualContainer = new VisualCollector();

        #region BindProps
        // Property trigger
        public static readonly BindableProperty PropertyTriggerProperty =
            BindableProperty.Create(nameof(PropertyTrigger), typeof(string), typeof(RowTrigger), null,
                propertyChanged: (b,o,n)=>
                {
                    var self = (RowTrigger)b;
                    self.Init();
                });
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
                    self.Init();
                });
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Row background color
        public static readonly BindableProperty RowBackgroundColorProperty =
            BindableProperty.Create(nameof(RowBackgroundColor), typeof(Color), typeof(RowTrigger), null,
                propertyChanged: (b, o, n) =>
                {
                    var self = (RowTrigger)b;
                    self.VisualContainer.BackgroundColor = (Color)n;
                });
        public Color RowBackgroundColor
        {
            get { return (Color)GetValue(RowBackgroundColorProperty); }
            set { SetValue(RowBackgroundColorProperty, value); }
        }

        // Row text style
        public static readonly BindableProperty RowTextStyleProperty =
            BindableProperty.Create(nameof(RowTextStyle), typeof(Style), typeof(RowTrigger), null,
                propertyChanged: (b, o, n) =>
                {
                    var self = (RowTrigger)b;
                    self.VisualContainerStyle.OnUpdateStyle(n as Style);
                });
        public Style RowTextStyle
        {
            get { return (Style)GetValue(RowTextStyleProperty); }
            set { SetValue(RowTextStyleProperty, value); }
        }

        // Row text color
        public static readonly BindableProperty RowTextColorProperty =
            BindableProperty.Create(nameof(RowTextColor), typeof(Color), typeof(RowTrigger), null,
                propertyChanged: (b, o, n) => {
                    var self = (RowTrigger)b;
                    self.VisualContainer.TextColor = (Color)n;
                });
        public Color RowTextColor
        {
            get { return (Color)GetValue(RowTextColorProperty); }
            set { SetValue(RowTextColorProperty, value); }
        }

        // Row font family
        public static readonly BindableProperty RowFontFamilyProperty =
            BindableProperty.Create(nameof(RowFontFamily), typeof(string), typeof(RowTrigger), null,
                propertyChanged: (b, o, n) =>
                {
                    var self = (RowTrigger)b;
                    self.VisualContainer.FontFamily = (string)n;
                });
        public string RowFontFamily
        {
            get { return (string)GetValue(RowFontFamilyProperty); }
            set { SetValue(RowFontFamilyProperty, value); }
        }

        // Row text attribute (bold, italic)
        public static readonly BindableProperty RowTextAttributeProperty =
            BindableProperty.Create(nameof(RowTextAttribute), typeof(FontAttributes), typeof(DataGrid), null,
                propertyChanged: (b, o, n) =>
                {
                    var self = (RowTrigger)b;
                    self.VisualContainer.FontAttribute = (FontAttributes)n;
                });
        public FontAttributes RowTextAttribute
        {
            get { return (FontAttributes)GetValue(RowTextAttributeProperty); }
            set { SetValue(RowTextAttributeProperty, value); }
        }
        #endregion

        #region Methods
        internal void Init()
        {
            if (targetProp == null)
                return;

            if (Value is string && Value != null)
            {
                valueString = Value.ToString();

                if (targetProp.PropertyType.IsEnum)
                {
                    foreach (var item in targetProp.PropertyType.GetFields())
                    {
                        if (item.Name == valueString)
                        {
                            var parse = Enum.Parse(targetProp.PropertyType, valueString, false);
                            if (parse != null)
                            {
                                valueTrigger = parse;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    valueTrigger = Convert.ChangeType(valueString, targetProp.PropertyType);
                }
            }
        }

        internal void OnAttached(DataGrid host)
        {
            grid = host;
        }

        internal void OnSourceTypeChanged(Type newSourceType)
        {
            sourceType = newSourceType;
            targetProp = sourceType?.GetProperty(PropertyTrigger);
            Init();
        }

        internal bool CheckTriggerActivated(object rowContext)
        {
            if (targetProp == null || valueTrigger == null)
                return false;

            var valueProp = targetProp.GetValue(rowContext);

            if (valueProp is IComparable valueComparable &&
                valueTrigger is IComparable tvalueComparable)
            {
                try
                {
                    if (valueComparable.CompareTo(tvalueComparable) == 0)
                        return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        internal static RowTrigger SetTriggerStyle(Row row, string propName, bool isNeedUpdate = true)
        {
            if (row.DataGrid.RowTriggers.Count == 0)
                return null;

            // Any trigger is activated
            RowTrigger anyTrigger = null;
            bool isTriggerActive = false;

            foreach (var trigger in row.DataGrid.RowTriggers)
            {
                if (propName == trigger.PropertyTrigger)
                {
                    anyTrigger = trigger;
                    if (trigger.CheckTriggerActivated(row.BindingContext))
                    {
                        isTriggerActive = true;
                        if (!isNeedUpdate)
                            return anyTrigger;
                    }
                    break;
                }
            }

            if (!isNeedUpdate)
                return null;

            if (anyTrigger == null)
                return null;

            if (anyTrigger != null && (row.enableTrigger==anyTrigger || row.enableTrigger==null) )
            {
                if (isTriggerActive)
                {
                    row.enableTrigger = anyTrigger;
                    row.UpdateStyle();
                }
                else
                {
                    row.enableTrigger = GetFirstTrigger(row);
                    row.UpdateStyle();
                }
            }

            return anyTrigger;
        }

        private static RowTrigger GetFirstTrigger(Row row)
        {
            foreach (var trigger in row.DataGrid.RowTriggers)
            {
                if (trigger.CheckTriggerActivated(row.BindingContext))
                {
                    return trigger;
                }
            }
            return null;
        }
        #endregion
    }
}
