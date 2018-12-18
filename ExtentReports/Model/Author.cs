using System;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class Author : TestAttribute
    {
        public Author(string name) : base(name)
        { }
    }
}
