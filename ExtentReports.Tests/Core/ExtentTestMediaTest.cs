using System;
using System.Collections.Generic;
using System.Linq;
using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Core
{
    public class ExtentTestMediaTest
    {
        private const string Base64Encoded = "data:image/png;base64,";
        private const string Base64 = "iVBORw0KGgoAAAANSUhEUgAAAY4AAABbCAYAAABkgGJUAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAABIMSURBVHhe7Z3"
                + "daxRXH8effyZ3gVwIXgiFemXwookXkUIINFQQGrx4clV9LpILadCLEOzjWm0U00ZMQ7SQUEuKbSw1BoKkEdnqI1tss3lZ1rh1a9JsTMLvOWfmnJmzu/NyzuyLO/H7gUObcWfmzMzu7zPn/V8EAAAA"
                + "GABxAAAAMALiAAAAYATEAQAAwAiIAwAAgBEQBwAAACMgDgAAAEZAHAAAAIyAOAAAABgBcQAAADAC4gAAAGAExAEAAMAIiAMAAIAREAcAAAAjIA4AAABGQBwAAACMgDgAAAAYAXEAAAAwAuIAAABgB";
        private const string Path = "src/test/resources/img.png";
        private const string Title = "MediaTitle";

        private ExtentReports _extent;
        private ExtentTest _test;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
            _test = _extent.CreateTest("Test");
        }

        [Test]
        public void AddScreenCaptureFromEmptyPathTest()
        {
            Assert.Throws(typeof(ArgumentException), () => _test.AddScreenCaptureFromPath(""));
        }

        [Test]
        public void AddScreenCaptureFromNullPathTest()
        {
            Assert.Throws(typeof(ArgumentException), () => _test.AddScreenCaptureFromPath(null));
        }

        [Test]
        public void AddScreenCaptureFromPathTest()
        {
            _test.AddScreenCaptureFromPath(Path, Title).Pass("Pass");
            Assert.AreEqual(_test.Test.Media.Count, 1);
            Assert.AreEqual(_test.Test.Media[0].Path, Path);
            Assert.AreEqual(_test.Test.Media[0].Title, Title);
        }

        [Test]
        public void AddScreenCaptureFromPathTestOverloads()
        {
            _test
                    .AddScreenCaptureFromPath(Path)
                    .Pass("Pass");
            Assert.AreEqual(_test.Test.Media.Count, 1);
            Assert.AreEqual(_test.Test.Media[0].Path, Path);
        }

        [Test]
        public void AddScreenCaptureFromPathNode()
        {
            ExtentTest node = _test
                    .CreateNode("Node")
                    .AddScreenCaptureFromPath(Path, Title)
                    .Pass("Pass");
            Assert.AreEqual(_test.Test.Media.Count, 0);
            Assert.AreEqual(node.Test.Media.Count, 1);
            Assert.AreEqual(node.Test.Media[0].Path, Path);
            Assert.AreEqual(node.Test.Media[0].Title, Title);
        }

        [Test]
        public void AddScreenCaptureEmptyPathTestLog()
        {
            Assert.Throws(typeof(ArgumentException), () => _test.Pass("Pass", MediaEntityBuilder.CreateScreenCaptureFromPath("").Build()));
        }

        [Test]
        public void AddScreenCaptureNullPathTestLog()
        {
            Assert.Throws(typeof(ArgumentException), () => _test.Pass("Pass", MediaEntityBuilder.CreateScreenCaptureFromPath(null).Build()));
        }

        [Test]
        public void AddScreenCaptureFromPathTestLog()
        {
            _test.Pass("Pass", MediaEntityBuilder.CreateScreenCaptureFromPath(Path, Title).Build());
            Assert.AreEqual(_test.Test.Media.Count, 0);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.NotNull(logs[0].Media);
            Assert.AreEqual(logs[0].Media.Path, Path);
            Assert.AreEqual(logs[0].Media.Title, Title);
        }

        [Test]
        public void AddScreenCaptureFromPathTestLogOverloads()
        {
            _test.Pass("Pass", MediaEntityBuilder.CreateScreenCaptureFromPath(Path).Build());
            Assert.AreEqual(_test.Test.Media.Count, 0);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.NotNull(logs[0].Media);
            Assert.AreEqual(logs[0].Media.Path, Path);
        }

        [Test]
        public void AddScreenCaptureFromPathNodeLog()
        {
            ExtentTest node = _test
                    .CreateNode("Node")
                    .Pass("Pass", MediaEntityBuilder.CreateScreenCaptureFromPath(Path, Title).Build());
            Assert.AreEqual(node.Test.Media.Count, 0);
            var logs = new List<Log>(node.Test.Logs);
            Assert.NotNull(logs[0].Media);
            Assert.AreEqual(logs[0].Media.Path, Path);
            Assert.AreEqual(logs[0].Media.Title, Title);
        }

        [Test]
        public void AddScreenCaptureFromEmptyBase64Test()
        {
            Assert.Throws(typeof(ArgumentException), () => _test.AddScreenCaptureFromBase64String(""));
        }

        [Test]
        public void AddScreenCaptureFromNullBase64Test()
        {
            Assert.Throws(typeof(ArgumentException), () => _test.AddScreenCaptureFromBase64String(null));
        }

        [Test]
        public void AddScreenCaptureFromBase64Test()
        {
            _test.AddScreenCaptureFromBase64String(Base64, Title)
                    .Pass("Pass");
            Assert.AreEqual(_test.Test.Media.Count, 1);
            Assert.AreEqual(((ScreenCapture)_test.Test.Media[0]).Base64, Base64Encoded + Base64);
            Assert.AreEqual(_test.Test.Media[0].Title, Title);
        }

        [Test]
        public void AddScreenCaptureFromBase64Node()
        {
            var node = _test.CreateNode("Node")
                    .AddScreenCaptureFromBase64String(Base64, Title)
                    .Pass("Pass");
            Assert.AreEqual(_test.Test.Media.Count, 0);
            Assert.AreEqual(node.Test.Media.Count, 1);
            Assert.AreEqual(((ScreenCapture)node.Test.Media[0]).Base64, Base64Encoded + Base64);
            Assert.AreEqual(node.Test.Media[0].Title, Title);
        }

        [Test]
        public void AddScreenCaptureFromBase64NodeOverloads()
        {
            var node = _test.CreateNode("Node")
                    .AddScreenCaptureFromBase64String(Base64)
                    .Pass("Pass");
            Assert.AreEqual(_test.Test.Media.Count, 0);
            Assert.AreEqual(node.Test.Media.Count, 1);
            Assert.AreEqual(((ScreenCapture)node.Test.Media[0]).Base64, Base64Encoded + Base64);
        }

        [Test]
        public void AddScreenCaptureFromBase64TestLog()
        {
            _test.Pass("Pass", MediaEntityBuilder.CreateScreenCaptureFromBase64String(Base64, Title).Build());
            Assert.AreEqual(_test.Test.Media.Count, 0);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.NotNull(logs[0].Media);
            Assert.AreEqual(((ScreenCapture)logs[0].Media).Base64,
                    Base64Encoded + Base64);
            Assert.AreEqual(logs[0].Media.Title, Title);
        }

        [Test]
        public void AddScreenCaptureFromBase64NodeLog()
        {
            var node = _test.CreateNode("Node")
                    .Pass("Pass", MediaEntityBuilder.CreateScreenCaptureFromBase64String(Base64, Title).Build());
            Assert.AreEqual(node.Test.Media.Count, 0);
            var logs = new List<Log>(node.Test.Logs);
            Assert.NotNull(logs[0].Media);
            Assert.AreEqual(((ScreenCapture)logs[0].Media).Base64,
                    Base64Encoded + Base64);
            Assert.AreEqual(logs[0].Media.Title, Title);
        }

        [Test]
        public void AddScreenCaptureFromBase64NodeLogOverloads()
        {
            var node = _test.CreateNode("Node")
                    .Pass("Pass", MediaEntityBuilder.CreateScreenCaptureFromBase64String(Base64).Build());
            Assert.AreEqual(node.Test.Media.Count, 0);
            var logs = new List<Log>(node.Test.Logs);
            Assert.NotNull(logs[0].Media);
            Assert.AreEqual(((ScreenCapture)logs[0].Media).Base64,
                    Base64Encoded + Base64);
        }
    }
}
