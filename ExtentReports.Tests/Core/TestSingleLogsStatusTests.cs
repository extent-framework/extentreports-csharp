using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.APITests
{
    [TestFixture]
    public class TestSingleLogsStatusTests : Base
    {
        [Test]
        public void verifyIfTestHasStatusPass()
        {
            //Console.WriteLine("in " + TestContext.CurrentContext.Test.Name);
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name).Pass("pass");

            Assert.AreEqual(test.Model.LogContext.Count, 1);
            Assert.AreEqual(test.Status, Status.Pass);
            //Console.WriteLine("out " + TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public void verifyIfTestHasStatusSkip()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name).Skip("skip");

            Assert.AreEqual(test.Model.LogContext.Count, 1);
            Assert.AreEqual(test.Status, Status.Skip);
        }

        [Test]
        public void verifyIfTestHasStatusWarning()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name).Warning("warning");

            Assert.AreEqual(test.Model.LogContext.Count, 1);
            Assert.AreEqual(test.Status, Status.Warning);
        }

        [Test]
        public void verifyIfTestHasStatusError()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name).Error("error");

            Assert.AreEqual(test.Model.LogContext.Count, 1);
            Assert.AreEqual(test.Status, Status.Error);
        }

        [Test]
        public void verifyIfTestHasStatusFail()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name).Fail("fail");

            Assert.AreEqual(test.Model.LogContext.Count, 1);
            Assert.AreEqual(test.Status, Status.Fail);
        }

        [Test]
        public void verifyIfTestHasStatusFatal()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name).Fatal("fatal");

            Assert.AreEqual(test.Model.LogContext.Count, 1);
            Assert.AreEqual(test.Status, Status.Fatal);
        }

        [Test]
        public void verifyIfTestHasStatusPassWithOnlyInfoSingle()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name).Info("info");

            Assert.AreEqual(test.Model.LogContext.Count, 1);
            Assert.AreEqual(test.Status, Status.Pass);
        }
    }
}
