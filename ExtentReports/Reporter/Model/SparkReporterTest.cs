using AventStack.ExtentReports.Model;

namespace AventStack.ExtentReports.Reporter.Model
{
    public class SparkReporterTest
    {
        public SparkReporterTest(ExtentSparkReporter reporter, Test test)
        {
            Reporter = reporter;
            Test = test;
        }

        public ExtentSparkReporter Reporter;
        public Test Test;
    }
}
