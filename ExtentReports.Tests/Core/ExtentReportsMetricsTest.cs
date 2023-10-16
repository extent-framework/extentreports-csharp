using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Core
{
    public class ExtentReportsMetricsTest
    {
        
        private ExtentReports _extent;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
        }

        [Test]
        public void ExtentTestWithAttributeCount()
        {
            _extent.CreateTest("Test1").AssignCategory("Tag1").Pass();
            _extent.CreateTest("Test2").AssignCategory("Tag2").Pass();
            _extent.CreateTest("Test3").AssignCategory("Tag1").Fail();
            _extent.CreateTest("Test4").AssignCategory("Tag2").Skip();
            _extent.Flush();

            var tag1Ctx = _extent.Report.CategoryCtx.Context["Tag1"];
            Assert.AreEqual(1, tag1Ctx.Passed);
            Assert.AreEqual(1, tag1Ctx.Failed);

            var tag2Ctx = _extent.Report.CategoryCtx.Context["Tag2"];
            Assert.AreEqual(1, tag2Ctx.Passed);
            Assert.AreEqual(1, tag2Ctx.Skipped);
        }
    }
}
