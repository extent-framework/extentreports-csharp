using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Model;
using NUnit.Framework;
using NUnit.Framework.Internal;

using System.Linq;

namespace AventStack.ExtentReports.Tests.Core
{
    public class ExtentTestGeneratedLogTest
    {
        private const string TestName = "Test";

        private ExtentReports _extent;
        private ExtentTest _test;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
            _test = _extent.CreateTest(TestName);
        }

        [Test]
        public void GeneratedLogEmpty()
        {
            var t = _test.GenerateLog(Status.Pass, "");
            Assert.True(t.Test.HasAnyLog);
            Assert.True(t.Test.HasGeneratedLog);
            Assert.True(!t.Test.GeneratedLog.IsEmpty);
            Assert.AreEqual(t.Test.GeneratedLog.First().Details, "");
        }

        [Test]
        public void GeneratedLogDetails()
        {
            var t = _test.GenerateLog(Status.Pass, "Details");
            Assert.True(t.Test.HasAnyLog);
            Assert.True(!t.Test.GeneratedLog.IsEmpty);
            Assert.AreEqual(t.Test.GeneratedLog.First().Details, "Details");
        }

        [Test]
        public void GeneratedLogMarkup()
        {
            var json = "{ 'key': 'value' }";
            var m = MarkupHelper.CreateCodeBlock(json, CodeLanguage.Json);
            var t = _test.GenerateLog(Status.Pass, m);
            Assert.True(t.Test.HasAnyLog);
            Assert.True(!t.Test.GeneratedLog.IsEmpty);
            Assert.True(t.Test.GeneratedLog.First().Details.Contains("jsonTree"));
            Assert.True(t.Test.GeneratedLog.First().Details.Contains("<script>"));
            Assert.True(t.Test.GeneratedLog.First().Details.Contains("</script>"));
            Assert.True(t.Test.GeneratedLog.First().Details.Contains("{ 'key': 'value' }"));
        }
    }
}
