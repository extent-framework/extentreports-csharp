using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AventStack.ExtentReports.Reporter.Config
{
    [XmlRoot("configuration")]
    public class ExtentSparkReporterConfig : InteractiveReporterConfig
    {
        private const string OfflinePackage = "extent";
        private const string Package = "AventStack.ExtentReports.Views.Spark.Offline";
        private const string DefaultTimeStampFormat = "MMM dd, yyyy HH:mm:ss a";
        private bool _offline = false;
        private string _timestampFormat = DefaultTimeStampFormat;

        internal ExtentSparkReporter Reporter { get; set; }

        [XmlElement("offlineMode")]
        public bool OfflineMode
        {
            get
            {
                return _offline;
            }
            set
            {
                _offline = value;

                if (value)
                {
                    Task.Run(() => SaveOfflineResources());
                }
            }
        }

        [XmlElement("timeStampFormat")]
        public string TimeStampFormat
        {
            get
            {
                return string.IsNullOrEmpty(_timestampFormat) ? DefaultTimeStampFormat : _timestampFormat;
            }
            set
            {
                _timestampFormat = value;
            }
        }

        public ExtentSparkReporterConfig() { }

        [JsonConstructor]
        public ExtentSparkReporterConfig(ExtentSparkReporter reporter)
        {
            Reporter = reporter;
        }

        internal void SaveOfflineResources()
        {
            if (Reporter == null)
            {
                return;
            }

            string folderPath = Reporter.FolderSavePath;
            folderPath += Path.DirectorySeparatorChar + OfflinePackage + Path.DirectorySeparatorChar;
            Directory.CreateDirectory(folderPath);

            SaveResourceToDisk(Package + ".spark-style.css", folderPath + "spark-style.css");
            SaveResourceToDisk(Package + ".spark-script.js", folderPath + "spark-script.js");
            SaveCommonsResources(folderPath);
        }
    }
}
