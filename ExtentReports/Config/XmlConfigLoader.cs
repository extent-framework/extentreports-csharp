using System.IO;
using System.Xml.Serialization;

namespace AventStack.ExtentReports.Config
{
    internal class XmlConfigLoader
    {
        public void LoadXMLConfig<T>(ref T config, string filePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            var textReader = new StreamReader(filePath);
            config = (T)serializer.Deserialize(textReader);
            textReader.Close();
        }
    }
}
