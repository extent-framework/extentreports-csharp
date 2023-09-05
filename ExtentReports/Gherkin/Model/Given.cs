using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class Given : IGherkinFormatterModel
    {
        public static string Name = "Given";

        public override string ToString()
        {
            return Name;
        }
    }
}
