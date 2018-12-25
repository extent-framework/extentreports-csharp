using AventStack.ExtentReports.Reporter;

using NUnit.Framework;

using System.IO;

namespace AventStack.ExtentReports.Tests.APITests
{
    [TestFixture]
    public class BuildsReportWithoutTests
    {
        [Test]
        public void BuildReportWithoutTests()
        {
            var fileName = TestContext.CurrentContext.Test.Name + ".html";
            var reporter = new ExtentV3HtmlReporter(fileName);
            var extent = new ExtentReports();
            extent.AttachReporter(reporter);
            extent.Flush();
            Assert.False(File.Exists(fileName));
        }
    }
}
