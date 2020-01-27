using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class Background : IGherkinFormatterModel
    {
        public static string GherkinName { get; } = "Background";

        public override string ToString()
        {
            return GherkinName;
        }
    }
}
