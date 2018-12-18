using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AventStack.ExtentReports.Tests.APITests
{
    [TestFixture]
    public class TestStatusHierarchyTests : Base
    {
        [Test]
        public void VerifyPassHasHigherPriorityThanInfo()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            test.Info("Info");
            test.Pass("Pass");
            
            Assert.AreEqual(test.Status, Status.Pass);
        }

        [Test]
        public void VerifySkipHasHigherPriorityThanPass()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            test.Pass("Pass");
            test.Skip("Skip");

            Assert.AreEqual(test.Status, Status.Skip);
        }

        [Test]
        public void VerifyWarningHasHigherPriorityThanSkip()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            test.Skip("Skip");
            test.Warning("Warning");

            Assert.AreEqual(test.Status, Status.Warning);
        }

        [Test]
        public void VerifyErrorHasHigherPriorityThanWarning()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            test.Warning("Warning");
            test.Error("Error");

            Assert.AreEqual(test.Status, Status.Error);
        }

        [Test]
        public void VerifFailHasHigherPriorityThanError()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            test.Error("Error");
            test.Fail("Fail");

            Assert.AreEqual(test.Status, Status.Fail);
        }

        [Test]
        public void VerifFatalHasHigherPriorityThanFail()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            test.Fail("Fail");
            test.Fatal("Fatal");

            Assert.AreEqual(test.Status, Status.Fatal);
        }
    }
}
