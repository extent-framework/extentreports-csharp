using AventStack.ExtentReports.Model;

using System.Collections.Generic;

namespace AventStack.ExtentReports.Model.Context
{
    public class SystemAttributeContext
    {
        public List<SystemAttribute> SystemAttributeCollection { get; private set; } = new List<SystemAttribute>();

        public void AddSystemAttribute(SystemAttribute attr)
        {
            lock (_synclock)
            {
                SystemAttributeCollection.Add(attr);
            }
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

        private readonly object _synclock = new object();
    }
}
