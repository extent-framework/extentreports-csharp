using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class But : IGherkinFormatterModel
    {
        public static string GherkinName { get; } = "But";

        public override string ToString()
        {
            return GherkinName;
        }
    }
}
