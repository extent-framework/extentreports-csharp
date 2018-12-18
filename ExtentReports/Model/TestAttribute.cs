using System;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public abstract class TestAttribute : Attribute
    {
        public TestAttribute(string name) : base(name) { }
    }
}
