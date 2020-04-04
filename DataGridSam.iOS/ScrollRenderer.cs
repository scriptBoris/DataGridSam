using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataGridSam.Elements;
using DataGridSam.iOS;
using DataGridSam.Utils;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GridScroll), typeof(ScrollRenderer))]
namespace DataGridSam.iOS
{
    public class ScrollRenderer : ScrollViewRenderer
    {
        public ScrollRenderer()
        {

        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (Element != null)
            {
                if (NativeView is UIScrollView scroll)
                {
                    scroll.Bounces = false;
                }
            }
        }
    }
}