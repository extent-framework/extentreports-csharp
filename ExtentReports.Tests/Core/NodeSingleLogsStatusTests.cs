using NUnit.Framework;

using System.Linq;

namespace AventStack.ExtentReports.Tests.APITests
{
    [TestFixture]
    public class NodeSingleLogsStatusTests : Base
    {
        [Test]
        public void verifyIfTestHasStatusPass()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var node = test.CreateNode("Child").Pass("Pass");

            Assert.AreEqual(node.Model.Level, 1);
            Assert.AreEqual(node.Model.LogContext.Count, 1);
            Assert.AreEqual(node.Status, Status.Pass);
            Assert.AreEqual(test.Status, Status.Pass);
        }

        [Test]
        public void verifyIfTestHasStatusSkip()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var node = test.CreateNode("Child").Skip("Skip");

            Assert.AreEqual(node.Model.Level, 1);
            Assert.AreEqual(node.Model.LogContext.Count, 1);
            Assert.AreEqual(node.Status, Status.Skip);
            Assert.AreEqual(test.Status, Status.Skip);
        }

        [Test]
        public void verifyIfTestHasStatusWarning()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var node = test.CreateNode("Child").Warning("Warning");

            Assert.AreEqual(node.Model.Level, 1);
            Assert.AreEqual(node.Model.LogContext.Count, 1);
            Assert.AreEqual(node.Status, Status.Warning);
            Assert.AreEqual(test.Status, Status.Warning);
        }

        [Test]
        public void verifyIfTestHasStatusError()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var node = test.CreateNode("Child").Error("Error");

            Assert.AreEqual(node.Model.Level, 1);
            Assert.AreEqual(node.Model.LogContext.Count, 1);
            Assert.AreEqual(node.Status, Status.Error);
            Assert.AreEqual(test.Status, Status.Error);
        }

        [Test]
        public void verifyIfTestHasStatusFail()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var node = test.CreateNode("Child").Fail("Fail");

            Assert.AreEqual(node.Model.Level, 1);
            Assert.AreEqual(node.Model.LogContext.Count, 1);
            Assert.AreEqual(node.Status, Status.Fail);
            Assert.AreEqual(test.Status, Status.Fail);
        }

        [Test]
        public void verifyIfTestHasStatusFatal()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var node = test.CreateNode("Child").Fatal("Fatal");

            Assert.AreEqual(node.Model.Level, 1);
            Assert.AreEqual(node.Model.LogContext.Count, 1);
            Assert.AreEqual(node.Status, Status.Fatal);
            Assert.AreEqual(test.Status, Status.Fatal);
        }

        [Test]
        public void verifyIfTestHasStatusPassWithOnlyInfoSingle()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var node = test.CreateNode("Child").Info("Info");

            Assert.AreEqual(node.Model.Level, 1);
            Assert.AreEqual(node.Model.LogContext.Count, 1);
            Assert.AreEqual(node.Status, Status.Pass);
            Assert.AreEqual(test.Status, Status.Pass);
        }
    }
}
