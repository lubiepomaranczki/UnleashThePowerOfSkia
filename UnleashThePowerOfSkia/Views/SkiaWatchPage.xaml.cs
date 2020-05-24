using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UnleashThePowerOfSkia.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SkiaWatchPage : ContentPage
    {
        private const int Frames = 60;

        private bool _needsUpdate;

        public SkiaWatchPage()
        {
            InitializeComponent();

            _needsUpdate = true;
            Device.StartTimer(TimeSpan.FromSeconds(1f / Frames), () =>
            {
                CanvasView.InvalidateSurface();
                return _needsUpdate;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _needsUpdate = false;
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var canvasWidth = args.Info.Width;
            var canvasHeight = args.Info.Height;

            var surface = args.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();
            canvas.Translate(canvasWidth / 2, canvasHeight / 2);

            var now = DateTime.Now;

            var watchDim = GetWatchDimensions(canvasHeight, canvasWidth);
            DrawClockWatch(canvas, watchDim);
            DrawIndicators(canvas, watchDim);
            DrawHourHand(canvas, watchDim, now);
            DrawMinuteHand(canvas, watchDim, now);
            DrawSecondHand(canvas, watchDim, now);
            DrawCenter(canvas, watchDim.CanvasCenter);
        }

        private void DrawIndicators(SKCanvas canvas, WatchDimensions watchDim)
        {
            const float fontSize = 52f;
            const float magicNumberToCenterText = -6;

            using (var skPaint = new SKPaint
            {
                IsAntialias = true,
                Color = SKColor.Parse("#808080"),
                TextSize = fontSize,
                TextAlign = SKTextAlign.Center
            })
            {
                canvas.DrawText("12", new SKPoint(watchDim.CanvasCenter.X, -watchDim.ClockInnerRadius + 60f), skPaint);
                canvas.DrawText("3", new SKPoint(watchDim.ClockInnerRadius - 60f, watchDim.CanvasCenter.Y + fontSize / 2 + magicNumberToCenterText), skPaint);
                canvas.DrawText("6", new SKPoint(watchDim.CanvasCenter.X, watchDim.ClockInnerRadius - 60f), skPaint);
                canvas.DrawText("9", new SKPoint(-watchDim.ClockInnerRadius + 60f, watchDim.CanvasCenter.Y + fontSize / 2 + magicNumberToCenterText), skPaint);
            }
        }

        private void DrawHourHand(SKCanvas canvas, WatchDimensions watchDim, DateTime now)
        {
            using (var skPaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColor.Parse("#808080"),
                StrokeCap = SKStrokeCap.Round,
                StrokeWidth = 16f
            })
            {
                canvas.Save();
                canvas.RotateDegrees(30 * now.Hour + now.Minute / 2f);
                canvas.DrawLine(watchDim.CanvasCenter, new SKPoint(watchDim.CanvasCenter.X, -watchDim.ClockInnerRadius / 2), skPaint);
                canvas.Restore();
            }
        }

        private void DrawMinuteHand(SKCanvas canvas, WatchDimensions watchDim, DateTime now)
        {
            using (var skPaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColor.Parse("#808080"),
                StrokeCap = SKStrokeCap.Round,
                StrokeWidth = 12f
            })
            {
                canvas.Save();
                canvas.RotateDegrees(6 * now.Minute + now.Second / 10f);
                canvas.DrawLine(watchDim.CanvasCenter, new SKPoint(watchDim.CanvasCenter.X, -watchDim.ClockInnerRadius * 0.75f), skPaint);
                canvas.Restore();
            }
        }

        private void DrawSecondHand(SKCanvas canvas, WatchDimensions watchDim, DateTime now)
        {
            using (var skPaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = SKColor.Parse("#e9b414"),
                StrokeCap = SKStrokeCap.Round,
                StrokeWidth = 4f
            })
            {
                canvas.Save();
                canvas.RotateDegrees(6 * (now.Second + now.Millisecond / 1000f));
                canvas.DrawLine(watchDim.CanvasCenter, new SKPoint(watchDim.CanvasCenter.X, -watchDim.ClockInnerRadius * 0.9f), skPaint);
                skPaint.StrokeWidth = 16f;
                canvas.DrawLine(watchDim.CanvasCenter, new SKPoint(watchDim.CanvasCenter.X, 20), skPaint);
                canvas.Restore();
            }
        }

        private void DrawCenter(SKCanvas canvas, SKPoint canvasCenter)
        {
            using (var skPaint = new SKPaint
            {
                IsAntialias = true,
                Color = SKColor.Parse("#e9b414")
            })
            {
                canvas.DrawCircle(canvasCenter, 16, skPaint);
            }
        }

        private void DrawClockWatch(SKCanvas canvas, WatchDimensions watchDimensions)
        {
            using (var skPaint = new SKPaint
            {
                IsAntialias = true,
                Color = SKColor.Parse("#808080"),
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = 2,
                ImageFilter = SKImageFilter.CreateDropShadow(8, 8, 4, 4, SKColor.Parse("#A6808080"), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground, null, null)
            })
            {
                canvas.DrawCircle(watchDimensions.CanvasCenter, watchDimensions.ClockRadius, skPaint);
                skPaint.ImageFilter = null;
                skPaint.Color = SKColor.Parse("#424242");
                skPaint.Style = SKPaintStyle.Fill;
                canvas.DrawCircle(watchDimensions.CanvasCenter, watchDimensions.ClockInnerRadius, skPaint);
            }
        }

        private WatchDimensions GetWatchDimensions(int canvasHeight, int canvasWidth)
        {
            const int clockWatchWidth = 40;
            const int clockMargin = 40;

            int clockRadius;

            if (canvasHeight < canvasWidth)
            {
                clockRadius = canvasHeight / 2 - clockMargin;
            }
            else
            {
                clockRadius = canvasWidth / 2 - clockMargin;
            }

            var clockInnerRadius = clockRadius - clockWatchWidth;


            var canvasCenter = new SKPoint(0, 0);

            return new WatchDimensions(clockRadius, clockInnerRadius, canvasCenter);
        }

        private struct WatchDimensions
        {
            public int ClockRadius { get; }
            public int ClockInnerRadius { get; }
            public SKPoint CanvasCenter { get; }

            public WatchDimensions(int clockRadius, int clockInnerRadius, SKPoint canvasCenter)
            {
                ClockRadius = clockRadius;
                ClockInnerRadius = clockInnerRadius;
                CanvasCenter = canvasCenter;
            }
        }
    }
}