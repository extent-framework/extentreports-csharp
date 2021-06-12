using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class Background : IGherkinFormatterModel
    {
        public static string Name = "Background";

        public override string ToString()
        {
            return Name;
        }
    }
}
