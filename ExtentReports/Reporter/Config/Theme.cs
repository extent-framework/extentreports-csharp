using System.Xml.Serialization;

namespace AventStack.ExtentReports.Reporter.Config
{
    public enum Theme
    {
        [XmlEnum(Name = "STANDARD")]
        Standard,
        [XmlEnum(Name = "DARK")]
        Dark
    }
}
