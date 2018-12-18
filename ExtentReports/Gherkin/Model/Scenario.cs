using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class Scenario : IGherkinFormatterModel
    {
        public static string GherkinName { get; } = "Scenario";

        public override string ToString()
        {
            return GherkinName;
        }
    }
}
