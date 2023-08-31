using AventStack.ExtentReports.Gherkin.Model;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Threading;

namespace AventStack.ExtentReports.Tests.Core
{
    class ExtentTestBDDNodeTest
    {
        private const string TestName = "Test";
        private const string NodeName = "Node";
        private const string Description = "Description";

        private ExtentReports _extent;
        private ExtentTest _test;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
            _test = _extent.CreateTest<Feature>(TestName);
        }

        [Test]
        public void CreateNodeNullName()
        {
            Assert.Throws(typeof(ArgumentException), () => _test.CreateNode<Scenario>(null));
        }

        [Test]
        public void CreateNodeName()
        {
            var node = _test.CreateNode<Scenario>(NodeName);
            var model = node.Test;
            Assert.NotNull(model.BddType);
            Assert.AreEqual(model.BddType.Name, "Scenario");
            Assert.AreEqual(NodeName, model.Name);
            Assert.IsEmpty(model.Description);
        }

        [Test]
        public void CreateNodeNameDesc()
        {
            var node = _test.CreateNode<Scenario>(NodeName, Description);
            var model = node.Test;
            Assert.NotNull(model.BddType);
            Assert.AreEqual(model.BddType.Name, "Scenario");
            Assert.AreEqual(NodeName, model.Name);
            Assert.AreEqual(Description, model.Description);
        }

        [Test]
        public void NodeTimeInit()
        {
            var node = _test.CreateNode<Scenario>(NodeName);
            var model = node.Test;
            Assert.True(model.TimeTaken <= 1);
        }

        [Test]
        public void NodeTimeElapsed()
        {
            var node = _test.CreateNode<Scenario>(NodeName);
            var model = node.Test;
            Thread.Sleep(100);
            node.Pass("");
            Assert.True(model.TimeTaken >= 100);
        }
    }
}
