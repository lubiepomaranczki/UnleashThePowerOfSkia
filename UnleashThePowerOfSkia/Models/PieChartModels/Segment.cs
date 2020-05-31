using SkiaSharp;

namespace UnleashThePowerOfSkia.Models.PieChartModels
{
    public class Segment
    {
        public string Answer { get; }
        public int Percentage { get; }
        public SKColor Color { get; }

        public Segment(string answer, int percentage, SKColor color)
        {
            Answer = answer;
            Percentage = percentage;
            Color = color;
        }
    }
}