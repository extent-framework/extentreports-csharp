using NUnit.Framework;

using System.Linq;

namespace AventStack.ExtentReports.Tests.APITests
{
    [TestFixture]
    public class NodeWithoutLogs : Base
    {
        [Test]
        public void VerifyNodeAndParentHasPassStatusIfNoLogsAdded()
        {
            ExtentTest test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            ExtentTest node = test.CreateNode("Child");

            Assert.AreEqual(node.Model.Level, 1);
            Assert.AreEqual(test.Model.LogContext.Count, 0);
            Assert.AreEqual(test.Status, Status.Pass);
            Assert.AreEqual(node.Model.LogContext.Count, 0);
            Assert.AreEqual(node.Status, Status.Pass);
        }
    }
}
