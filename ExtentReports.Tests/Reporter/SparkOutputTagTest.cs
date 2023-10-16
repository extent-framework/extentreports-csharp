using System.IO;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Reporter
{
    public class SparkOutputTagTest
    {
        private const string Path = "spark.html";
        private const string Tag = "TAG#1234";

        private ExtentReports _extent;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
            _extent.AttachReporter(new ExtentSparkReporter(Path));
        }

        [Test]
        public void TestTag()
        {
            _extent.CreateTest("Test1").AssignCategory(Tag).Pass();
            _extent.CreateTest("Test2").AssignCategory(Tag).Fail();
            _extent.Flush();
            var text = File.ReadAllText(Path);
            Assert.True(text.Contains(Tag));
        }

        [Test]
        public void NodeTag()
        {
            _extent.CreateTest("Test").CreateNode("Node").AssignCategory(Tag);
            _extent.Flush();
            var text = File.ReadAllText(Path);
            Assert.True(text.Contains(Tag));
        }
    }
}
