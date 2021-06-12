using System.Linq;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Core
{
    public class AppenderTest
    {
        private const string JsonArchive = "extent.json";

        private ExtentReports _extent;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
            var json = new ExtentJsonFormatter(JsonArchive);
            _extent.AttachReporter(json);
        }

        [Test]
        public void TestWithLogs()
        {
            // initial, create archive:
            var test1 = _extent.CreateTest("Testname1", "description1")
                    .Pass("Pass")
                    .Skip("Skip")
                    .Fail("Fail");
            var test2 = _extent.CreateTest("Testname2", "description2")
                    .Warning("Warn")
                    .Info("Info");
            _extent.Flush();

            // post, check archive
            var list = _extent.Report.Tests.ToList();
            Assert.AreEqual(list.Count, 2);
            Assert.AreEqual(list[0].Status, test1.Status);
            Assert.AreEqual(list[1].Status, test2.Status);
            Assert.AreEqual(list[0].Name, test1.Model.Name);
            Assert.AreEqual(list[1].Name, test2.Model.Name);
            Assert.AreEqual(list[0].Description, test1.Model.Description);
            Assert.AreEqual(list[1].Description, test2.Model.Description);
            Assert.AreEqual(list[0].Logs.Count, test1.Model.Logs.Count);
            Assert.AreEqual(list[1].Logs.Count, test2.Model.Logs.Count);
        }

        [Test]
        public void TestWithChildren()
        {
            // initial, create archive:
            var test1 = _extent.CreateTest("Testname1", "description1");
            test1.CreateNode("Child1")
                        .Pass("Pass")
                        .Skip("Skip")
                        .Fail("Fail");
            var test2 = _extent.CreateTest("Testname2", "description2");
            test2.CreateNode("Child2")
                        .Warning("Warn")
                        .Info("Info");
            test2.CreateNode("Child3")
                        .Pass("Pass");
            _extent.Flush();

            // post, check archive
            _extent = new ExtentReports();
            Assert.AreEqual(0, _extent.Report.Tests.Count);
            _extent.CreateDomainFromJsonArchive(JsonArchive);
            var list = _extent.Report.Tests.ToList();

            // parent checks
            Assert.AreEqual(list.Count, 2);
            Assert.AreEqual(list[0].Children.Count, 1);
            Assert.AreEqual(list[1].Children.Count, 2);
            Assert.AreEqual(list[0].Status, test1.Status);
            Assert.AreEqual(list[1].Status, test2.Status);
            Assert.AreEqual(list[0].Name, test1.Model.Name);
            Assert.AreEqual(list[1].Name, test2.Model.Name);
            Assert.AreEqual(list[0].Description, test1.Model.Description);
            Assert.AreEqual(list[1].Description, test2.Model.Description);
            Assert.AreEqual(list[0].Logs.Count, test1.Model.Logs.Count);
            Assert.AreEqual(list[1].Logs.Count, test2.Model.Logs.Count);
        }

        [Test]
        public void Children()
        {
            // initial, create archive:
            ExtentTest test1 = _extent.CreateTest("Testname1", "description1");
            ExtentTest child1 = test1.CreateNode("Child1")
                    .Pass("Pass")
                    .Skip("Skip")
                    .Fail("Fail");
            ExtentTest test2 = _extent.CreateTest("Testname2", "description2");
            ExtentTest child2 = test2.CreateNode("Child2")
                    .Warning("Warn")
                    .Info("Info");
            ExtentTest child3 = test2.CreateNode("Child3")
                    .Pass("Pass");
            _extent.Flush();

            // post, check archive
            _extent = new ExtentReports();
            _extent.CreateDomainFromJsonArchive(JsonArchive);
            var list = _extent.Report.Tests.ToList();

            Assert.IsNotEmpty(list);
            Assert.IsNotEmpty(list[0].Children);

            // children checks
            Assert.AreEqual(list[0].Children.First().Name, child1.Model.Name);
            Assert.AreEqual(list[1].Children.First().Name, child2.Model.Name);
            Assert.AreEqual(list[0].Children.First().Logs.Count, child1.Model.Logs.Count);
            Assert.AreEqual(list[1].Children.First().Logs.Count, child2.Model.Logs.Count);
            list[1].Children.RemoveAt(0);
            Assert.AreEqual(list[1].Children.First().Name, child3.Model.Name);
            Assert.AreEqual(list[1].Children.First().Logs.Count, child3.Model.Logs.Count);
        }

        [Test]
        public void TestWithMedia()
        {
            // initial, create archive:
            var test1 = _extent.CreateTest("Testname1")
                    .AddScreenCaptureFromPath("img.png")
                    .Fail("Fail", MediaEntityBuilder.CreateScreenCaptureFromPath("img.png").Build());
            _extent.Flush();

            // post, check archive
            _extent = new ExtentReports();
            _extent.CreateDomainFromJsonArchive(JsonArchive);
            var list = _extent.Report.Tests.ToList();

            // parent checks
            Assert.AreEqual(list.Count, 1);
            Assert.AreEqual(list[0].Media.Count, 1);
            Assert.NotNull(list[0].Logs.First().Media);
            Assert.AreEqual(list[0].Media[0].Path, test1.Model.Media[0].Path);
            Assert.AreEqual(list[0].Logs.First().Media.Path,
                    test1.Model.Logs.First().Media.Path);
        }

        [Test]
        public void TestWithMediaBase64()
        {
            // initial, create archive:
            var test1 = _extent.CreateTest("Testname1")
                    .AddScreenCaptureFromBase64String("base64string")
                    .Fail("Fail", MediaEntityBuilder.CreateScreenCaptureFromBase64String("base64string").Build());
            _extent.Flush();

            // post, check archive
            _extent = new ExtentReports();
            _extent.CreateDomainFromJsonArchive(JsonArchive);
            var list = _extent.Report.Tests.ToList();

            // parent checks
            Assert.AreEqual(list.Count, 1);
            Assert.AreEqual(list[0].Media.Count, 1);
            Assert.NotNull(list[0].Logs.First().Media);
            Assert.AreEqual(((ScreenCapture)list[0].Media[0]).Base64,
                    ((ScreenCapture)test1.Model.Media[0]).Base64);
            Assert.AreEqual(((ScreenCapture)list[0].Logs.First().Media).Base64,
                    ((ScreenCapture)test1.Model.Logs.First().Media).Base64);
        }
    }
}
