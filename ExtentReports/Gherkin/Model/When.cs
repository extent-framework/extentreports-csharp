using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class When : IGherkinFormatterModel
    {
        public static string GherkinName { get; } = "When";

        public override string ToString()
        {
            return GherkinName;
        }
    }
}
