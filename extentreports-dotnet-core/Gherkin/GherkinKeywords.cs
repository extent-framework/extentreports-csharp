using System;
using System.Collections.Generic;

namespace AventStack.ExtentReports.Gherkin
{
    internal class GherkinKeywords
    {
        public List<string> And { get; set; }
        public List<string> Background { get; set; }
        public List<string> But { get; set; }
        public List<string> Examples { get; set; }
        public List<string> Feature { get; set; }
        public List<string> Given { get; set; }
        public string Name { get; set; }
        public string Native { get; set; }
        public List<string> Scenario { get; set; }
        public List<string> ScenarioOutline { get; set; }
        public List<string> Then { get; set; }
        public List<string> When { get; set; }
    }
}
