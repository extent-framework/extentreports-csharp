using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class Feature : IGherkinFormatterModel
    {
        public static string GherkinName { get; } = "Feature";

        public override string ToString()
        {
            return GherkinName;
        }
    }
}
