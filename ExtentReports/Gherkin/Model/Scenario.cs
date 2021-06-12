using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class Scenario : IGherkinFormatterModel
    {
        public static string Name = "Scenario";

        public override string ToString()
        {
            return Name;
        }
    }
}
