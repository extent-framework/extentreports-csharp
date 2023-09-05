using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class And : IGherkinFormatterModel
    {
        public const string Name = "And";

        public override string ToString()
        {
            return Name;
        }
    }
}
