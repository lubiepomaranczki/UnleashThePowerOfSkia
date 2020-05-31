using System;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using UnleashThePowerOfSkia.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamForms.Enhanced.Extensions;

namespace UnleashThePowerOfSkia.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LineChartPage : ContentPage
    {
        private DatesChartSourceViewModel ViewModel => (DatesChartSourceViewModel) BindingContext;

        private int _chartPadding = 40;
        private int _chartWidth;
        private int _chartHeight;

        public LineChartPage()
        {
            InitializeComponent();
            BindingContext = new DatesChartSourceViewModel();
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var canvasWidth = args.Info.Width;
            var canvasHeight = args.Info.Height;

            var surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            canvas.Translate(0, canvasHeight);
            canvas.Translate(_chartPadding, -_chartPadding);
            
            DrawChartAxis(canvas, canvasWidth, canvasHeight);
            DrawData(canvas);
        }

        private void DrawChartAxis(SKCanvas canvas, int canvasWidth, int canvasHeight)
        {
            using (var paint = new SKPaint {IsAntialias = true, Color = SKColors.Black.WithAlpha(Byte.MaxValue/2), Style = SKPaintStyle.Stroke, StrokeWidth = 2f})
            {
                _chartWidth = canvasWidth - 2 * _chartPadding;
                _chartHeight = -canvasHeight + 2 * _chartPadding;

                canvas.DrawLine(new SKPoint(0, 0), new SKPoint(_chartWidth, 0), paint);
                canvas.DrawLine(new SKPoint(0, -0), new SKPoint(0, _chartHeight), paint);
                
                DrawAxisArrows();
            }

            void DrawAxisArrows()
            {
                int arrowWidth=24;
                int arrowHeight = 24;

                using (var paint = new SKPaint {IsAntialias = true, Color = SKColors.Black.WithAlpha(Byte.MaxValue/2)})
                {
                    using (var path = new SKPath())
                    {
                        path.MoveTo(-arrowWidth / 2, _chartHeight);
                        path.LineTo(0, _chartHeight - arrowHeight);
                        path.LineTo(arrowWidth / 2, _chartHeight);

                        canvas.DrawPath(path, paint);
                    }
                    using (var path = new SKPath())
                    {
                        path.MoveTo(_chartWidth, -arrowWidth/2);
                        path.LineTo(_chartWidth+arrowHeight, 0);
                        path.LineTo(_chartWidth, arrowWidth/2);
                    
                        canvas.DrawPath(path, paint);
                    }
                }
            }
        }

        private void DrawData(SKCanvas canvas)
        {
            if (ViewModel.AverageSales.IsNullOrEmpty())
            {
                return;
            }

            var maxValue = ViewModel.AverageSales.Max(s => s.Value);
            var amountOfDays = ViewModel.AverageSales.Count;

            var dataPadding = 20;

            var valueScaleFactor = (_chartHeight+dataPadding) / maxValue;
            var dayScaleFactor = (_chartWidth-dataPadding) / amountOfDays;
            
            using (var path = new SKPath())
            using (var paint = new SKPaint{Color = SKColors.Magenta, IsAntialias = true, Style = SKPaintStyle.Stroke, StrokeWidth = 4f})
            {
                var lastDayValue = 0;
                foreach (var averageSales in ViewModel.AverageSales)
                {
                    var averageSalesRelative = averageSales.Value * valueScaleFactor;
                    if (path.IsEmpty)
                    {
                        path.MoveTo(new SKPoint(lastDayValue,averageSalesRelative));
                    }
                    else
                    {
                        path.LineTo(lastDayValue, averageSalesRelative);
                    }

                    
                    using (var labelPaint = new SKPaint{Color = SKColors.Black, IsAntialias = true, TextSize = 20f})
                    {
                        canvas.DrawCircle(new SKPoint(0,averageSalesRelative ),4, labelPaint);
                        canvas.DrawCircle(new SKPoint(lastDayValue,0 ),4, labelPaint);

                        var shouldDrawLabel = true;
                        
                        if (ViewModel.AverageSales.Count > 30 && ViewModel.AverageSales.IndexOf(averageSales)%2!=0)
                        {
                            shouldDrawLabel = false;
                        }
                        
                        if(shouldDrawLabel)
                        {
                            canvas.DrawText(averageSales.Value.ToString(), new SKPoint(-_chartPadding, averageSalesRelative), labelPaint);
                            canvas.DrawText(averageSales.Day.ToString("dd-MM"), new SKPoint(lastDayValue, _chartPadding), labelPaint);
                        }
                    }

                    lastDayValue += dayScaleFactor;
                }
                canvas.DrawPath(path, paint);
            }
        }
    }
}