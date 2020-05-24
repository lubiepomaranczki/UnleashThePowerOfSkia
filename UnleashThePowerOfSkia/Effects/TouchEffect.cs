//taken from https://github.com/xamarin/xamarin-forms-samples/blob/master/SkiaSharpForms/Demos/Demos/SkiaSharpFormsDemos/TouchEffect.cs

using System;
using Xamarin.Forms;

namespace UnleashThePowerOfSkia.Effects
{
    public class TouchEffect : RoutingEffect
    {
        public event TouchActionEventHandler TouchAction;

        public TouchEffect() : base("UnleashThePowerOfSkia.TouchEffect")
        {
        }

        public bool Capture { set; get; }

        public void OnTouchAction(Element element, TouchActionEventArgs args)
        {
            TouchAction?.Invoke(element, args);
        }
    }
    
    public class TouchActionEventArgs : EventArgs
    {
        public TouchActionEventArgs(long id, TouchActionType type, Point location, bool isInContact)
        {
            Id = id;
            Type = type;
            Location = location;
            IsInContact = isInContact;
        }

        public long Id { get; }

        public TouchActionType Type { get; }

        public Point Location { get; }

        public bool IsInContact { get; }
    }
    
    public delegate void TouchActionEventHandler(object sender, TouchActionEventArgs args);
    
    public enum TouchActionType
    {
        Entered,
        Pressed,
        Moved,
        Released,
        Exited,
        Cancelled
    }
}