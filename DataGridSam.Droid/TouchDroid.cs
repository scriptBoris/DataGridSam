using Android.Animation;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using DataGridSam.Platform;
using System;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ResolutionGroupName("DataGridSam")]
[assembly: Xamarin.Forms.ExportEffect(typeof(DataGridSam.Droid.TouchDroid), "Touch")]
namespace DataGridSam.Droid
{
    public class TouchDroid : PlatformEffect
    {
        readonly Rect _rect = new Rect();
        readonly int[] _location = new int[2];

        private System.Timers.Timer timer;
        //private MotionEvent motion;
        private Color color;
        private byte alpha;
        private ObjectAnimator animation;
        private FrameLayout viewOverlay;
        private RippleDrawable ripple;
        private bool isEnabled;

        public bool EnableRipple => Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop;
        public View View => Control ?? Container;
        public bool IsDisposed => (Container as IVisualElementRenderer)?.Element == null;

        public static void Init()
        {
        }

        protected override void OnAttached()
        {
            if (Touch.GetLongTap(Element) != null)
            {
                timer = new System.Timers.Timer();
                timer.Elapsed += OnTimerEvent;
            }
            isEnabled = Touch.GetIsEnabled(Element);
            View.Clickable = true;
            View.LongClickable = true;
            View.SoundEffectsEnabled = true;

            viewOverlay = new FrameLayout(Container.Context)
            {
                LayoutParameters = new ViewGroup.LayoutParams(-1, -1),
                Clickable = false,
                Focusable = false,
            };
            Container.LayoutChange += ViewOnLayoutChange;

            if (EnableRipple)
                viewOverlay.Background = CreateRipple(color);

            SetEffectColor();
            View.Touch += OnTouch;

            Container.AddView(viewOverlay);
            viewOverlay.BringToFront();
        }

        protected override void OnDetached()
        {
            if (IsDisposed)
                return;

            Container.RemoveView(viewOverlay);
            viewOverlay.Pressed = false;
            viewOverlay.Foreground = null;
            viewOverlay.Dispose();
            Container.LayoutChange -= ViewOnLayoutChange;

            if (timer != null)
            {
                timer.Elapsed -= OnTimerEvent;
                timer.Stop();
                timer.Close();
            }

            if (EnableRipple)
                ripple?.Dispose();

            View.Touch -= OnTouch;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == Touch.ColorProperty.PropertyName)
            {
                SetEffectColor();
            }
            else if (e.PropertyName == Touch.IsEnabledProperty.PropertyName)
            {
                isEnabled = Touch.GetIsEnabled(Element);
            }
        }

        void OnTouch(object sender, View.TouchEventArgs args)
        {
            switch (args.Event.Action)
            {
                case MotionEventActions.Down:
                    if (EnableRipple)
                        ForceStartRipple(args.Event.GetX(), args.Event.GetY());
                    else
                        StartAnimation();

                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    if (IsDisposed) return;

                    if (EnableRipple)
                        ForceEndRipple();
                    else
                        TapAnimation(250, alpha, 0);

                    break;
            }
        }

        void OnTouch1(object sender, View.TouchEventArgs args)
        {
            if (!isEnabled)
                return;

            //var x = args.Event.GetX();
            //var y = args.Event.GetY();
            //Console.Out.WriteLine($"x: {x}; y: {y} (action: {args.Event.Action.ToString()})");
            //motion = args.Event;

            if (args.Event.Action == MotionEventActions.Down)
            {
                View.PlaySoundEffect(SoundEffects.Click);
                // DOWN
                //if (EnableRipple)
                //    ForceStartRipple(args.Event.GetX(), args.Event.GetY());
                //else
                //    StartAnimation();

                if (Touch.GetLongTap(Element) != null)
                {
                    if (timer == null)
                    {
                        timer = new System.Timers.Timer();
                        timer.Elapsed += OnTimerEvent;
                    }
                    timer.Interval = Touch.GetLongTapLatency(Element);
                    timer.AutoReset = false;
                    timer.Start();
                }
            }
            else
            if (args.Event.Action == MotionEventActions.Up ||
                args.Event.Action == MotionEventActions.Cancel ||
                args.Event.Action == MotionEventActions.Outside)
            {
                args.Handled = true;
                // UP
                if (IsDisposed)
                    return;

                //if (EnableRipple)
                //    ForceEndRipple();
                //else
                //    TapAnimation(250, alpha, 0);

                if (IsViewInBounds((int)args.Event.RawX, (int)args.Event.RawY))
                {
                    if (Touch.GetLongTap(Element) == null)
                        ClickHandler();
                    else if (timer == null || timer.Enabled)
                        ClickHandler();
                }

                timer?.Stop();
            }
        }

