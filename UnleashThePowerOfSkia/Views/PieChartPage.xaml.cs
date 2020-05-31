using System;
using System.ComponentModel;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using UnleashThePowerOfSkia.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UnleashThePowerOfSkia.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PieChartPage : ContentPage
    {
        private PieChartSourceViewModel ViewModel => (PieChartSourceViewModel) BindingContext;

        public PieChartPage()
        {
            InitializeComponent();
            BindingContext = new PieChartSourceViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.ShouldExplode))
            {
                CanvasView.InvalidateSurface();
            }
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var canvasWidth = args.Info.Width;
            var canvasHeight = args.Info.Height;

            var surface = args.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();
            canvas.Translate(canvasWidth / 2, canvasHeight / 2);

            var chartRadius = canvasWidth > canvasHeight ? canvasHeight : canvasWidth;

            DrawChart(chartRadius * 0.8f * 0.5f, canvas);
        }

        private void DrawChart(float chartRadius, SKCanvas canvas)
        {
            var percantageScaleFactor = 360 / 100f;
            float startAngle = 0;
            float explodeOffset = 50;

            //Nelson gradient. Taken from https://uigradients.com/#Nelson
            SKColor[] gradientColors =
            {
                SKColor.Parse("f2709c"),
                SKColor.Parse("ff9472")
            };

            foreach (var segment in ViewModel.PieChartSource.Segments)
            {
                var sweepAngle = segment.Percentage * percantageScaleFactor;
                var center = new SKPoint(0, 0);
                SKRect rect = new SKRect(center.X - chartRadius, center.Y - chartRadius, center.X + chartRadius, center.Y + chartRadius);

                using (SKPath path = new SKPath())
                using (SKPaint fillPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Shader = SKShader.CreateRadialGradient(center, chartRadius, gradientColors, SKShaderTileMode.Clamp),
                    ImageFilter = SKImageFilter.CreateDropShadow(8, 8, 4, 4, SKColor.Parse("#A6808080"), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground)
                })
                using (var textPaint = new SKPaint
                {
                    TextSize = 52,
                    Color = SKColors.White,
                    ImageFilter = SKImageFilter.CreateDropShadow(8, 8, 4, 4, SKColor.Parse("#A6808080"), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground),
                    TextAlign = SKTextAlign.Center,
                })
                {
                    path.MoveTo(center);
                    path.ArcTo(rect, startAngle, sweepAngle, false);
                    path.Close();

                    float angle = startAngle + 0.5f * sweepAngle;
                    float x = explodeOffset * (float) Math.Cos(Math.PI * angle / 180);
                    float y = explodeOffset * (float) Math.Sin(Math.PI * angle / 180);

                    canvas.Save();
                    if (ViewModel.ShouldExplode)
                    {
                        canvas.Translate(x, y);
                    }

                    canvas.DrawPath(path, fillPaint);

                    canvas.DrawText($"{segment.Percentage.ToString()} %", new SKPoint(path.TightBounds.MidX, path.TightBounds.MidY - 20), textPaint);
                    if(ViewModel.ShouldExplode)
                    {
                        textPaint.TextSize = 30;
                        canvas.DrawText($"{segment.Answer}", new SKPoint(path.TightBounds.MidX, path.TightBounds.MidY + 20), textPaint);
                    }
                    
                    canvas.Restore();
                }

                startAngle += sweepAngle;
            }
        }

        private void DrawGraphShadow(float chartRadius, SKCanvas canvas)
        {
            using (var paint = new SKPaint
            {
                Color = SKColor.Empty,
                ImageFilter = SKImageFilter.CreateDropShadow(8, 8, 4, 4, SKColor.Parse("#A6808080"), SKDropShadowImageFilterShadowMode.DrawShadowAndForeground)
            })
            {
                canvas.DrawCircle(0, 0, chartRadius, paint);
            }
        }
    }
}