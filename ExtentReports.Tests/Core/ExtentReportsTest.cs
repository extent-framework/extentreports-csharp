using System.Linq;
using AventStack.ExtentReports.Gherkin;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Core
{
    class ExtentReportsTest
    {
        private const string TestName = "Test";

        private ExtentReports _extent;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
        }

        [TearDown]
        public void TearDown()
        {
            _extent.GherkinDialect = "en";
        }

        [Test]
        public void CreateTestOverloadTypeNameDesc()
        {
            var test = _extent.CreateTest<Feature>(TestName, "Description");
            var model = test.Model;
            Assert.True(model.IsBdd);
            Assert.AreEqual(model.Name, TestName);
            Assert.AreEqual(model.Description, "Description");
            Assert.AreEqual(model.BddType.Name, "Feature");
            Assert.True(model.Leaf);
        }

        [Test]
        public void CreateTestOverloadTypeName()
        {
            var test = _extent.CreateTest<Feature>(TestName);
            var model = test.Model;
            Assert.True(model.IsBdd);
            Assert.AreEqual(model.Name, TestName);
            Assert.IsEmpty(model.Description);
            Assert.AreEqual(model.BddType.Name, "Feature");
            Assert.True(model.Leaf);
        }

        [Test]
        public void CreateTestOverloadKeywordNameDesc()
        {
            var test = _extent.CreateTest(new GherkinKeyword("Feature"), TestName, "Description");
            var model = test.Model;
            Assert.True(model.IsBdd);
            Assert.AreEqual(model.Name, TestName);
            Assert.AreEqual(model.Description, "Description");
            Assert.AreEqual(model.BddType.Name, "Feature");
            Assert.True(model.Leaf);
        }

        [Test]
        public void CreateTestOverloadKeywordName()
        {
            var test = _extent.CreateTest(new GherkinKeyword("Feature"), TestName);
            var model = test.Model;
            Assert.True(model.IsBdd);
            Assert.AreEqual(model.Name, TestName);
            Assert.IsEmpty(model.Description);
            Assert.AreEqual(model.BddType.Name, "Feature");
            Assert.True(model.Leaf);
        }

        [Test]
        public void CreateTestOverloadNameDesc()
        {
            var test = _extent.CreateTest(TestName, "Description");
            var model = test.Model;
            Assert.False(model.IsBdd);
            Assert.AreEqual(model.Name, TestName);
            Assert.AreEqual(model.Description, "Description");
            Assert.Null(model.BddType);
            Assert.True(model.Leaf);
        }

        [Test]
        public void CreateTestOverloadName()
        {
            var test = _extent.CreateTest(TestName);
            var model = test.Model;
            Assert.False(model.IsBdd);
            Assert.AreEqual(model.Name, TestName);
            Assert.IsEmpty(model.Description);
            Assert.Null(model.BddType);
            Assert.True(model.Leaf);
        }

        [Test]
        public void GherkinDialect()
        {
            _extent.GherkinDialect = "de";
            Assert.AreEqual(GherkinDialectProvider.Lang, "de");
        }

        [Test]
        public void AddTestRunnerOutputSingle()
        {
            var logs = new string[] { "Log1", "Log2" };
            var list = logs.ToList();
            list.ForEach(x => _extent.AddTestRunnerLogs(x));
            Assert.AreEqual(2, _extent.Report.Logs.Count);
            list.ForEach(x => Assert.True(_extent.Report.Logs.Contains(x)));
        }

        [Test]
        public void AddTestRunnerOutputArr()
        {
            var logs = new string[] { "Log1", "Log2" };
            _extent.AddTestRunnerLogs(logs);
            Assert.AreEqual(2, _extent.Report.Logs.Count);
            logs.ToList().ForEach(x => Assert.True(_extent.Report.Logs.Contains(x)));
        }

        [Test]
        public void AddTestRunnerOutputList()
        {
            var logs = new string[] { "Log1", "Log2" };
            var list = logs.ToList();
            _extent.AddTestRunnerLogs(list);
            Assert.AreEqual(2, _extent.Report.Logs.Count);
            list.ForEach(x => Assert.True(_extent.Report.Logs.Contains(x)));
        }
    }
}
