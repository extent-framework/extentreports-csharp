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
            Assert.True(feature.Model.IsBdd);
            Assert.AreEqual(feature.Model.BddType.Name, "Feature");
        }

        [Test]
        public void ScenarioIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            Assert.True(scenario.Model.IsBdd);
            Assert.AreEqual(scenario.Model.BddType.Name, "Scenario");
        }

        [Test]
        public void ScenarioOutlineIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenarioOutline = feature.CreateNode<ScenarioOutline>("ScenarioOutline");
            Assert.True(scenarioOutline.Model.IsBdd);
            Assert.AreEqual(scenarioOutline.Model.BddType.Name, "ScenarioOutline");
        }

        [Test]
        public void AndIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var and = scenario.CreateNode<And>("And");
            Assert.True(and.Model.IsBdd);
            Assert.AreEqual(and.Model.BddType.Name, "And");
        }

        [Test]
        public void AsteriskIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var asterisk = scenario.CreateNode<Asterisk>("Asterisk");
            Assert.True(asterisk.Model.IsBdd);
            Assert.AreEqual(asterisk.Model.BddType.Name, "*");
        }

        [Test]
        public void BackgroundIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var background = scenario.CreateNode<Background>("Background");
            Assert.True(background.Model.IsBdd);
            Assert.AreEqual(background.Model.BddType.Name, "Background");
        }

        [Test]
        public void ButIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var but = scenario.CreateNode<But>("But");
            Assert.True(but.Model.IsBdd);
            Assert.AreEqual(but.Model.BddType.Name, "But");
        }

        [Test]
        public void GivenIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var given = scenario.CreateNode<Given>("Given");
            Assert.True(given.Model.IsBdd);
            Assert.AreEqual(given.Model.BddType.Name, "Given");
        }

        [Test]
        public void ThenIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var then = scenario.CreateNode<Then>("Then");
            Assert.True(then.Model.IsBdd);
            Assert.AreEqual(then.Model.BddType.Name, "Then");
        }

        [Test]
        public void WhenIsOfBddTypeWithBddChild()
        {
            var feature = _extent.CreateTest<Feature>(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            var when = scenario.CreateNode<When>("When");
            Assert.True(when.Model.IsBdd);
            Assert.AreEqual(when.Model.BddType.Name, "When");
        }
    }
}
