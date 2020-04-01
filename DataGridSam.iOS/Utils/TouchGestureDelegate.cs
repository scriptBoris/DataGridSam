using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreFoundation;
using Foundation;
using UIKit;

namespace DataGridSam.iOS.Utils
{
    public class TouchGestureDelegate : UIGestureRecognizerDelegate
    {
        readonly UIView _view;

        public TouchGestureDelegate(UIView view)
        {
            _view = view;
        }

        public override bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer,
            UIGestureRecognizer otherGestureRecognizer)
        {
            //if (gestureRecognizer is TouchGestureRecognizer rec && otherGestureRecognizer is UIPanGestureRecognizer &&
            //    otherGestureRecognizer.State == UIGestureRecognizerState.Began)
            //{
            //    rec.TryEndOrFail();
            //}

            return true;
        }

        public override bool ShouldReceiveTouch(UIGestureRecognizer recognizer, UITouch touch)
        {
            //if (recognizer is TouchGestureRecognizer && TouchGestureRecognizer.IsActive)
            //{
            //    return false;
            //}

            return touch.View == _view;
        }
    }
}