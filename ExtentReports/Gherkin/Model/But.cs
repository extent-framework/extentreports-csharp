using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class But : IGherkinFormatterModel
    {
        public static string Name = "But";

        public override string ToString()
        {
            return Name;
        }
    }
}
