using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class Then : IGherkinFormatterModel
    {
        public static string GherkinName { get; } = "Then";

        public override string ToString()
        {
            return GherkinName;
        }
    }
}
