using AventStack.ExtentReports.Gherkin.Model;

using NUnit.Framework;

using System;

namespace AventStack.ExtentReports.Tests.APITests
{
    [TestFixture]
    public class TestInitializeWithNullValues : Base
    {
        [Test]
        public void TestNameNull()
        {
            Assert.Throws<ArgumentException>(() => testNameNull());
        }

        private void testNameNull()
        {
            _extent.CreateTest(null).Pass("Pass");
        }

        [Test]
        public void BddTestScenarioNameNull()
        {
            Assert.Throws<ArgumentException>(() => bddTestScenarioNameNull());
        }

        private void bddTestScenarioNameNull()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            feature.CreateNode<Scenario>(null);
        }
            
        [Test]
        public void BddTestScenarioNameEmpty()
        {
            Assert.Throws<ArgumentException>(() => bddTestScenarioNameEmpty());
        }

        private void bddTestScenarioNameEmpty()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            feature.CreateNode<Scenario>("");
        }

        [Test]
        public void BddTestStepNameNull()
        {
            Assert.Throws<ArgumentException>(() => bddTestStepNameNull());
        }

        private void bddTestStepNameNull()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            scenario.CreateNode<Given>(null);
        }

        [Test]
        public void BddTestStepNameEmpty()
        {
            Assert.Throws<ArgumentException>(() => bddTestStepNameEmpty());
        }

        private void bddTestStepNameEmpty()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            scenario.CreateNode<Given>("");
        }

        [Test]
        public void BddTestScenarioNameNullGherkinKeyword()
        {
            Assert.Throws<ArgumentException>(() => bddTestScenarioNameNullGherkinKeyword());
        }

        private void bddTestScenarioNameNullGherkinKeyword()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            feature.CreateNode(new GherkinKeyword("Scenario"), null);
        }

        [Test]
        public void BddTestScenarioNameEmptyGherkinKeyword()
        {
            Assert.Throws<ArgumentException>(() => bddTestScenarioNameEmptyGherkinKeyword());
        }
        
        private void bddTestScenarioNameEmptyGherkinKeyword()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            feature.CreateNode(new GherkinKeyword("Scenario"), "");
        }

        [Test]
        public void BddTestStepNameNullGherkinKeyword()
        {
            Assert.Throws<ArgumentException>(() => bddTestStepNameNullGherkinKeyword());
        }

        private void bddTestStepNameNullGherkinKeyword()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            scenario.CreateNode(new GherkinKeyword("Given"), null);
        }

        [Test]
        public void BddTestStepNameEmptyGherkinKeyword()
        {
            Assert.Throws<ArgumentException>(() => bddTestStepNameEmptyGherkinKeyword());
        }

        private void bddTestStepNameEmptyGherkinKeyword()
        {
            var feature = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            var scenario = feature.CreateNode<Scenario>("Scenario");
            scenario.CreateNode(new GherkinKeyword("Given"), "");
        }

        [Test]
        public void TestNameEmpty()
        {
            Assert.Throws<ArgumentException>(() => testNameEmpty());
        }

        private void testNameEmpty()
        {
            _extent.CreateTest("").Pass("Pass");
        }

        [Test]
        public void NodeNameNull()
        {
            Assert.Throws<ArgumentException>(() => nodeNameNull());
        }
        
        private void nodeNameNull()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name).Fail("fail");
            var node = test.CreateNode(null);

            Assert.Equals(test.Model.NodeContext.Count, 0);
            Assert.Null(node);
        }

        [Test]
        public void NodeNameEmpty()
        {
            Assert.Throws<ArgumentException>(() => nodeNameEmpty());
        }

        private void nodeNameEmpty()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name).Fail("fail");
            var node = test.CreateNode("");

            Assert.Equals(test.Model.NodeContext.Count, 0);
            Assert.Null(node);
        }

        [Test]
        public void TestDescriptionNull()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name, null).Pass("pass");

            Assert.True(string.IsNullOrEmpty(test.Model.Description));
        }

        [Test]
        public void nodeDescriptionNull()
        {
            var node = _extent.CreateTest(TestContext.CurrentContext.Test.Name).CreateNode("Child", null).Pass("pass");

            Assert.True(string.IsNullOrEmpty(node.Model.Description));
        }
    }
}
