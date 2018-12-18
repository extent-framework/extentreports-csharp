using AventStack.ExtentReports.Gherkin.Model;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;

namespace AventStack.ExtentReports.Tests.APITests
{
    [TestFixture]
    public class BddWithStepStatusHierarchyTests : Base
    {
        [Test]
        public void throwClassNotFoundExceptionWithInvalidKeyword()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);

            ActualValueDelegate<object> testDelegate = () => feature.CreateNode(new GherkinKeyword("Invalid"), "Child");
            Assert.That(testDelegate, Throws.TypeOf<InvalidOperationException>());

            Assert.AreEqual(feature.Status, Status.Pass);
        }

        [Test]
        public void throwClassNotFoundExceptionWithEmptyKeyword()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);

            ActualValueDelegate<object> testDelegate = () => feature.CreateNode(new GherkinKeyword(""), "Child");
            Assert.That(testDelegate, Throws.TypeOf<InvalidOperationException>());

            Assert.AreEqual(feature.Status, Status.Pass);
        }

        [Test]
        public void verifyValidKeywordFoundWithInvalidCaseFirstCharacter()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            feature.CreateNode(new GherkinKeyword("given"), "Child").Pass("Pass");

            Assert.AreEqual(feature.Status, Status.Pass);
        }

        [Test]
        public void verifyValidKeywordFounWithInvalidCase()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            feature.CreateNode(new GherkinKeyword("giVen"), "Child").Pass("Pass");

            Assert.AreEqual(feature.Status, Status.Pass);
        }

        [Test]
        public void verifyPassHasHigherPriorityThanInfoUsingGherkinKeyword()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode(new GherkinKeyword("Scenario"), "Child");
            var given = scenario.CreateNode(new GherkinKeyword("Given"), "Given").Info("Info");
            var and = scenario.CreateNode(new GherkinKeyword("And"), "And").Info("Info");
            var when = scenario.CreateNode(new GherkinKeyword("When"), "When").Info("Info");
            var then = scenario.CreateNode(new GherkinKeyword("Then"), "Then").Pass("Pass");

            Assert.AreEqual(feature.Model.Level, 0);
            Assert.AreEqual(scenario.Model.Level, 1);
            Assert.AreEqual(given.Model.Level, 2);
            Assert.AreEqual(and.Model.Level, 2);
            Assert.AreEqual(when.Model.Level, 2);
            Assert.AreEqual(then.Model.Level, 2);
            Assert.AreEqual(given.Status, Status.Pass);
            Assert.AreEqual(and.Status, Status.Pass);
            Assert.AreEqual(when.Status, Status.Pass);
            Assert.AreEqual(then.Status, Status.Pass);
            Assert.AreEqual(scenario.Status, Status.Pass);
            Assert.AreEqual(feature.Status, Status.Pass);
        }

        [Test]
        public void verifyPassHasHigherPriorityThanInfoUsingClass()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var given = scenario.CreateNode<Given>("Given").Info("Info");
            var and = scenario.CreateNode<And>("And").Info("Info");
            var when = scenario.CreateNode<When>("When").Info("Info");
            var then = scenario.CreateNode<Then>("Then").Pass("Pass");

            Assert.AreEqual(given.Status, Status.Pass);
            Assert.AreEqual(and.Status, Status.Pass);
            Assert.AreEqual(when.Status, Status.Pass);
            Assert.AreEqual(then.Status, Status.Pass);
            Assert.AreEqual(scenario.Status, Status.Pass);
            Assert.AreEqual(feature.Status, Status.Pass);
        }

        [Test]
        public void verifySkipHasHigherPriorityThanPassUsingGherkinKeyword()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode(new GherkinKeyword("Scenario"), "Child");
            var given = scenario.CreateNode(new GherkinKeyword("Given"), "Given").Pass("Pass");
            var and = scenario.CreateNode(new GherkinKeyword("And"), "And").Pass("Pass");
            var when = scenario.CreateNode(new GherkinKeyword("When"), "When").Pass("Pass");
            var then = scenario.CreateNode(new GherkinKeyword("Then"), "Then").Skip("Skip");

            Assert.AreEqual(given.Status, Status.Pass);
            Assert.AreEqual(and.Status, Status.Pass);
            Assert.AreEqual(when.Status, Status.Pass);
            Assert.AreEqual(then.Status, Status.Skip);
            Assert.AreEqual(scenario.Status, Status.Skip);
            Assert.AreEqual(feature.Status, Status.Skip);
        }

        [Test]
        public void verifySkipHasHigherPriorityThanPassUsingClass()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var given = scenario.CreateNode<Given>("Given").Pass("Pass");
            var and = scenario.CreateNode<And>("And").Pass("Pass");
            var when = scenario.CreateNode<When>("When").Pass("Pass");
            var then = scenario.CreateNode<Then>("Then").Skip("Skip");

            Assert.AreEqual(given.Status, Status.Pass);
            Assert.AreEqual(and.Status, Status.Pass);
            Assert.AreEqual(when.Status, Status.Pass);
            Assert.AreEqual(then.Status, Status.Skip);
            Assert.AreEqual(scenario.Status, Status.Skip);
            Assert.AreEqual(feature.Status, Status.Skip);
        }

        [Test]
        public void verifyWarningHasHigherPriorityThanSkipUsingGherkinKeyword()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode(new GherkinKeyword("Scenario"), "Child");
            var given = scenario.CreateNode(new GherkinKeyword("Given"), "Given").Skip("Skip");
            var and = scenario.CreateNode(new GherkinKeyword("And"), "And").Skip("Skip");
            var when = scenario.CreateNode(new GherkinKeyword("When"), "When").Skip("Skip");
            var then = scenario.CreateNode(new GherkinKeyword("Then"), "Then").Warning("Warning");

            Assert.AreEqual(given.Status, Status.Skip);
            Assert.AreEqual(and.Status, Status.Skip);
            Assert.AreEqual(when.Status, Status.Skip);
            Assert.AreEqual(then.Status, Status.Warning);
            Assert.AreEqual(scenario.Status, Status.Warning);
            Assert.AreEqual(feature.Status, Status.Warning);
        }

        [Test]
        public void verifyWarningHasHigherPriorityThanSkipUsingClass()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var given = scenario.CreateNode<Given>("Given").Skip("Skip");
            var and = scenario.CreateNode<And>("And").Skip("Skip");
            var when = scenario.CreateNode<When>("When").Skip("Skip");
            var then = scenario.CreateNode<Then>("Then").Warning("Warning");

            Assert.AreEqual(given.Status, Status.Skip);
            Assert.AreEqual(and.Status, Status.Skip);
            Assert.AreEqual(when.Status, Status.Skip);
            Assert.AreEqual(then.Status, Status.Warning);
            Assert.AreEqual(scenario.Status, Status.Warning);
            Assert.AreEqual(feature.Status, Status.Warning);
        }

        [Test]
        public void verifyErrorHasHigherPriorityThanWarningUsingGherkinKeyword()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode(new GherkinKeyword("Scenario"), "Child");
            var given = scenario.CreateNode(new GherkinKeyword("Given"), "Given").Warning("Warning");
            var and = scenario.CreateNode(new GherkinKeyword("And"), "And").Warning("Warning");
            var when = scenario.CreateNode(new GherkinKeyword("When"), "When").Warning("Warning");
            var then = scenario.CreateNode(new GherkinKeyword("Then"), "Then").Error("Error");

            Assert.AreEqual(given.Status, Status.Warning);
            Assert.AreEqual(and.Status, Status.Warning);
            Assert.AreEqual(when.Status, Status.Warning);
            Assert.AreEqual(then.Status, Status.Error);
            Assert.AreEqual(scenario.Status, Status.Error);
            Assert.AreEqual(feature.Status, Status.Error);
        }

        [Test]
        public void verifyErrorHasHigherPriorityThanWarningUsingClass()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var given = scenario.CreateNode<Given>("Given").Warning("Warning");
            var and = scenario.CreateNode<And>("And").Warning("Warning");
            var when = scenario.CreateNode<When>("When").Warning("Warning");
            var then = scenario.CreateNode<Then>("Then").Error("Error");

            Assert.AreEqual(given.Status, Status.Warning);
            Assert.AreEqual(and.Status, Status.Warning);
            Assert.AreEqual(when.Status, Status.Warning);
            Assert.AreEqual(then.Status, Status.Error);
            Assert.AreEqual(scenario.Status, Status.Error);
            Assert.AreEqual(feature.Status, Status.Error);
        }

        [Test]
        public void verifyFailHasHigherPriorityThanErrorUsingGherkinKeyword()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode(new GherkinKeyword("Scenario"), "Child");
            var given = scenario.CreateNode(new GherkinKeyword("Given"), "Given").Error("Error");
            var and = scenario.CreateNode(new GherkinKeyword("And"), "And").Error("Error");
            var when = scenario.CreateNode(new GherkinKeyword("When"), "When").Error("Error");
            var then = scenario.CreateNode(new GherkinKeyword("Then"), "Then").Fail("Fail");

            Assert.AreEqual(given.Status, Status.Error);
            Assert.AreEqual(and.Status, Status.Error);
            Assert.AreEqual(when.Status, Status.Error);
            Assert.AreEqual(then.Status, Status.Fail);
            Assert.AreEqual(scenario.Status, Status.Fail);
            Assert.AreEqual(feature.Status, Status.Fail);
        }

        [Test]
        public void verifyFailHasHigherPriorityThanErrorUsingClass()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var given = scenario.CreateNode<Given>("Given").Error("Error");
            var and = scenario.CreateNode<And>("And").Error("Error");
            var when = scenario.CreateNode<When>("When").Error("Error");
            var then = scenario.CreateNode<Then>("Then").Fail("Fail");

            Assert.AreEqual(given.Status, Status.Error);
            Assert.AreEqual(and.Status, Status.Error);
            Assert.AreEqual(when.Status, Status.Error);
            Assert.AreEqual(then.Status, Status.Fail);
            Assert.AreEqual(scenario.Status, Status.Fail);
            Assert.AreEqual(feature.Status, Status.Fail);
        }

        [Test]
        public void verifyFatalHasHigherPriorityThanFailUsingGherkinKeyword()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode(new GherkinKeyword("Scenario"), "Child");
            var given = scenario.CreateNode(new GherkinKeyword("Given"), "Given").Fail("Fail");
            var and = scenario.CreateNode(new GherkinKeyword("And"), "And").Fail("Fail");
            var when = scenario.CreateNode(new GherkinKeyword("When"), "When").Fail("Fail");
            var then = scenario.CreateNode(new GherkinKeyword("Then"), "Then").Fatal("Fatal");

            Assert.AreEqual(given.Status, Status.Fail);
            Assert.AreEqual(and.Status, Status.Fail);
            Assert.AreEqual(when.Status, Status.Fail);
            Assert.AreEqual(then.Status, Status.Fatal);
            Assert.AreEqual(scenario.Status, Status.Fatal);
            Assert.AreEqual(feature.Status, Status.Fatal);
        }

        [Test]
        public void verifyFatalHasHigherPriorityThanFailUsingClass()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var given = scenario.CreateNode<Given>("Given").Fail("Fail");
            var and = scenario.CreateNode<And>("And").Fail("Fail");
            var when = scenario.CreateNode<When>("When").Fail("Fail");
            var then = scenario.CreateNode<Then>("Then").Fatal("Fatal");

            Assert.AreEqual(given.Status, Status.Fail);
            Assert.AreEqual(and.Status, Status.Fail);
            Assert.AreEqual(when.Status, Status.Fail);
            Assert.AreEqual(then.Status, Status.Fatal);
            Assert.AreEqual(scenario.Status, Status.Fatal);
            Assert.AreEqual(feature.Status, Status.Fatal);
        }
    }
}
