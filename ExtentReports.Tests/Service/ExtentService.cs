using System;

using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;

using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Parallel
{
    public class ExtentService
    {
        private static readonly Lazy<ExtentReports> _lazy = new Lazy<ExtentReports>(() => new ExtentReports());

        public static ExtentReports Instance { get { return _lazy.Value; } }

        static ExtentService()
        {
            var htmlReporter = new ExtentHtmlReporter(TestContext.CurrentContext.TestDirectory + "\\Extent.html");
            htmlReporter.Config.DocumentTitle = "Extent/NUnit Samples";
            htmlReporter.Config.ReportName = "Extent/NUnit Samples";
            htmlReporter.Config.Theme = Theme.Standard;
            Instance.AttachReporter(htmlReporter);
        }

        private ExtentService()
        {
        }
    }
}
