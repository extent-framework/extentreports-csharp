using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class Asterisk : IGherkinFormatterModel
    {
        public static string GherkinName { get; } = "*";

        public override string ToString()
        {
            return GherkinName;
        }
    }
}
