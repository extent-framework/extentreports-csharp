using System.IO;
using System.Xml.Serialization;

namespace AventStack.ExtentReports.Reporter.Config
{
    public class InteractiveReporterConfig : FileReporterConfig
    {
        private readonly string OfflinePackage = "AventStack.ExtentReports.Views.Commons.Offline";

        [XmlElement("protocol")]
        public Protocol Protocol = Protocol.HTTP;

        [XmlElement("theme")]
        public Theme Theme = Theme.Standard;

        [XmlElement("timelineEnabled")]
        public bool TimelineEnabled = true;

        [XmlElement("styles")]
        public string CSS = "";

        [XmlElement("scripts")]
        public string JS = "";

        protected void SaveResourceToDisk(string resourceStreamPath, string savePath)
        {
            var stream = this.GetType().Assembly.GetManifestResourceStream(resourceStreamPath);
            using (var sr = new StreamReader(stream))
            {
                var text = sr.ReadToEnd();
                File.WriteAllText(savePath, text);
            }
        }

        protected void SaveBinaryResourceToDisk(string resourceStreamPath, string savePath)
        {
            using (var sourceFileStream = this.GetType().Assembly.GetManifestResourceStream(resourceStreamPath))
            {
                using (var destinationFileStream = new FileStream(savePath, FileMode.OpenOrCreate))
                {
                    while (sourceFileStream.Position < sourceFileStream.Length)
                    {
                        destinationFileStream.WriteByte((byte)sourceFileStream.ReadByte());
                    }
                }
            }
        }

        protected void SaveCommonsResources(string folderPath, bool saveFontAwesomeFonts = true)
        {
            SaveResourceToDisk(OfflinePackage + ".jsontree.js", folderPath + "jsontree.js");
            SaveBinaryResourceToDisk(OfflinePackage + ".logo.png", folderPath + "logo.png");

            if (saveFontAwesomeFonts)
            {
                SaveResourceToDisk(OfflinePackage + ".font-awesome.min.css", folderPath + "font-awesome.min.css");
                SaveBinaryResourceToDisk(OfflinePackage + ".FontAwesome.otf", folderPath + "FontAwesome.otf");
                SaveBinaryResourceToDisk(OfflinePackage + ".fontawesome-webfont.eot", folderPath + "fontawesome-webfont.eot");
                SaveBinaryResourceToDisk(OfflinePackage + ".fontawesome-webfont.svg", folderPath + "fontawesome-webfont.svg");
                SaveBinaryResourceToDisk(OfflinePackage + ".fontawesome-webfont.ttf", folderPath + "fontawesome-webfont.ttf");
                SaveBinaryResourceToDisk(OfflinePackage + ".fontawesome-webfont.woff", folderPath + "fontawesome-webfont.woff");
                SaveBinaryResourceToDisk(OfflinePackage + ".fontawesome-webfont.woff2", folderPath + "fontawesome-webfont.woff2");
            }
        }
    }
}
