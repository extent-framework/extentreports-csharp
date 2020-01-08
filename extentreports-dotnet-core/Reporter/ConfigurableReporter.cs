using AventStack.ExtentReports.Configuration;

using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AventStack.ExtentReports.Reporter
{
    public abstract class ConfigurableReporter : AbstractReporter
    {
        public ConfigurationManager MasterConfig { get; protected internal set; } = new ConfigurationManager();

        public void LoadConfig(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The file " + filePath + " was not found.");

            var xdoc = XDocument.Load(filePath, LoadOptions.None);
            if (xdoc == null)
            {
                throw new FileLoadException("Unable to configure report with the supplied configuration. Please check the input file and try again.");
            }

            LoadConfigFileContents(xdoc);
        }

        private void LoadConfigFileContents(XDocument xdoc)
        {
            foreach (var xe in xdoc.Descendants("configuration").First().Elements())
            {
                var key = xe.Name.ToString();
                var value = xe.Value;

                var c = new Config(key, value);
                MasterConfig.AddConfig(c);
            }
        }
    }
}
