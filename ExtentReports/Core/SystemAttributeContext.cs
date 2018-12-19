using AventStack.ExtentReports.Model;

using System.Collections.Generic;

namespace AventStack.ExtentReports.Core
{
    public class SystemAttributeContext
    {
        public List<SystemAttribute> SystemAttributeCollection { get; private set; } = new List<SystemAttribute>();

        public void AddSystemAttribute(SystemAttribute attr)
        {
            SystemAttributeCollection.Add(attr);
        }

        public int Count
        {
            get
            {
                return SystemAttributeCollection.Count;
            }
        }

        public void Clear()
        {
            SystemAttributeCollection.Clear();
        }
    }
}
