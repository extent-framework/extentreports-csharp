using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class ScenarioOutline : IGherkinFormatterModel
    {
        public static string GherkinName { get; } = "Scenario Outline";

        public override string ToString()
        {
            return GherkinName;
        }
    }
}
