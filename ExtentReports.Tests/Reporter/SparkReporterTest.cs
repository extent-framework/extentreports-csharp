using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace AventStack.ExtentReports.Tests.Reporter
{
    public class SparkReporterTest
    {
        private const string FileName = "spark.html";
        private const string Parent = "Parent";
        private const string Child = "Child";
        private const string Grandchild = "Grandchild";
        private const string Scripts = "spark-script.js";
        private const string Stylesheet = "spark-style.css";

        private ExtentReports _extent;
        private ExtentSparkReporter _spark;
        private string _path;

        [SetUp]
        public void Setup()
        {
            _path = DateTime.Now.Millisecond + FileName;
            _extent = new ExtentReports();
            _spark = new ExtentSparkReporter(_path);
            _extent.AttachReporter(_spark);
        }

        private void AssertFileExists(bool delete = true)
        {
            Assert.True(File.Exists(_path));

            if (delete)
            {
                File.Delete(_path);
            }
        }

        [Test]
        public void CreatesReportWithNoTestsInitPath()
        {
            _extent.Flush();
            Assert.False(File.Exists(_path));
        }

        [Test]
        public void ReportContainsTestsAndNodes()
        {
            _extent.CreateTest(Parent)
                    .CreateNode(Child)
                    .CreateNode(Grandchild)
                    .Pass("Pass");
            _extent.Flush();
            AssertFileExists();
            Assert.AreEqual(_spark.Report.Tests.Count, 1);

            var list = _spark.Report.Tests.ToList();
            var children = list[0].Children.ToList();
            var grandchildren = children[0].Children.ToList();

            Assert.AreEqual(list[0].Name, Parent);
            Assert.AreEqual(list[0].Children.Count, 1);
            Assert.AreEqual(children[0].Name, Child);
            Assert.AreEqual(grandchildren.Count, 1);
            Assert.AreEqual(grandchildren[0].Name, Grandchild);
        }

        [Test]
        public void ReportContainsTestsAndNodesTags()
        {
            _extent.CreateTest(Parent).AssignCategory("Tag1")
                    .CreateNode(Child).AssignCategory("Tag2")
                    .CreateNode(Grandchild).AssignCategory("Tag3")
                    .Pass("Pass");
            _extent.Flush();
            AssertFileExists();

            var list = _spark.Report.Tests.ToList();
            var children = list[0].Children.ToList();
            var grandchildren = children[0].Children.ToList();

            Assert.True(list[0].Category.Any(x => x.Name.Equals("Tag1")));
            Assert.True(children[0].Category.Any(x => x.Name.Equals("Tag2")));
            Assert.True(grandchildren[0].Category.Any(x => x.Name.Equals("Tag2")));
        }

        [Test]
        public void ReportContainsTestsAndNodesUsers()
        {
            _extent.CreateTest(Parent).AssignAuthor("Tag1")
                    .CreateNode(Child).AssignAuthor("Tag2")
                    .CreateNode(Grandchild).AssignAuthor("Tag3")
                    .Pass("Pass");
            _extent.Flush();
            AssertFileExists();

            var list = _spark.Report.Tests.ToList();
            var children = list[0].Children.ToList();
            var grandchildren = children[0].Children.ToList();

            Assert.True(list[0].Author.Any(x => x.Name.Equals("Tag1")));
            Assert.True(children[0].Author.Any(x => x.Name.Equals("Tag2")));
            Assert.True(grandchildren[0].Author.Any(x => x.Name.Equals("Tag2")));
        }

        [Test]
        public void ReportContainsTestsAndNodesDevices()
        {
            _extent.CreateTest(Parent).AssignDevice("Tag1")
                    .CreateNode(Child).AssignDevice("Tag2")
                    .CreateNode(Grandchild).AssignDevice("Tag3")
                    .Pass("Pass");
            _extent.Flush();
            AssertFileExists();

            var list = _spark.Report.Tests.ToList();
            var children = list[0].Children.ToList();
            var grandchildren = children[0].Children.ToList();

            Assert.True(list[0].Device.Any(x => x.Name.Equals("Tag1")));
            Assert.True(children[0].Device.Any(x => x.Name.Equals("Tag2")));
            Assert.True(grandchildren[0].Device.Any(x => x.Name.Equals("Tag2")));
        }

        [Test]
        public void SparkOffline()
        {
            _spark.Config.OfflineMode = true;

            _extent.CreateTest(Parent).Pass("Pass");
            _extent.Flush();
            AssertFileExists(false);
            Assert.True(File.Exists("extent/" + Scripts));
            Assert.True(File.Exists("extent/" + Stylesheet));
        }

        [Test]
        public void SparkXMLConfig()
        {
            var xmlConfigPath = Path.Combine(@"../../../../", @"config/spark-config.xml");
            var spark = new ExtentSparkReporter("Spark.html");
            spark.LoadXMLConfig(xmlConfigPath);
            Assert.AreEqual(spark.Config.Theme, Theme.Standard);
            Assert.AreEqual(spark.Config.DocumentTitle, "Extent Framework");
            Assert.AreEqual(spark.Config.Protocol, Protocol.HTTPS);
        }

        [Test]
        public void SparkJSONConfig()
        {
            var jsonConfigPath = Path.Combine(@"../../../../", @"config/spark-config.json");
            var spark = new ExtentSparkReporter("Spark.html");
            spark.LoadJSONConfig(jsonConfigPath);
            Assert.AreEqual(spark.Config.Theme, Theme.Standard);
            Assert.AreEqual(spark.Config.DocumentTitle, "Extent Framework");
            Assert.AreEqual(spark.Config.Protocol, Protocol.HTTPS);
        }

        [Test]
        public void SparkReporterTimestamp()
        {
            var extent = new ExtentReports();
            var spark = new ExtentSparkReporter("Index.html");
            extent.AttachReporter(spark);
            var jsonConfigPath = Path.Combine(@"../../../../", @"config/spark-config.json");
            spark.LoadJSONConfig(jsonConfigPath);

            extent.CreateTest("SparkReporterTimestamp")
                .Pass("Passed");
            extent.Flush();
        }
    }
}
