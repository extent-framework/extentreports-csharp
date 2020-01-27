using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class Given : IGherkinFormatterModel
    {
        public static string GherkinName { get; } = "Given";

        public override string ToString()
        {
            return GherkinName;
        }
    }
}
