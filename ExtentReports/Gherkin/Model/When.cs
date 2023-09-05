using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class When : IGherkinFormatterModel
    {
        public static string Name = "When";

        public override string ToString()
        {
            return Name;
        }
    }
}
