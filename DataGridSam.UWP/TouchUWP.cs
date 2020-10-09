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
        private DataGrid host;

        public static void Init() { }

        protected override void OnAttached()
        {
            if (View != null)
            {
                host = Touch.GetHost(Element);
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
            if (Element is ContentView cont)
            {
                await cont.RelScaleTo(0.2, 40, Easing.SinIn);
                host.SelectedItem = Element.BindingContext;
                await cont.RelScaleTo(-0.2, 40, Easing.SinOut);
            }
            else
            {
                host.SelectedItem = Element.BindingContext;
            }
        }

        private void OnTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Tap();

            var cmd = host.CommandSelectedItem;
            if (cmd?.CanExecute(Element.BindingContext) ?? false)
                cmd.Execute(Element.BindingContext);
        }

        private void OnRightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            Tap();

            var cmd = host.CommandLongTapItem; //Touch.GetLongTap(Element);
            if (cmd?.CanExecute(Element.BindingContext) ?? false)
                cmd.Execute(Element.BindingContext);
        }
    }
}
