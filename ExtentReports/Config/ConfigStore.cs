using AventStack.ExtentReports.Extensions;
using System;
using System.Collections.Generic;

namespace AventStack.ExtentReports.Config
{
    public class ConfigStore
    {
        public Dictionary<string, object> Store { get; } = new Dictionary<string, object>();

        public bool IsEmpty
        {
            get
            {
                return Store.Count == 0;
            }
        }

        public void AddConfig(string k, string v)
        {
            Store[k] = v;
        }

        public void RemoveConfig(string k)
        {
            Store.Remove(k);
        }

        public bool Contains(string k)
        {
            return Store.ContainsKey(k);
        }

        public object GetConfig(string k)
        {
            return Store[k];
        }

        public void Extend(IDictionary<string, object> dict)
        {
            Store.AddRange(dict);
        }
    }
}