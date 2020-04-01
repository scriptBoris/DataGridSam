using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using DataGridSam.iOS.Utils;
using DataGridSam.Platform;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("DataGridSam")]
[assembly: ExportEffect(typeof(DataGridSam.iOS.TouchIOS), nameof(DataGridSam.Platform.Touch))]
namespace DataGridSam.iOS
{
    public class TouchIOS : PlatformEffect
    {
        public bool IsDisposed => (Container as IVisualElementRenderer)?.Element == null;
        public UIView View => Control ?? Container;

        private UIView _layer;
        private nfloat _alpha;

        private CancellationTokenSource _cancellation;
        private bool isTaped;

        private System.Timers.Timer timer;
        private UILongPressGestureRecognizer gestureTap;
        private bool isEnabled;

        public static void Preserve() { }

        protected override void OnAttached()
        {
            View.UserInteractionEnabled = true;
            _layer = new UIView
            {
                UserInteractionEnabled = false,
                Opaque = false,
                Alpha = 0,
                TranslatesAutoresizingMaskIntoConstraints = false,
            };

            isEnabled = Touch.GetIsEnabled(Element);
            //var gest = new TouchGestureRecognizer
            //{
            //    CancelsTouchesInView = false,
            //    Delegate = new TouchGestureRecognizerDelegate(View),
            //};

            gestureTap = new UILongPressGestureRecognizer(OnTap);
            gestureTap.MinimumPressDuration = 0;
            gestureTap.Delegate = new TouchGestureRecognizerDelegate(View);

            TimerInit();
            UpdateEffectColor();

            View.AddGestureRecognizer(gestureTap);
            View.AddSubview(_layer);
            View.BringSubviewToFront(_layer);

            _layer.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            _layer.LeftAnchor.ConstraintEqualTo(View.LeftAnchor).Active = true;
            _layer.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
            _layer.RightAnchor.ConstraintEqualTo(View.RightAnchor).Active = true;
        }

        protected override void OnDetached()
        {
            View.RemoveGestureRecognizer(gestureTap);
            gestureTap.Dispose();
            _layer?.Dispose();
            _layer = null;
            TimerDispose();
        }

        private void TimerInit()
        {
            if (timer == null)
            {
                timer = new System.Timers.Timer();
                timer.Elapsed += OnTimerElapsed;
                timer.AutoReset = false;
            }
        }

        private void TimerDispose()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Elapsed -= OnTimerElapsed;
                timer.Dispose();
                timer = null;
            }
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(e);

            if (e.PropertyName == Touch.ColorProperty.PropertyName)
            {
                UpdateEffectColor();
            }
            else if (e.PropertyName == Touch.IsEnabledProperty.PropertyName)
            {
                isEnabled = Touch.GetIsEnabled(Element);
            }
        }

        private void OnTap(UILongPressGestureRecognizer press)
        {
            var coordinate = press.LocationInView(press.View);
            bool isInside = press.View.PointInside(coordinate, null);

            switch (press.State)
            {
                case UIGestureRecognizerState.Began:
                    if (isEnabled)
                    {
                        isTaped = true;
                        if (timer != null)
                        {
                            timer.Interval = Touch.GetLongTapLatency(Element);
                            timer.Start();
                        }
                        AnimRun();
                        //TapAnimation(0.3, 0, _alpha, false);
                    }
                    break;
                case UIGestureRecognizerState.Changed:
                    if (!isInside && isTaped)
                    {
                        isTaped = false;
                        AnimStop();
                        //TapAnimation(0.3, _alpha);
                    }
                    break;
                case UIGestureRecognizerState.Ended:
                    if (isInside && isTaped && isEnabled)
                    {
                        if (Touch.GetLongTap(Element) == null)
                        {
                            SelectHandler();
                            ClickHandler();
                        }
                        else
                        {
                            if (timer.Enabled)
                            {
                                SelectHandler();
                                ClickHandler();
                                timer.Stop();
                            }
                        }
                        AnimStop();
                        //TapAnimation(0.3, _alpha);
                    }
                    isTaped = false;
                    break;
                case UIGestureRecognizerState.Cancelled:
                case UIGestureRecognizerState.Failed:
                    if (isTaped)
                    {
                        isTaped = false;
                        AnimStop();
                        //TapAnimation(0.3, _alpha);
                    }
                    break;
            }
        }

        private void OnTimerElapsed(object o, System.Timers.ElapsedEventArgs e)
        {
            if (isTaped)
            {
                isTaped = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    LongTapExecute();
                    AnimStop();
                    //TapAnimation(0.3, _alpha);
                });
            }
        }


        #region Click handlers
        private void LongTapExecute()
        {
            SelectHandler();

            var cmdLong = Touch.GetLongTap(Element);
            if (cmdLong == null)
            {
                ClickHandler();
                return;
            }

            if (cmdLong.CanExecute(Element.BindingContext))
                cmdLong.Execute(Element.BindingContext);
        }

        private void SelectHandler()
        {
            var cmd = Touch.GetSelect(Element);
            cmd.Execute(Element.BindingContext);
        }

        private void ClickHandler()
        {
            var cmd = Touch.GetTap(Element);
            if (cmd == null)
                return;

            if (cmd.CanExecute(Element.BindingContext))
                cmd.Execute(Element.BindingContext);
        }
        #endregion Click handlers

        #region TouchEffects
        private void UpdateEffectColor()
        {
            var color = Touch.GetColor(Element);
            if (color == Color.Default)
            {
                return;
            }

            _alpha = color.A < 1.0 ? 1 : (nfloat)0.3;
            _layer.BackgroundColor = color.ToUIColor();
        }

        private void AnimRun()
        {
            _layer.Layer.RemoveAllAnimations();
            _layer.Alpha = _alpha;
            View.BringSubviewToFront(_layer);
        }

        private void AnimStop()
        {
            if (!IsDisposed && _layer != null)
            {
                _layer.Layer.RemoveAllAnimations();
                UIView.Animate(0.225,
                () => {
                    _layer.Alpha = 0;
                });
            }
        }

        private async void TapAnimation(double duration, double start = 1, double end = 0, bool remove = true)
        {
            if (!IsDisposed && _layer != null)
            {
                _cancellation?.Cancel();
                _cancellation = new CancellationTokenSource();

                var token = _cancellation.Token;

                _layer.Layer.RemoveAllAnimations();
                _layer.Alpha = (float)start;
                //View.BringSubviewToFront(_layer);
                //UpdateEffectColor();

                await UIView.AnimateAsync(duration, () => {
                    if (!token.IsCancellationRequested && !IsDisposed)
                        _layer.Alpha = (float)end;
                });
                
                //if (remove && !IsDisposed && !token.IsCancellationRequested)
                //{
                //    _layer?.RemoveFromSuperview();
                //}
            }
        }
        #endregion TouchEffects
    }
}