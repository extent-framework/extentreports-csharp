using AventStack.ExtentReports.Reporter;

namespace AventStack.ExtentReports.Tests.Usage.Core
{
    internal sealed class ExtentManager
    {
        private ExtentManager() { }

        public static ExtentReports Instance
        {
            get
            {
                if (_extent == null)
                {
                    Init();
                }

                return _extent;
            }
        }

        private static ExtentReports _extent;

        public static void Init()
        {
            var extent = new ExtentReports();
            extent.AttachReporter(new ExtentSparkReporter("spark.html"));
            _extent = extent;
        }
    }
}
