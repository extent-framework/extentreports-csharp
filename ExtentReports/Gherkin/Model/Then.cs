using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class Then : IGherkinFormatterModel
    {
        public static string Name = "Then";

        public override string ToString()
        {
            return Name;
        }
    }
}
