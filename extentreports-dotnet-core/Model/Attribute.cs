using System;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public abstract class Attribute
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public Attribute(string name, string description)
        {
            Name = name;
            Value = description;
        }

        public Attribute(string name) : this(name, null) { }
    }
}
