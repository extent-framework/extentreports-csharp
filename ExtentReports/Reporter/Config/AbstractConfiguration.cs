using AventStack.ExtentReports.Config;
using System.Xml.Serialization;

namespace AventStack.ExtentReports.Reporter.Config
{
    public class AbstractConfiguration
    {
        protected ConfigStore Store = new ConfigStore();

        [XmlElement("reportName")]
        public string ReportName = "";
    }
}
