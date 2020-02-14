using DataGridSam.Platform;
using DataGridSam.UWP;
using Xamarin.Forms;
using Windows.UI.Xaml;
using Xamarin.Forms.Platform.UWP;

[assembly: ResolutionGroupName("DataGridSam")]
[assembly: ExportEffect(typeof(CommandsPlatform), nameof(Commands))]
namespace DataGridSam.UWP
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class CommandsPlatform : PlatformEffect
    {
        public UIElement View => Control ?? Container;
        public bool IsDisposed => (Container as IVisualElementRenderer)?.Element == null;


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

        private void OnTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ClickHandler();
        }

        private void OnRightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            var cmd = Commands.GetLongTap(Element);
            var param = Commands.GetLongTapParameter(Element);
            if (cmd?.CanExecute(param) ?? false)
                cmd.Execute(param);
        }

        private void ClickHandler()
        {
            var cmd = Commands.GetTap(Element);
            var param = Commands.GetTapParameter(Element);
            if (cmd?.CanExecute(param) ?? false)
                cmd.Execute(param);
        }

        public static void Init()
        {
        }
    }
}
