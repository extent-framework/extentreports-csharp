using System.IO;

namespace AventStack.ExtentReports.Reporter
{
    public abstract class AbstractFileReporter : AbstractFilterableReporter<ExtentSparkReporter>
    {
        internal string SavePath { get; set; } = "Index.html";
        internal string FolderSavePath { get; set; }

        public AbstractFileReporter(string filePath)
        {
            SavePath = filePath;
            FolderSavePath = Path.GetDirectoryName(filePath);
            FolderSavePath = string.IsNullOrEmpty(FolderSavePath) ? "." + Path.DirectorySeparatorChar : FolderSavePath;

            if (!Directory.Exists(FolderSavePath))
            {
                Directory.CreateDirectory(FolderSavePath);
            }
        }
    }
}
