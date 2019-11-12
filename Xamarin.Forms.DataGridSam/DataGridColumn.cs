using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam
{
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

        internal Label HeaderLabel { get; set; }


        void OnSizeChanged()
        {
            SizeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
