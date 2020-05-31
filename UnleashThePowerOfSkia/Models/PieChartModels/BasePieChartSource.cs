using System.Collections.Generic;
using SkiaSharp;

namespace UnleashThePowerOfSkia.Models.PieChartModels
{
    public class WhoThinksIamBeautifulChartSource : BasePieChartSource
    {
        public override string Title => "People who think I am beautiful";
        public override IList<Segment> Segments => new List<Segment>
        {
            new Segment("My mom", 50),
            new Segment("James Blunt", 50),
        };
    }
    
    public class PitbullChartSource : BasePieChartSource
    {
        public override string Title => "Content of Pitbull songs";
        public override IList<Segment> Segments => new List<Segment>
        {
            new Segment("Spanish words", 15),
            new Segment("Mr. Word Wide", 25),
            new Segment("Names of 4 big cities", 15),
            new Segment("OOOOOOEEEEEEEEIIIIIII", 45),
        };
    }
    
    public class FearPhresesChartSource : BasePieChartSource
    {
        public override string Title => "Phrases that cause fear";
        public override IList<Segment> Segments => new List<Segment>
        {
            new Segment("There is a spider!", 20),
            new Segment("The test came positive...", 20),
            new Segment("Can I talk to you?", 60),
        };
    }
    
    public abstract class BasePieChartSource
    {
        public abstract string Title { get; }
        public abstract IList<Segment> Segments { get; }
    }
}