using System;
using System.Threading.Tasks;
using DataGridSam;
using DataGridSam.Platform;
using DataGridSam.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ResolutionGroupName("DataGridSam")]
[assembly: ExportEffect(typeof(TouchUWP), "Touch")]
namespace DataGridSam.UWP
{
    public class TouchUWP : PlatformEffect
    {
        public UIElement View => Control ?? Container;
        public bool IsDisposed => (Container as IVisualElementRenderer)?.Element == null;

        public static void Init() { }

        protected override void OnAttached()
        {
            if (View != null)
            {
                View.Tapped += OnTapped;
                View.RightTapped += OnRightTapped;
            }
        }

        protected override void OnDetached()
        {
            if (IsDisposed)
                return;

            if (View != null)
            {
                View.Tapped -= OnTapped;
                View.RightTapped -= OnRightTapped;
            }
        }

        private async void Tap()
        {
            var cmd = Touch.GetSelect(Element);

            if (Element is ContentView cont)
            {
                await cont.RelScaleTo(0.2, 40, Easing.SinIn);
                cmd.Execute(null);
                await cont.RelScaleTo(-0.2, 40, Easing.SinOut);
            }
            else
            {
                cmd.Execute(null);
            }
        }

        private void OnTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Tap();

            var cmd = Touch.GetTap(Element);
            if (cmd?.CanExecute(Element.BindingContext) ?? false)
                cmd.Execute(Element.BindingContext);
        }

        private void OnRightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            Tap();

            var cmd = Touch.GetLongTap(Element);
            if (cmd?.CanExecute(Element.BindingContext) ?? false)
                cmd.Execute(Element.BindingContext);
        }
    }
}
