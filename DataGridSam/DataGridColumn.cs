using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class DataGridColumn : BindableObject, IDefinition
    {
        public event EventHandler SizeChanged;

        public DataGridColumn()
        {
            HeaderLabel = new Label();
        }

        // Title
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(DataGridColumn), string.Empty,
                propertyChanged: (b, o, n) =>
                {
                    (b as DataGridColumn).HeaderLabel.Text = (string)n;
                });
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Width
        public static readonly BindableProperty WidthProperty =
            BindableProperty.Create(nameof(Width), typeof(GridLength), typeof(DataGridColumn), new GridLength(1, GridUnitType.Star),
                propertyChanged: (b, o, n) => 
                {
                    if (o != n) (b as DataGridColumn).OnSizeChanged(); 
                });
        public GridLength Width
        {
            get { return (GridLength)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        // String format
        public static readonly BindableProperty StringFormatProperty =
            BindableProperty.Create(nameof(StringFormat), typeof(string), typeof(DataGridColumn), null);
        public string StringFormat
        {
            get { return (string)GetValue(StringFormatProperty); }
            set { SetValue(StringFormatProperty, value); }
        }

        // Property name
        public static readonly BindableProperty PropertyNameProperty =
            BindableProperty.Create(nameof(PropertyName), typeof(string), typeof(DataGridColumn), null);
        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        // Cell text color
        public static readonly BindableProperty CellTextColorProperty =
            BindableProperty.Create(nameof(CellTextColor), typeof(Color), typeof(DataGridColumn), null);
        public Color CellTextColor
        {
            get { return (Color)GetValue(CellTextColorProperty); }
            set { SetValue(CellTextColorProperty, value); }
        }

        // Cell background color
        public static readonly BindableProperty CellBackgroundColorProperty =
            BindableProperty.Create(nameof(CellBackgroundColor), typeof(Color), typeof(DataGridColumn), null);
        public Color CellBackgroundColor
        {
            get { return (Color)GetValue(CellBackgroundColorProperty); }
            set { SetValue(CellBackgroundColorProperty, value); }
        }

        // Horizontal content aligment
        public static readonly BindableProperty HorizontalContentAlignmentProperty =
            BindableProperty.Create(nameof(HorizontalContentAlignment), typeof(LayoutOptions), typeof(DataGridColumn), LayoutOptions.FillAndExpand);
        public LayoutOptions HorizontalContentAlignment
        {
            get { return (LayoutOptions)GetValue(HorizontalContentAlignmentProperty); }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }

        // Vertical content aligment
        public static readonly BindableProperty VerticalContentAlignmentProperty =
            BindableProperty.Create(nameof(VerticalContentAlignment), typeof(LayoutOptions), typeof(DataGridColumn), LayoutOptions.FillAndExpand);
        public LayoutOptions VerticalContentAlignment
        {
            get { return (LayoutOptions)GetValue(VerticalContentAlignmentProperty); }
            set { SetValue(VerticalContentAlignmentProperty, value); }
        }

        // Horizontal text aligment
        public static readonly BindableProperty HorizontalTextAlignmentProperty =
            BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(DataGridColumn), TextAlignment.Start);
        public TextAlignment HorizontalTextAlignment
        {
            get { return (TextAlignment)GetValue(HorizontalTextAlignmentProperty); }
            set { SetValue(HorizontalTextAlignmentProperty, value); }
        }

        // Vertical text aligment
        public static readonly BindableProperty VerticalTextAlignmentProperty =
            BindableProperty.Create(nameof(VerticalTextAlignment), typeof(TextAlignment), typeof(DataGridColumn), TextAlignment.Start);
        public TextAlignment VerticalTextAlignment
        {
            get { return (TextAlignment)GetValue(VerticalTextAlignmentProperty); }
            set { SetValue(VerticalTextAlignmentProperty, value); }
        }

        // Header label style
        public static readonly BindableProperty HeaderLabelStyleProperty =
            BindableProperty.Create(nameof(HeaderLabelStyle), typeof(Style), typeof(DataGridColumn),
                propertyChanged: (b, old, newValue) => {
                    var self = (DataGridColumn)b;
                    if (self.HeaderLabel != null && (old != newValue))
                        self.HeaderLabel.Style = newValue as Style;
                });
        public Style HeaderLabelStyle
        {
            get { return (Style)GetValue(HeaderLabelStyleProperty); }
            set { SetValue(HeaderLabelStyleProperty, value); }
        }

        // Cell template
        public static readonly BindableProperty CellTemplateProperty =
            BindableProperty.Create(nameof(CellTemplate), typeof(DataTemplate), typeof(DataGridColumn), null);
        /// <summary>
        /// Custom cell template (default: null)
        /// </summary>
        public DataTemplate CellTemplate
        {
            get { return (DataTemplate)GetValue(CellTemplateProperty); }
            set { SetValue(CellTemplateProperty, value); }
        }

        #region Other
        internal Label HeaderLabel { get; set; }

        void OnSizeChanged()
        {
            SizeChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
