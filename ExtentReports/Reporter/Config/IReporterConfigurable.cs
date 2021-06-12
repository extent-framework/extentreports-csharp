namespace AventStack.ExtentReports.Reporter.Config
{
    public interface IReporterConfigurable
    {
        /// <summary>
        /// Loads reporter configuration from JSON
        /// </summary>
        /// <param name="filePath"></param>
        void LoadJSONConfig(string filePath);
        
        /// <summary>
        /// Loads XML configuration
        /// </summary>
        /// <param name="filePath"></param>
        void LoadXMLConfig(string filePath);

        /// <summary>
        /// Loads XML configuration (for backward compatibility)
        /// </summary>
        /// <param name="filePath"></param>
        void LoadConfig(string filePath);
    }
}
