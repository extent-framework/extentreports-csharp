using System;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class SystemAttribute : Attribute
    {
        public SystemAttribute(string name, string value) : base(name, value)
        {
        }
    }
}
