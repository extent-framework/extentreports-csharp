using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Core
{
    public class ExtentReportsSystemEnvTest
    {
        private ExtentReports _extent;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
        }

        [Test]
        public void SystemInfo()
        {
            _extent.AddSystemInfo("a", "b");
            Assert.AreEqual(_extent.Report.SystemEnvInfo.Count, 1);
            Assert.AreEqual(_extent.Report.SystemEnvInfo[0].Name, "a");
            Assert.AreEqual(_extent.Report.SystemEnvInfo[0].Value, "b");
        }

        [Test]
        public void NullSystemInfo()
        {
            _extent.AddSystemInfo(null, null);
            Assert.AreEqual(_extent.Report.SystemEnvInfo.Count, 1);
            Assert.AreEqual(_extent.Report.SystemEnvInfo[0].Name, null);
            Assert.AreEqual(_extent.Report.SystemEnvInfo[0].Value, null);
        }

        [Test]
        public void EmptySystemInfo()
        {
            _extent.AddSystemInfo("", "");
            Assert.AreEqual(_extent.Report.SystemEnvInfo.Count, 1);
            Assert.AreEqual(_extent.Report.SystemEnvInfo[0].Name, "");
            Assert.AreEqual(_extent.Report.SystemEnvInfo[0].Value, "");
        }
    }
}
