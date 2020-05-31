using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using UnleashThePowerOfSkia.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UnleashThePowerOfSkia.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnalyticsPage : ContentPage
    {
        public AnalyticsPage()
        {
            InitializeComponent();
        }
        
        private readonly Dictionary<long, SKPath> _inProgressPaths = new Dictionary<long, SKPath>();
        private readonly List<SKPath> _completedPaths = new List<SKPath>();

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();
            
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Fuchsia,
                StrokeWidth = 10,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round
            })
            {
                
                foreach (SKPath path in _completedPaths)
                {
                    canvas.DrawPath(path, paint);
                }

                foreach (SKPath path in _inProgressPaths.Values)
                {
                    canvas.DrawPath(path, paint);
                }
            }
        }

        private SKPoint ConvertToPixel(Point pt)
        {
            return new SKPoint((float)(CanvasView.CanvasSize.Width * pt.X / CanvasView.Width),
                               (float)(CanvasView.CanvasSize.Height * pt.Y / CanvasView.Height));
        }
        
        private void TouchEffect_OnTouchAction(object sender, TouchActionEventArgs args)
        {
            if (!CanvasView.IsVisible)
            {
                return;
            }
            
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!_inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = new SKPath();
                        path.MoveTo(ConvertToPixel(args.Location));
                        _inProgressPaths.Add(args.Id, path);
                        CanvasView.InvalidateSurface();
                    }
                    break;
        
                case TouchActionType.Moved:
                    if (_inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = _inProgressPaths[args.Id];
                        path.LineTo(ConvertToPixel(args.Location));
                        CanvasView.InvalidateSurface();
                    }
                    break;
        
                case TouchActionType.Released:
                    if (_inProgressPaths.ContainsKey(args.Id))
                    {
                        _completedPaths.Add(_inProgressPaths[args.Id]);
                        _inProgressPaths.Remove(args.Id);
                        CanvasView.InvalidateSurface();
                    }
                    break;
        
                case TouchActionType.Cancelled:
                    if (_inProgressPaths.ContainsKey(args.Id))
                    {
                        _inProgressPaths.Remove(args.Id);
                        CanvasView.InvalidateSurface();
                    }
                    break;
            }
        }
    }
}