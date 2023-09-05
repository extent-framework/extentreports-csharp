using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class Feature : IGherkinFormatterModel
    {
        public static string Name = "Feature";

        public override string ToString()
        {
            return Name;
        }
    }
}
