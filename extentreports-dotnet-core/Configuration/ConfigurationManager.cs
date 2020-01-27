using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports.Configuration
{
    public class ConfigurationManager
    {
        public List<Config> Configuration { get; internal set; } = new List<Config>();
        
        public string GetValue(string k)
        {
            var c = Configuration.Where(x => x.Key.Equals(k));

            if (c.Count() > 0)
                return c.First().Value;

            return null;
        }

        public void AddConfig(Config c)
        {
            if (ContainsConfig(c.Key))
                RemoveConfig(c.Key);

            Configuration.Add(c);
        }

        private bool ContainsConfig(string k)
        {
            return Configuration.Where(x => x.Key.Equals(k)).Count() == 1;
        }

        private void RemoveConfig(string k)
        {
            var c = Configuration.Where(x => x.Key.Equals(k)).First();
            Configuration.Remove(c);
        }
    }
}
