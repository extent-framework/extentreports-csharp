using System;
using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports.Tests.Markup
{
    class Foo
    {
        public List<string> Names { get; set; } = new string[] { "Anshoo", "Extent", "Klov" }.ToList();
        public object[] Stack { get; set; } = new object[] { "Java", "C#", "Angular" };
    }
}
