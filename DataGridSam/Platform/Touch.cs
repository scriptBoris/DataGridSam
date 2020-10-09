using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DataGridSam.Platform
{
    public static class Touch
    {
        public static void Init() { }


        // Data grid host
        public static readonly BindableProperty HostProperty = BindableProperty.CreateAttached(
                "Host", typeof(DataGrid), typeof(Touch), null,
                propertyChanged: PropertyChanged);
        public static void SetHost(BindableObject view, DataGrid value)
        {
            view.SetValue(HostProperty, value);
        }
        public static DataGrid GetHost(BindableObject view)
        {
            return (DataGrid)view.GetValue(HostProperty);
        }


        private static void PropertyChanged(BindableObject bindable, object o, object n)
        {
            if (!(bindable is View view))
                return;

            var effect = view.Effects.FirstOrDefault(e => e is TouchEffect);
            if (effect != null)
                return;

            view.InputTransparent = false;
            view.Effects.Add(new TouchEffect());
        }

        //// Color
        //public static readonly BindableProperty ColorProperty = BindableProperty.CreateAttached(
        //        "Color", typeof(Color), typeof(Touch), Color.Default);
        //public static void SetColor(BindableObject view, Color value)
        //{
        //    view.SetValue(ColorProperty, value);
        //}
        //public static Color GetColor(BindableObject view)
        //{
        //    return (Color)view.GetValue(ColorProperty);
        //}



        //// Select
        //public static readonly BindableProperty SelectProperty = BindableProperty.CreateAttached(
        //        "Select", typeof(ICommand), typeof(Touch), default(ICommand),
        //        propertyChanged: PropertyChanged);
        //public static void SetSelect(BindableObject view, ICommand value)
        //{
        //    view.SetValue(SelectProperty, value);
        //}
        //public static ICommand GetSelect(BindableObject view)
        //{
        //    return (ICommand)view.GetValue(SelectProperty);
        //}



        //// Tap
        //public static readonly BindableProperty TapProperty = BindableProperty.CreateAttached(
        //        "Tap", typeof(ICommand), typeof(Touch), default(ICommand));
        //public static void SetTap(BindableObject view, ICommand value)
        //{
        //    view.SetValue(TapProperty, value);
        //}
        //public static ICommand GetTap(BindableObject view)
        //{
        //    return (ICommand)view.GetValue(TapProperty);
        //}



        //// Long tap
        //public static readonly BindableProperty LongTapProperty = BindableProperty.CreateAttached(
        //        "LongTap", typeof(ICommand), typeof(Touch), default(ICommand));
        //public static void SetLongTap(BindableObject view, ICommand value)
        //{
        //    view.SetValue(LongTapProperty, value);
        //}
        //public static ICommand GetLongTap(BindableObject view)
        //{
        //    return (ICommand)view.GetValue(LongTapProperty);
        //}



        //// Long tap latency
        //public static readonly BindableProperty LongTapLatencyProperty = BindableProperty.CreateAttached(
        //        "LongTapLantency", typeof(double), typeof(Touch), 700.0);
        //public static void SetLongTapLatency(BindableObject view, double value)
        //{
        //    view.SetValue(LongTapLatencyProperty, value);
        //}
        //public static double GetLongTapLatency(BindableObject view)
        //{
        //    return (double)view.GetValue(LongTapLatencyProperty);
        //}



    }

    internal class TouchEffect : RoutingEffect
    {
        internal TouchEffect() : base($"DataGridSam.{nameof(Touch)}")
        {
        }
    }
}
