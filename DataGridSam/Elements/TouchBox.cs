using DataGridSam.Platform;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataGridSam.Elements
{
    public class TouchBox : ContentView
    {
        public object Context { get; private set; }

        public TouchBox(object context, DataGrid host, Action<object> touchAction)
        {
            BindingContext = context;

            BackgroundColor = Color.Transparent;
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            HeightRequest = 1.0;
            //InputTransparent = true;

            // Add tap system event
            Touch.SetSelect(this, new Command(touchAction));

            if (host.TapColor != Color.Default)
                Touch.SetColor(this, host.TapColor);

            if (host.CommandSelectedItem != null)
                Touch.SetTap(this, host.CommandSelectedItem);

            if (host.CommandLongTapItem != null)
                Touch.SetLongTap(this, host.CommandLongTapItem);
        }
    }
}
