//taken from: https://github.com/xamarin/xamarin-forms-samples/blob/master/SkiaSharpForms/Demos/Demos/SkiaSharpFormsDemos.iOS/TouchRecognizer.cs
using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;
using UnleashThePowerOfSkia.Effects;
using Xamarin.Forms;

namespace UnleashThePowerOfSkia.iOS.Effects
{
    class TouchRecognizer : UIGestureRecognizer
    {
        private Element _element; // Forms element for firing events
        private UIView _view; // iOS UIView 
        private UnleashThePowerOfSkia.Effects.TouchEffect _touchEffect;
        private bool _capture;

        private static readonly Dictionary<UIView, TouchRecognizer> ViewDictionary = new Dictionary<UIView, TouchRecognizer>();

        private static readonly Dictionary<long, TouchRecognizer> IdToTouchDictionary = new Dictionary<long, TouchRecognizer>();

        public TouchRecognizer(Element element, UIView view, UnleashThePowerOfSkia.Effects.TouchEffect touchEffect)
        {
            _element = element;
            _view = view;
            _touchEffect = touchEffect;
        }


        public void Attach()
        {
            ViewDictionary.Add(_view, this);
        }

        public void Detach()
        {
            ViewDictionary.Remove(_view);
        }

        // touches = touches of interest; evt = all touches of type UITouch
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();
                FireEvent(this, id, TouchActionType.Pressed, touch, true);

                if (!IdToTouchDictionary.ContainsKey(id))
                {
                    IdToTouchDictionary.Add(id, this);
                }
            }

            // Save the setting of the Capture property
            _capture = _touchEffect.Capture;
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            if (!Enabled)
            {
                return;
            }

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();

                if (_capture)
                {
                    FireEvent(this, id, TouchActionType.Moved, touch, true);
                }
                else
                {
                    CheckForBoundaryHop(touch);

                    if (IdToTouchDictionary[id] != null)
                    {
                        FireEvent(IdToTouchDictionary[id], id, TouchActionType.Moved, touch, true);
                    }
                }
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();

                if (_capture)
                {
                    FireEvent(this, id, TouchActionType.Released, touch, false);
                }
                else
                {
                    CheckForBoundaryHop(touch);

                    if (IdToTouchDictionary[id] != null)
                    {
                        FireEvent(IdToTouchDictionary[id], id, TouchActionType.Released, touch, false);
                    }
                }

                IdToTouchDictionary.Remove(id);
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                long id = touch.Handle.ToInt64();

                if (_capture)
                {
                    FireEvent(this, id, TouchActionType.Cancelled, touch, false);
                }
                else if (IdToTouchDictionary[id] != null)
                {
                    FireEvent(IdToTouchDictionary[id], id, TouchActionType.Cancelled, touch, false);
                }

                IdToTouchDictionary.Remove(id);
            }
        }

        void CheckForBoundaryHop(UITouch touch)
        {
            long id = touch.Handle.ToInt64();

            // TODO: Might require converting to a List for multiple hits
            TouchRecognizer recognizerHit = null;

            foreach (UIView view in ViewDictionary.Keys)
            {
                CGPoint location = touch.LocationInView(view);

                if (new CGRect(new CGPoint(), view.Frame.Size).Contains(location))
                {
                    recognizerHit = ViewDictionary[view];
                }
            }

            if (recognizerHit != IdToTouchDictionary[id])
            {
                if (IdToTouchDictionary[id] != null)
                {
                    FireEvent(IdToTouchDictionary[id], id, TouchActionType.Exited, touch, true);
                }

                if (recognizerHit != null)
                {
                    FireEvent(recognizerHit, id, TouchActionType.Entered, touch, true);
                }

                IdToTouchDictionary[id] = recognizerHit;
            }
        }

        void FireEvent(TouchRecognizer recognizer, long id, TouchActionType actionType, UITouch touch, bool isInContact)
        {
            CGPoint cgPoint = touch.LocationInView(recognizer.View);
            Point xfPoint = new Point(cgPoint.X, cgPoint.Y);

            Action<Element, TouchActionEventArgs> onTouchAction = recognizer._touchEffect.OnTouchAction;

            onTouchAction(recognizer._element, new TouchActionEventArgs(id, actionType, xfPoint, isInContact));
        }
    }
}