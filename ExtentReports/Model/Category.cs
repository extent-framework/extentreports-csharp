using System;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class Category : TestAttribute
    {
        public Category(string name) : base(name)
        { }
    }
}
