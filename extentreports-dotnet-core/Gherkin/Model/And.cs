using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class And : IGherkinFormatterModel
    {
        public static string GherkinName { get; } = "And";

        public override string ToString()
        {
            return GherkinName;
        }
    }
}
