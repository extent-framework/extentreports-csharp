using System;

using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.APITests
{
    [TestFixture]
    public class BuildsReportWithoutAttachedReporters
    {
        private ExtentReports _extent;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
        }

        [Test]
        public void CreateTestWithoutAttachedReporter()
        {
            Assert.DoesNotThrow(() => CreateTest());
        }

        private void CreateTest()
        {
            _extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void TearDown()
        {
            _extent.Flush();
        }
    }
}
