using System.Linq;
using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Core
{
    public class ExtentReportsRemoveTestTest
    {
        private ExtentReports _extent;
        private ExtentTest _test;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
            _test = _extent.CreateTest("Test");
        }

        [Test]
        public void RemoveTest()
        {
            Assert.AreEqual(1, _extent.Report.Tests.Count);
            _extent.RemoveTest(_test);
            Assert.AreEqual(0, _extent.Report.Tests.Count);
        }

        [Test]
        public void RemoveTestByName()
        {
            Assert.AreEqual(1, _extent.Report.Tests.Count);
            _extent.RemoveTest("Test");
            Assert.AreEqual(0, _extent.Report.Tests.Count);
        }

        [Test]
        public void RemoveNode()
        {
            var node = _test.CreateNode("Node");
            Assert.AreEqual(1, _extent.Report.Tests.Count);
            Assert.AreEqual(1, _extent.Report.Tests.ToList()[0].Children.Count);
            _extent.RemoveTest(node);
            Assert.AreEqual(1, _extent.Report.Tests.Count);
            Assert.AreEqual(0, _extent.Report.Tests.ToList()[0].Children.Count);
        }

        [Test]
        public void RemoveNodeByName()
        {
            _test.CreateNode("Node");
            Assert.AreEqual(1, _extent.Report.Tests.Count);
            Assert.AreEqual(1, _extent.Report.Tests.ToList()[0].Children.Count);
            _extent.RemoveTest("Node");
            Assert.AreEqual(1, _extent.Report.Tests.Count);
            Assert.AreEqual(0, _extent.Report.Tests.ToList()[0].Children.Count);
        }
    }
}
