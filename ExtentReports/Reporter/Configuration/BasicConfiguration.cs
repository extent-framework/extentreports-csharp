using System.Collections.Generic;

namespace AventStack.ExtentReports.Reporter.Configuration
{
    public abstract class BasicConfiguration
    {
        public AbstractReporter Reporter { get; protected internal set; }
        public Dictionary<string, string> UserConfigurationMap = new Dictionary<string, string>();

        public string ReportName
        {
            get
            {
                return _reportName;
            }
            set
            {
                UserConfigurationMap.Add("reportName", value);
                _reportName = value;
            }
        }

        private string _reportName;

        protected BasicConfiguration(AbstractReporter reporter)
        {
            Reporter = reporter;
        }
    }
}
