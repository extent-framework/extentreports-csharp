using System;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class Category : Attribute
    {
        public Category(string name) : base(name) { }

        public Category(string name, string description) : base(name, description) { }
    }
}
