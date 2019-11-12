using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam
{
    public class DataGridColumn : BindableObject, IDefinition
    {
        public DataGridColumn()
        {
            HeaderLabel = new Label();
        }

        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(string), typeof(DataGridColumn), string.Empty,
                propertyChanged: (b, o, n) =>
                {
                    (b as DataGridColumn).HeaderLabel.Text = (string)n;
                });

        public static readonly BindableProperty WidthProperty =
            BindableProperty.Create(nameof(Width), typeof(GridLength), typeof(DataGridColumn), new GridLength(1, GridUnitType.Star),
                propertyChanged: (b, o, n) => 
                {
                    if (o != n) (b as DataGridColumn).OnSizeChanged(); 
                });

        public static readonly BindableProperty PropertyNameProperty =
            BindableProperty.Create(nameof(PropertyName), typeof(string), typeof(DataGridColumn), null);

        public static readonly BindableProperty HorizontalContentAlignmentProperty =
            BindableProperty.Create(nameof(HorizontalContentAlignment), typeof(LayoutOptions), typeof(DataGridColumn), LayoutOptions.Center);

        public static readonly BindableProperty VerticalContentAlignmentProperty =
            BindableProperty.Create(nameof(VerticalContentAlignment), typeof(LayoutOptions), typeof(DataGridColumn), LayoutOptions.Center);

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public GridLength Width
        {
            get { return (GridLength)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public LayoutOptions HorizontalContentAlignment
        {
            get { return (LayoutOptions)GetValue(HorizontalContentAlignmentProperty); }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }

        public LayoutOptions VerticalContentAlignment
        {
            get { return (LayoutOptions)GetValue(VerticalContentAlignmentProperty); }
            set { SetValue(VerticalContentAlignmentProperty, value); }
        }


        public event EventHandler SizeChanged;
        internal Label HeaderLabel { get; set; }


        void OnSizeChanged()
        {
            SizeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
