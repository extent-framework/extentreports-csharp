using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.APITests
{
    [TestFixture]
    public class NodesStatusHierarchyTests : Base
    {
        [Test]
        public void verifyPassHasHigherPriorityThanInfoLevelsShallow()
        {
            var parent = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var child = parent.CreateNode("Child");
            child.Info("Info");
            child.Pass("Pass");

            Assert.AreEqual(child.Model.Level, 1);
            Assert.AreEqual(parent.Status, Status.Pass);
            Assert.AreEqual(child.Status, Status.Pass);
        }

        [Test]
        public void verifyPassHasHigherPriorityThanInfoLevelsDeep()
        {
            var parent = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var child = parent.CreateNode("Child");
            var grandchild = child.CreateNode("GrandChild");
            grandchild.Info("Info");
            grandchild.Pass("Pass");

            Assert.AreEqual(child.Model.Level, 1);
            Assert.AreEqual(grandchild.Model.Level, 2);
            Assert.AreEqual(parent.Status, Status.Pass);
            Assert.AreEqual(child.Status, Status.Pass);
            Assert.AreEqual(grandchild.Status, Status.Pass);
        }

        [Test]
        public void verifySkipHasHigherPriorityThanPassLevelsShallow()
        {
            var parent = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var child = parent.CreateNode("Child");
            child.Pass("Pass");
            child.Skip("Skip");

            Assert.AreEqual(child.Model.Level, 1);
            Assert.AreEqual(parent.Status, Status.Skip);
            Assert.AreEqual(child.Status, Status.Skip);
        }

        [Test]
        public void verifySkipHasHigherPriorityThanPassLevelsDeep()
        {
            var parent = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var child = parent.CreateNode("Child");
            var grandchild = child.CreateNode("GrandChild");
            grandchild.Pass("Pass");
            grandchild.Skip("Skip");

            Assert.AreEqual(child.Model.Level, 1);
            Assert.AreEqual(grandchild.Model.Level, 2);
            Assert.AreEqual(parent.Status, Status.Skip);
            Assert.AreEqual(child.Status, Status.Skip);
            Assert.AreEqual(grandchild.Status, Status.Skip);
        }

        [Test]
        public void verifyWarningHasHigherPriorityThanSkipLevelsShallow()
        {
            var parent = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var child = parent.CreateNode("Child");
            child.Skip("Skip");
            child.Warning("Warning");

            Assert.AreEqual(child.Model.Level, 1);
            Assert.AreEqual(parent.Status, Status.Warning);
            Assert.AreEqual(child.Status, Status.Warning);
        }

        [Test]
        public void verifyWarningHasHigherPriorityThanSkipLevelsDeep()
        {
            var parent = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var child = parent.CreateNode("Child");
            var grandchild = child.CreateNode("GrandChild");
            grandchild.Skip("Skip");
            grandchild.Warning("Warning");

            Assert.AreEqual(child.Model.Level, 1);
            Assert.AreEqual(grandchild.Model.Level, 2);
            Assert.AreEqual(parent.Status, Status.Warning);
            Assert.AreEqual(child.Status, Status.Warning);
            Assert.AreEqual(grandchild.Status, Status.Warning);
        }

        [Test]
        public void verifyErrorHasHigherPriorityThanWarningLevelsShallow()
        {
            var parent = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var child = parent.CreateNode("Child");
            child.Warning("Warning");
            child.Error("Error");

            Assert.AreEqual(child.Model.Level, 1);
            Assert.AreEqual(parent.Status, Status.Error);
            Assert.AreEqual(child.Status, Status.Error);
        }

        [Test]
        public void verifyErrorHasHigherPriorityThanWarningLevelsDeep()
        {
            var parent = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var child = parent.CreateNode("Child");
            var grandchild = child.CreateNode("GrandChild");
            grandchild.Warning("Warning");
            grandchild.Error("Error");

            Assert.AreEqual(child.Model.Level, 1);
            Assert.AreEqual(grandchild.Model.Level, 2);
            Assert.AreEqual(parent.Status, Status.Error);
            Assert.AreEqual(child.Status, Status.Error);
            Assert.AreEqual(grandchild.Status, Status.Error);
        }

        [Test]
        public void verifFailHasHigherPriorityThanErrorLevelsShallow()
        {
            var parent = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var child = parent.CreateNode("Child");
            child.Error("Error");
            child.Fail("Fail");

            Assert.AreEqual(child.Model.Level, 1);
            Assert.AreEqual(parent.Status, Status.Fail);
            Assert.AreEqual(child.Status, Status.Fail);
        }

        [Test]
        public void verifFailHasHigherPriorityThanErrorLevelsDeep()
        {
            var parent = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var child = parent.CreateNode("Child");
            var grandchild = child.CreateNode("GrandChild");
            grandchild.Error("Error");
            grandchild.Fail("Fail");

            Assert.AreEqual(child.Model.Level, 1);
            Assert.AreEqual(grandchild.Model.Level, 2);
            Assert.AreEqual(parent.Status, Status.Fail);
            Assert.AreEqual(child.Status, Status.Fail);
            Assert.AreEqual(grandchild.Status, Status.Fail);
        }

        [Test]
        public void verifFatalHasHigherPriorityThanFailLevelsShallow()
        {
            var parent = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var child = parent.CreateNode("Child");
            child.Fail("Fail");
            child.Fatal("Fatal");

            Assert.AreEqual(child.Model.Level, 1);
            Assert.AreEqual(parent.Status, Status.Fatal);
            Assert.AreEqual(child.Status, Status.Fatal);
        }

        [Test]
        public void verifFatalHasHigherPriorityThanFailLevelsDeep()
        {
            var parent = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var child = parent.CreateNode("Child");
            var grandchild = child.CreateNode("GrandChild");
            grandchild.Fail("Fail");
            grandchild.Fatal("Fatal");

            Assert.AreEqual(child.Model.Level, 1);
            Assert.AreEqual(grandchild.Model.Level, 2);
            Assert.AreEqual(parent.Status, Status.Fatal);
            Assert.AreEqual(child.Status, Status.Fatal);
            Assert.AreEqual(grandchild.Status, Status.Fatal);
        }
    }
}
