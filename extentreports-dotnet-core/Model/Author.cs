using System;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class Author : Attribute
    {
        public Author(string name) : base(name) { }

        public Author(string name, string description) : base(name, description) { }
    }
}
