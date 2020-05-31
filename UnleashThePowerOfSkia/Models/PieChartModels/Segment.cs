using SkiaSharp;

namespace UnleashThePowerOfSkia.Models.PieChartModels
{
    public class Segment
    {
        public string Answer { get; }
        public int Percentage { get; }

        public Segment(string answer, int percentage)
        {
            Answer = answer;
            Percentage = percentage;
        }
    }
}