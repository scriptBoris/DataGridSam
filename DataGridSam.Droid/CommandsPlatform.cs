using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DataGridSam.Droid;
using DataGridSam.Droid.GestureCollectors;
using DataGridSam.Platform;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ResolutionGroupName("DataGridSam")]
[assembly: Xamarin.Forms.ExportEffect(typeof(CommandsPlatform), nameof(Commands))]
namespace DataGridSam.Droid
{
    public class CommandsPlatform : PlatformEffect
    {
        readonly System.Timers.Timer _timer = new System.Timers.Timer();
        readonly Rect _rect = new Rect();
        readonly int[] _location = new int[2];

        private Color _color;
        private byte _alpha;
        private RippleDrawable _ripple;
        private FrameLayout _viewOverlay;
        private ObjectAnimator _animator;

        public bool EnableRipple => Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop;
        public View View => Control ?? Container;
        public bool IsDisposed => (Container as IVisualElementRenderer)?.Element == null;

        public static void Init()
        {
        }

        protected override void OnAttached()
        {
            _timer.Elapsed += OnTimerEvent;

            View.Clickable = true;
            View.LongClickable = true;
            View.SoundEffectsEnabled = true;

            _viewOverlay = new FrameLayout(Container.Context)
            {
                LayoutParameters = new ViewGroup.LayoutParams(-1, -1),
                Clickable = false,
                Focusable = false,
            };
            Container.LayoutChange += ViewOnLayoutChange;

            if (EnableRipple)
                _viewOverlay.Background = CreateRipple(_color);

            SetEffectColor();
            TouchCollector.Add(View, OnTouch);

            Container.AddView(_viewOverlay);
            _viewOverlay.BringToFront();
        }

        void OnTouch(View.TouchEventArgs args)
        {
            switch (args.Event.Action)
            {
                case MotionEventActions.Down:
                    //tapX = (int)args.Event.GetX();
                    //tapY = (int)args.Event.GetY();

                    _timer.Interval = 800f;
                    _timer.AutoReset = false;
                    _timer.Start();

                    if (EnableRipple)
                        ForceStartRipple(args.Event.GetX(), args.Event.GetY());
                    else
                        StartAnimation();
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    if (IsViewInBounds((int)args.Event.RawX, (int)args.Event.RawY))
                    {
                        if (_timer.Enabled)
                        {
                            ClickHandler();
                        }
                        //var range = (DateTime.Now - _tapTime).TotalMilliseconds;
                        //if (range > 800)
                        //    LongClickHandler();
                        //else
                        //    ClickHandler();
                    }

                    _timer.Stop();
                    if (IsDisposed) 
                        return;

                    if (EnableRipple)
                        ForceEndRipple();
                    else
                        TapAnimation(250, _alpha, 0);
                    break;
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
            var cmd = Commands.GetTap(Element);
            var param = Commands.GetTapParameter(Element);
            if (cmd?.CanExecute(param) ?? false)
                cmd.Execute(param);
        }

        void LongClickHandler()
        {
            var cmd = Commands.GetLongTap(Element);
            if (cmd == null)
            {
                ClickHandler();
                return;
            }

            var param = Commands.GetLongTapParameter(Element);
            if (cmd.CanExecute(param))
                cmd.Execute(param);
        }

        protected override void OnDetached()
        {
            if (IsDisposed)
                return;

            Container.RemoveView(_viewOverlay);
            _viewOverlay.Pressed = false;
            _viewOverlay.Foreground = null;
            _viewOverlay.Dispose();
            Container.LayoutChange -= ViewOnLayoutChange;

            if (EnableRipple)
                _ripple?.Dispose();

            TouchCollector.Delete(View, OnTouch);
        }

        #region TouchPart
        private void OnTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Stop();
            Xamarin.Forms.Device.BeginInvokeOnMainThread(LongClickHandler);
        }

        private void DisposeTimer()
        {
            // TODO create dispose timer
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == DataGrid.LongTapColorProperty.PropertyName)
            {
                SetEffectColor();
            }
        }

        private void SetEffectColor()
        {
            var color = Commands.GetColor(Element);
            if (color == Xamarin.Forms.Color.Default)
            {
                return;
            }

            _color = color.ToAndroid();
            _alpha = _color.A == 255 ? (byte)80 : _color.A;

            if (EnableRipple)
            {
                _ripple.SetColor(GetPressedColorSelector(_color));
            }
        }

        private void ViewOnLayoutChange(object sender, View.LayoutChangeEventArgs layoutChangeEventArgs)
        {
            var group = (ViewGroup)sender;
            if (group == null || IsDisposed) 
                return;

            _viewOverlay.Right = group.Width;
            _viewOverlay.Bottom = group.Height;
        }
        #endregion

        #region Ripple
        private RippleDrawable CreateRipple(Color color)
        {
            if (Element is Xamarin.Forms.Layout)
            {
                var mask = new ColorDrawable(Color.White);
                return _ripple = new RippleDrawable(GetPressedColorSelector(color), null, mask);
            }

            var back = View.Background;
            if (back == null)
            {
                var mask = new ColorDrawable(Color.White);
                return _ripple = new RippleDrawable(GetPressedColorSelector(color), null, mask);
            }

            if (back is RippleDrawable)
            {
                _ripple = (RippleDrawable)back.GetConstantState().NewDrawable();
                _ripple.SetColor(GetPressedColorSelector(color));

                return _ripple;
            }

            return _ripple = new RippleDrawable(GetPressedColorSelector(color), back, null);
        }

        private static ColorStateList GetPressedColorSelector(int pressedColor)
        {
            return new ColorStateList(
                new[] { new int[] { } },
                new[] { pressedColor, });
        }

        private void ForceStartRipple(float x, float y)
        {
            if (IsDisposed || !(_viewOverlay.Background is RippleDrawable bc))
                return;

            _viewOverlay.BringToFront();
            bc.SetHotspot(x, y);
            _viewOverlay.Pressed = true;
        }

        private void ForceEndRipple()
        {
            if (IsDisposed) return;

            _viewOverlay.Pressed = false;
        }

        #endregion

        #region Overlay
        private void StartAnimation()
        {
            if (IsDisposed)
                return;

            ClearAnimation();

            _viewOverlay.BringToFront();
            var color = _color;
            color.A = _alpha;
            _viewOverlay.SetBackgroundColor(color);
        }

        private void TapAnimation(long duration, byte startAlpha, byte endAlpha)
        {
            if (IsDisposed)
                return;

            _viewOverlay.BringToFront();

            var start = _color;
            var end = _color;
            start.A = startAlpha;
            end.A = endAlpha;

            ClearAnimation();
            _animator = ObjectAnimator.OfObject(_viewOverlay,
                "BackgroundColor",
                new ArgbEvaluator(),
                start.ToArgb(),
                end.ToArgb());
            _animator.SetDuration(duration);
            _animator.RepeatCount = 0;
            _animator.RepeatMode = ValueAnimatorRepeatMode.Restart;
            _animator.Start();
            _animator.AnimationEnd += AnimationOnAnimationEnd;
        }

        private void AnimationOnAnimationEnd(object sender, EventArgs eventArgs)
        {
            if (IsDisposed) return;

            ClearAnimation();
        }

        private void ClearAnimation()
        {
            if (_animator == null) return;
            _animator.AnimationEnd -= AnimationOnAnimationEnd;
            _animator.Cancel();
            _animator.Dispose();
            _animator = null;
        }
        #endregion
    }
}