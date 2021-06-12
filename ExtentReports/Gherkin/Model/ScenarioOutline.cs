using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class ScenarioOutline : IGherkinFormatterModel
    {
        public static string Name = "Scenario Outline";

        public override string ToString()
        {
            return Name;
        }
    }
}
