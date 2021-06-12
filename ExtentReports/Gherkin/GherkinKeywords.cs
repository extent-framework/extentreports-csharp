using System.Collections.Generic;

namespace AventStack.ExtentReports.Gherkin
{
    public class GherkinKeywords
    {
        public ISet<string> And { get; set; }
        public ISet<string> Background { get; set; }
        public ISet<string> But { get; set; }
        public ISet<string> Examples { get; set; }
        public ISet<string> Feature { get; set; }
        public ISet<string> Given { get; set; }
        public string Name { get; set; }
        public string Native { get; set; }
        public ISet<string> Scenario { get; set; }
        public ISet<string> ScenarioOutline { get; set; }
        public ISet<string> Then { get; set; }
        public ISet<string> When { get; set; }
    }
}
