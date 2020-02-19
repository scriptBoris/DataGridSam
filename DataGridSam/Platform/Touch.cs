using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace DataGridSam.Platform
{
    public static class Touch
    {
        public static void Init() { }


        // Is enabled
        public static readonly BindableProperty IsEnabledProperty = BindableProperty.CreateAttached(
                "IsEnabled", typeof(bool), typeof(Touch), true,
                propertyChanged: IsEnabledChanged);
        public static void SetIsEnabled(BindableObject view, bool value)
        {
            view.SetValue(IsEnabledProperty, value);
        }
        public static bool GetIsEnabled(BindableObject view)
        {
            return (bool)view.GetValue(IsEnabledProperty);
        }

        // Color
        public static readonly BindableProperty ColorProperty = BindableProperty.CreateAttached(
                "Color", typeof(Color), typeof(Touch), Color.Default,
                propertyChanged: PropertyChanged);
        public static void SetColor(BindableObject view, Color value)
        {
            view.SetValue(ColorProperty, value);
        }
        public static Color GetColor(BindableObject view)
        {
            return (Color)view.GetValue(ColorProperty);
        }



        // Tap
        public static readonly BindableProperty TapProperty = BindableProperty.CreateAttached(
                "Tap", typeof(ICommand), typeof(Touch), default(ICommand),
                propertyChanged: PropertyChanged);
        public static void SetTap(BindableObject view, ICommand value)
        {
            view.SetValue(TapProperty, value);
        }
        public static ICommand GetTap(BindableObject view)
        {
            return (ICommand)view.GetValue(TapProperty);
        }



        // Tap param
        public static readonly BindableProperty TapParameterProperty = BindableProperty.CreateAttached(
                "TapParameter", typeof(object), typeof(Touch), null);
        public static void SetTapParameter(BindableObject view, object value)
        {
            view.SetValue(TapParameterProperty, value);
        }
        public static object GetTapParameter(BindableObject view)
        {
            return view.GetValue(TapParameterProperty);
        }



        // Long tap
        public static readonly BindableProperty LongTapProperty = BindableProperty.CreateAttached(
                "LongTap", typeof(ICommand), typeof(Touch), default(ICommand),
                propertyChanged: PropertyChanged);
        public static void SetLongTap(BindableObject view, ICommand value)
        {
            view.SetValue(LongTapProperty, value);
        }
        public static ICommand GetLongTap(BindableObject view)
        {
            return (ICommand)view.GetValue(LongTapProperty);
        }



        // Long tap latency
        public static readonly BindableProperty LongTapLatencyProperty = BindableProperty.CreateAttached(
                "LongTap", typeof(double), typeof(Touch), 700.0);
        public static void SetLongTapLatency(BindableObject view, double value)
        {
            view.SetValue(LongTapLatencyProperty, value);
        }
        public static double GetLongTapLatency(BindableObject view)
        {
            return (double)view.GetValue(LongTapLatencyProperty);
        }



        // Long tap parametr
        public static readonly BindableProperty LongTapParameterProperty = BindableProperty.CreateAttached(
                "LongTapParameter", typeof(object), typeof(Touch), null);
        public static void SetLongTapParameter(BindableObject view, object value)
        {
            view.SetValue(LongTapParameterProperty, value);
        }
        public static object GetLongTapParameter(BindableObject view)
        {
            return view.GetValue(LongTapParameterProperty);
        }


        private static void PropertyChanged(BindableObject b, object o, object n)
        {
            if (!(b is View view))
                return;

            var effect = view.Effects.FirstOrDefault(e => e is TouchEffect);

            if (GetColor(b) != Color.Default || GetTap(b) != null || GetLongTap(b) != null)
            {
                view.InputTransparent = false;

                if (effect != null)
                    return;

                var commandEffect = new TouchEffect();
                view.Effects.Add(commandEffect);
            }
            else
            {
                if (effect == null || view.BindingContext == null)
                    return;

                view.Effects.Remove(effect);
            }
        }

        private static void IsEnabledChanged(BindableObject b, object o, object n)
        {

        }
    }

    public class TouchEffect : RoutingEffect
    {
        public TouchEffect() : base($"DataGridSam.{nameof(Touch)}")
        {
        }
    }
}
