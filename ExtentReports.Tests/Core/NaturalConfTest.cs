using AventStack.ExtentReports.Model;
using NUnit.Framework;
using System;
using System.Threading;

namespace AventStack.ExtentReports.Tests.Core
{
    public class NaturalConfTest
    {
        private ExtentReports _extent;
        private ExtentTest _test;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports
            {
                UseNaturalTime = false
            };
            _test = _extent.CreateTest("Test");
        }

        [Test]
        public void UseNaturalConfReport()
        {
            _test.Pass("init");
            Thread.Sleep(500);
            _test.Pass("complete");
            // must flush to determine time for report
            _extent.Flush();
            Assert.True(_extent.Report.TimeTaken.TotalMilliseconds < 5);
        }

        [Test]
        public void UseNaturalConfReportWithTimeChanged()
        {
            _test.Pass("init");
            _test.Test.EndTime = DateTime.Now.AddMilliseconds(5000);
            _test.Pass("complete");
            // must flush to determine time for report
            _extent.Flush();
            Assert.True(_extent.Report.TimeTaken.TotalMilliseconds >= 5000);
        }

        [Test]
        public void UseNaturalConfTest()
        {
            _test.Pass("init");
            Thread.Sleep(500);
            _test.Pass("complete");
            Assert.True(_test.Test.TimeTaken < 5);
        }

        [Test]
        public void UseNaturalConfTestWithTimeChanged()
        {
            _test.Pass("init");
            _test.Test.EndTime = DateTime.Now.AddMilliseconds(5000);
            _test.Pass("complete");
            Assert.True(_test.Test.TimeTaken >= 5000);
        }

        [Test]
        public void UseNaturalConfTestWithNodes()
        {
            ExtentTest child = _test.CreateNode("Node").Pass("init");
            Thread.Sleep(500);
            child.Pass("complete");
            Assert.True(_test.Test.TimeTaken < 5);
            Assert.True(child.Test.TimeTaken < 5);
        }
    }
}
