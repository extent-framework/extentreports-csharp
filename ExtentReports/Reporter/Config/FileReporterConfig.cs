using System.Xml.Serialization;

namespace AventStack.ExtentReports.Reporter.Config
{
    public class FileReporterConfig : AbstractConfiguration
    {
        [XmlElement("encoding")]
        public string Encoding = "utf-8";

        [XmlElement("documentTitle")]
        public string DocumentTitle = "";
    }
}
