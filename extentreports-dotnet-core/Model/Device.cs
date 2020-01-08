using System;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class Device : Attribute
    {
        public Device(string name) : base(name) { }

        public Device(string name, string description) : base(name, description) { }
    }
}
