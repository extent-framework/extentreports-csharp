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
        private bool _offline = false;

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