        bool IsViewInBounds(int x, int y)
        {
            View.GetDrawingRect(_rect);
            View.GetLocationOnScreen(_location);
            _rect.Offset(_location[0], _location[1]);
            return _rect.Contains(x, y);
        }

        void ClickHandler()
        {
            var cmd = Touch.GetTap(Element);
            var param = Touch.GetTapParameter(Element);
            if (cmd == null)
                return;

            if (cmd.CanExecute(param))
                cmd.Execute(param);
        }

        void LongClickHandler()
        {
            var cmdLong = Touch.GetLongTap(Element);
            var paramLong = Touch.GetLongTapParameter(Element);

            if (cmdLong == null)
            {
                ClickHandler();
                return;
            }

            if (cmdLong.CanExecute(paramLong))
                cmdLong.Execute(paramLong);
        }

        #region TouchPart
        private void OnTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            var x = 0;// motion?.GetX();
            var y = 0;// motion?.GetY();
            if (IsViewInBounds((int)x, (int)y))
            {
                timer.Stop();
                Xamarin.Forms.Device.BeginInvokeOnMainThread(LongClickHandler);
            }
        }

        private void SetEffectColor()
        {
            var color = Touch.GetColor(Element);
            if (color == Xamarin.Forms.Color.Default)
            {
                return;
            }

            this.color = color.ToAndroid();
            alpha = this.color.A == 255 ? (byte)80 : this.color.A;

            if (EnableRipple)
            {
                ripple.SetColor(GetPressedColorSelector(this.color));
            }
        }

        private void ViewOnLayoutChange(object sender, View.LayoutChangeEventArgs layoutChangeEventArgs)
        {
            var group = (ViewGroup)sender;
            if (group == null || IsDisposed)
                return;

            viewOverlay.Right = group.Width;
            viewOverlay.Bottom = group.Height;
        }
        #endregion

        #region Ripple
        private RippleDrawable CreateRipple(Color color)
        {
            if (Element is Xamarin.Forms.Layout)
            {
                var mask = new ColorDrawable(Color.White);
                return ripple = new RippleDrawable(GetPressedColorSelector(color), null, mask);
            }

            var back = View.Background;
            if (back == null)
            {
                var mask = new ColorDrawable(Color.White);
                return ripple = new RippleDrawable(GetPressedColorSelector(color), null, mask);
            }

            if (back is RippleDrawable)
            {
                ripple = (RippleDrawable)back.GetConstantState().NewDrawable();
                ripple.SetColor(GetPressedColorSelector(color));

                return ripple;
            }

            return ripple = new RippleDrawable(GetPressedColorSelector(color), back, null);
        }

        private static ColorStateList GetPressedColorSelector(int pressedColor)
        {
            return new ColorStateList(
                new[] { new int[] { } },
                new[] { pressedColor, });
        }

        private void ForceStartRipple(float x, float y)
        {
            if (IsDisposed || !(viewOverlay.Background is RippleDrawable bc))
                return;

            viewOverlay.BringToFront();
            bc.SetHotspot(x, y);
            viewOverlay.Pressed = true;
        }

        private void ForceEndRipple()
        {
            if (IsDisposed) return;

            viewOverlay.Pressed = false;
        }

        #endregion

        #region Overlay
        private void StartAnimation()
        {
            if (IsDisposed)
                return;

            ClearAnimation();

            viewOverlay.BringToFront();
            var color = this.color;
            color.A = alpha;
            viewOverlay.SetBackgroundColor(color);
        }

        private void TapAnimation(long duration, byte startAlpha, byte endAlpha)
        {
            if (IsDisposed)
                return;

            viewOverlay.BringToFront();

            var start = color;
            var end = color;
            start.A = startAlpha;
            end.A = endAlpha;

            ClearAnimation();
            animation = ObjectAnimator.OfObject(viewOverlay,
                "BackgroundColor",
                new ArgbEvaluator(),
                start.ToArgb(),
                end.ToArgb());
            animation.SetDuration(duration);
            animation.RepeatCount = 0;
            animation.RepeatMode = ValueAnimatorRepeatMode.Restart;
            animation.Start();
            animation.AnimationEnd += AnimationOnAnimationEnd;
        }

        private void AnimationOnAnimationEnd(object sender, EventArgs eventArgs)
        {
            if (IsDisposed) return;

            ClearAnimation();
        }

        private void ClearAnimation()
        {
            if (animation == null) return;
            animation.AnimationEnd -= AnimationOnAnimationEnd;
            animation.Cancel();
            animation.Dispose();
            animation = null;
        }
        #endregion
    }
}