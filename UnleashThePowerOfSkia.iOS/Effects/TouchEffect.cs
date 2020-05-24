//taken from https://github.com/xamarin/xamarin-forms-samples/blob/master/SkiaSharpForms/Demos/Demos/SkiaSharpFormsDemos.iOS/TouchEffect.cs

using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Shared= UnleashThePowerOfSkia.Effects;
using UIKit;
using UnleashThePowerOfSkia.iOS.Effects;

[assembly: ResolutionGroupName("UnleashThePowerOfSkia")]
[assembly: ExportEffect(typeof(TouchEffect), "TouchEffect")]

namespace UnleashThePowerOfSkia.iOS.Effects
{
    public class TouchEffect : PlatformEffect
    {
        UIView view;
        TouchRecognizer touchRecognizer;

        protected override void OnAttached()
        {
            // Get the iOS UIView corresponding to the Element that the effect is attached to
            view = Control == null ? Container : Control;

            // Get access to the TouchEffect class in the .NET Standard library
            Shared.TouchEffect effect = (Shared.TouchEffect)Element.Effects.FirstOrDefault(e => e is Shared.TouchEffect);

            if (effect != null && view != null)
            {
                // Create a TouchRecognizer for this UIView
                touchRecognizer = new TouchRecognizer(Element, view, effect); 
                view.AddGestureRecognizer(touchRecognizer);
            }
        }

        protected override void OnDetached()
        {
            if (touchRecognizer != null)
            {
                // Clean up the TouchRecognizer object
                touchRecognizer.Detach();

                // Remove the TouchRecognizer from the UIView
                view.RemoveGestureRecognizer(touchRecognizer);
            }
        }
    }
}