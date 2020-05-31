using System.Collections.Generic;
using SkiaSharp;

namespace UnleashThePowerOfSkia.Models.PieChartModels
{
    public class WhoThinksIamBeautifulChartSource : BasePieChartSource
    {
        public override string Title => "People who think I am beautiful";
        public override IList<Segment> Segments => new List<Segment>
        {
            new Segment("My mom", 50,SKColors.Aqua),
            new Segment("James Blunt", 50, SKColors.Red),
        };
    }
    
    public class PitbullChartSource : BasePieChartSource
    {
        public override string Title => "Content of Pitbull songs";
        public override IList<Segment> Segments => new List<Segment>
        {
            new Segment("Spanish words", 15, SKColors.Aqua),
            new Segment("Mr. Word Wide", 25, SKColors.Red),
            new Segment("Names of 4 big cities", 15, SKColors.Yellow),
            new Segment("OOOOOOEEEEEEEEIIIIIII", 45, SKColors.Green),
        };
    }
    
    public class FearPhresesChartSource : BasePieChartSource
    {
        public override string Title => "Phrases that cause fear";
        public override IList<Segment> Segments => new List<Segment>
        {
            new Segment("There is a spider!", 20, SKColors.Aqua),
            new Segment("The test came positive...", 20, SKColors.Red),
            new Segment("Can I talk to you?", 60, SKColors.Yellow),
        };
    }
    
    public abstract class BasePieChartSource
    {
        public abstract string Title { get; }
        public abstract IList<Segment> Segments { get; }
    }
}