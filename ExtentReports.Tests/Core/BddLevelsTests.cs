using AventStack.ExtentReports.Gherkin.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.APITests
{
    [TestFixture]
    public class BddLevelsTests : Base
    {
        [Test]
        public void VerifyLevelsUsingGherkinKeyword()
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
        }

        [Test]
        public void VerifyLevelsUsingClass()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Child");
            var given = scenario.CreateNode<Given>("Given").Info("Info");
            var and = scenario.CreateNode<And>("And").Info("Info");
            var when = scenario.CreateNode<When>("When").Info("Info");
            var then = scenario.CreateNode<Then>("Then").Pass("Pass");

            Assert.AreEqual(feature.Model.Level, 0);
            Assert.AreEqual(scenario.Model.Level, 1);
            Assert.AreEqual(given.Model.Level, 2);
            Assert.AreEqual(and.Model.Level, 2);
            Assert.AreEqual(when.Model.Level, 2);
            Assert.AreEqual(then.Model.Level, 2);
        }
    }
}
