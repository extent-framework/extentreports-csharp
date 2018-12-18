using System;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class Device : TestAttribute
    {
        public Device(string name) : base(name)
        { }
    }
}
