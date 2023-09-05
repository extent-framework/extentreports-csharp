using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Core
{
    public class BddTypeTest
    {
        private ExtentReports _extent;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
        }

        [Test]
        public void FeatureIsOfBddType()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            Assert.True(feature.Test.IsBdd);
            Assert.AreEqual(feature.Test.BddType.Name, "Feature");
        }

        [Test]
        public void ScenarioIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            Assert.True(scenario.Test.IsBdd);
            Assert.AreEqual(scenario.Test.BddType.Name, "Scenario");
        }

        [Test]
        public void ScenarioOutlineIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenarioOutline = feature.CreateNode<ScenarioOutline>("ScenarioOutline");
            Assert.True(scenarioOutline.Test.IsBdd);
            Assert.AreEqual(scenarioOutline.Test.BddType.Name, "ScenarioOutline");
        }

        [Test]
        public void AndIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var and = scenario.CreateNode<And>("And");
            Assert.True(and.Test.IsBdd);
            Assert.AreEqual(and.Test.BddType.Name, "And");
        }

        [Test]
        public void AsteriskIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var asterisk = scenario.CreateNode<Asterisk>("Asterisk");
            Assert.True(asterisk.Test.IsBdd);
            Assert.AreEqual(asterisk.Test.BddType.Name, "*");
        }

        [Test]
        public void BackgroundIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var background = scenario.CreateNode<Background>("Background");
            Assert.True(background.Test.IsBdd);
            Assert.AreEqual(background.Test.BddType.Name, "Background");
        }

        [Test]
        public void ButIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var but = scenario.CreateNode<But>("But");
            Assert.True(but.Test.IsBdd);
            Assert.AreEqual(but.Test.BddType.Name, "But");
        }

        [Test]
        public void GivenIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var given = scenario.CreateNode<Given>("Given");
            Assert.True(given.Test.IsBdd);
            Assert.AreEqual(given.Test.BddType.Name, "Given");
        }

        [Test]
        public void ThenIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var then = scenario.CreateNode<Then>("Then");
            Assert.True(then.Test.IsBdd);
            Assert.AreEqual(then.Test.BddType.Name, "Then");
        }

        [Test]
        public void WhenIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var when = scenario.CreateNode<When>("When");
            Assert.True(when.Test.IsBdd);
            Assert.AreEqual(when.Test.BddType.Name, "When");
        }
    }
}
