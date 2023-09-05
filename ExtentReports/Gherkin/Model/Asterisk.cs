using System;

namespace AventStack.ExtentReports.Gherkin.Model
{
    [Serializable]
    public class Asterisk : IGherkinFormatterModel
    {
        public const string Name = "*";

        public override string ToString()
        {
            return Name;
        }
    }
}
