using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Model;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;

namespace AventStack.ExtentReports.Tests.Core
{
    class ExtentTestLogsTest
    {
        private const string Details = "details";
        private const string Attachment = "img.png";

        private const string TestName = "Test";

        private ExtentReports _extent;
        private ExtentTest _test;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
            _test = _extent.CreateTest(TestName);
        }

        [Test]
        public void LogDetails()
        {
            _test.Log(Status.Skip, Details + "1");
            Assert.AreEqual(_test.Test.Status, Status.Skip);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].Details, Details + "1");
            Assert.AreEqual(logs[0].Status, Status.Skip);
            _test.Log(Status.Fail, Details);
            logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[1].Details, Details);
            Assert.AreEqual(logs[1].Status, Status.Fail);
        }

        [Test]
        public void LogDetailsMedia()
        {
            _test.Log(Status.Skip, Details,
                    MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(_test.Test.Status, Status.Skip);
            Assert.AreEqual(logs[0].Details, Details);
            Assert.AreEqual(logs[0].Status, Status.Skip);
            Assert.AreEqual(logs[0].Media.Path, Attachment);
            _test.Log(Status.Fail, Details, MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[1].Details, Details);
            Assert.AreEqual(logs[1].Status, Status.Fail);
            Assert.AreEqual(logs[1].Media.Path, Attachment);
        }

        [Test]
        public void LogMedia()
        {
            var test = _test.Log(Status.Skip,
                    MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            Assert.AreEqual(_test.Test.Status, Status.Skip);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].Details, "");
            Assert.AreEqual(logs[0].Status, Status.Skip);
            Assert.AreEqual(logs[0].Media.Path, Attachment);
            _test.Log(Status.Fail, MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[1].Details, "");
            Assert.AreEqual(logs[1].Status, Status.Fail);
            Assert.AreEqual(logs[1].Media.Path, Attachment);
        }

        [Test]
        public void LogMarkup()
        {
            var m = MarkupHelper.CreateCodeBlock("code");
            _test.Log(Status.Skip, m);
            Assert.AreEqual(_test.Test.Status, Status.Skip);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.True(logs[0].Details.Contains("code"));
            Assert.AreEqual(logs[0].Status, Status.Skip);
        }

        [Test]
        public void LogThrowable()
        {
            var ex = new Exception("Exception");
            _test.Log(Status.Skip, ex);
            Assert.AreEqual(_test.Test.Status, Status.Skip);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].ExceptionInfo.Exception, ex);
            Assert.AreEqual(logs[0].Status, Status.Skip);
            _test.Log(Status.Fail, ex);
            logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[1].ExceptionInfo.Exception, ex);
            Assert.AreEqual(logs[1].Status, Status.Fail);
        }

        [Test]
        public void LogThrowableMedia()
        {
            var ex = new Exception("Exception");
            var test = _test.Log(Status.Skip, ex,
                    MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            Assert.AreEqual(_test.Test.Status, Status.Skip);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].ExceptionInfo.Exception, ex);
            Assert.AreEqual(logs[0].Status, Status.Skip);
            Assert.AreEqual(logs[0].Media.Path, Attachment);
            _test.Log(Status.Fail, ex, MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[1].ExceptionInfo.Exception, ex);
            Assert.AreEqual(logs[1].Status, Status.Fail);
            Assert.AreEqual(logs[1].Media.Path, Attachment);
        }

        [Test]
        public void FailDetails()
        {
            _test.Fail(Details);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].Details, Details);
            Assert.AreEqual(logs[0].Status, Status.Fail);
        }

        [Test]
        public void FailMedia()
        {
            _test.Fail(
                    MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            _test.Log(Status.Fail, Details, MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].Status, Status.Fail);
            Assert.AreEqual(logs[0].Media.Path, Attachment);
        }

        [Test]
        public void FailThrowable()
        {
            var ex = new Exception("Exception");
            _test.Fail(ex);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].ExceptionInfo.Exception, ex);
            Assert.AreEqual(logs[0].Status, Status.Fail);
        }

        [Test]
        public void FailThrowableMedia()
        {
            var ex = new Exception("Exception");
            _test.Fail(ex,
                    MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].ExceptionInfo.Exception, ex);
            Assert.AreEqual(logs[0].Status, Status.Fail);
            Assert.AreEqual(logs[0].Media.Path, Attachment);
        }

        [Test]
        public void FailMarkup()
        {
            var m = MarkupHelper.CreateCodeBlock("code");
            _test.Fail(m);
            Assert.AreEqual(_test.Test.Status, Status.Fail);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.True(logs[0].Details.Contains("code"));
            Assert.AreEqual(logs[0].Status, Status.Fail);
        }

        [Test]
        public void SkipDetails()
        {
            _test.Skip(Details);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].Details, Details);
            Assert.AreEqual(logs[0].Status, Status.Skip);
        }

        [Test]
        public void SkipMedia()
        {
            _test.Skip(
                    MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            _test.Log(Status.Fail, Details, MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].Status, Status.Skip);
            Assert.AreEqual(logs[0].Media.Path, Attachment);
        }

        [Test]
        public void SkipThrowable()
        {
            var ex = new Exception("Exception");
            _test.Skip(ex);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].ExceptionInfo.Exception, ex);
            Assert.AreEqual(logs[0].Status, Status.Skip);
        }

        [Test]
        public void SkipThrowableMedia()
        {
            var ex = new Exception("Exception");
            _test.Skip(ex,
                    MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].ExceptionInfo.Exception, ex);
            Assert.AreEqual(logs[0].Status, Status.Skip);
            Assert.AreEqual(logs[0].Media.Path, Attachment);
        }

        [Test]
        public void SkipMarkup()
        {
            var m = MarkupHelper.CreateCodeBlock("code");
            _test.Log(Status.Skip, m);
            Assert.AreEqual(_test.Test.Status, Status.Skip);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.True(logs[0].Details.Contains("code"));
            Assert.AreEqual(logs[0].Status, Status.Skip);
        }

        [Test]
        public void WarnDetails()
        {
            _test.Warning(Details);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].Details, Details);
            Assert.AreEqual(logs[0].Status, Status.Warning);
        }

        [Test]
        public void WarnMedia()
        {
            _test.Warning(
                    MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            _test.Log(Status.Warning, Details, MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].Status, Status.Warning);
            Assert.AreEqual(logs[0].Media.Path, Attachment);
        }

        [Test]
        public void WarnThrowable()
        {
            var ex = new Exception("Exception");
            _test.Warning(ex);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].ExceptionInfo.Exception, ex);
            Assert.AreEqual(logs[0].Status, Status.Warning);
        }

        [Test]
        public void WarnThrowableMedia()
        {
            var ex = new Exception("Exception");
            _test.Log(Status.Warning, ex,
                    MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].ExceptionInfo.Exception, ex);
            Assert.AreEqual(logs[0].Status, Status.Warning);
            Assert.AreEqual(logs[0].Media.Path, Attachment);
        }

        [Test]
        public void WarnMarkup()
        {
            var m = MarkupHelper.CreateCodeBlock("code");
            _test.Log(Status.Warning, m);
            Assert.AreEqual(_test.Test.Status, Status.Warning);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.True(logs[0].Details.Contains("code"));
            Assert.AreEqual(logs[0].Status, Status.Warning);
        }

        [Test]
        public void PassDetails()
        {
            _test.Pass(Details);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].Details, Details);
            Assert.AreEqual(logs[0].Status, Status.Pass);
        }

        [Test]
        public void PassMedia()
        {
            _test.Pass(
                    MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            _test.Log(Status.Pass, Details, MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].Status, Status.Pass);
            Assert.AreEqual(logs[0].Media.Path, Attachment);
        }

        [Test]
        public void PassThrowable()
        {
            var ex = new Exception("Exception");
            _test.Pass(ex);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].ExceptionInfo.Exception, ex);
            Assert.AreEqual(logs[0].Status, Status.Pass);
        }

        [Test]
        public void PassThrowableMedia()
        {
            var ex = new Exception("Exception");
            _test.Pass(ex,
                    MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].ExceptionInfo.Exception, ex);
            Assert.AreEqual(logs[0].Status, Status.Pass);
            Assert.AreEqual(logs[0].Media.Path, Attachment);
        }

        [Test]
        public void PassMarkup()
        {
            var m = MarkupHelper.CreateCodeBlock("code");
            _test.Log(Status.Pass, m);
            Assert.AreEqual(_test.Test.Status, Status.Pass);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.True(logs[0].Details.Contains("code"));
            Assert.AreEqual(logs[0].Status, Status.Pass);
        }

        [Test]
        public void InfoDetails()
        {
            _test.Info(Details);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].Details, Details);
            Assert.AreEqual(logs[0].Status, Status.Info);
        }

        [Test]
        public void InfoMedia()
        {
            _test.Info(
                    MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            _test.Log(Status.Info, Details, MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].Status, Status.Info);
            Assert.AreEqual(logs[0].Media.Path, Attachment);
        }

        [Test]
        public void InfoThrowable()
        {
            var ex = new Exception("Exception");
            _test.Info(ex);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].ExceptionInfo.Exception, ex);
            Assert.AreEqual(logs[0].Status, Status.Info);
        }

        [Test]
        public void InfoThrowableMedia()
        {
            var ex = new Exception("Exception");
            _test.Info(ex,
                    MediaEntityBuilder.CreateScreenCaptureFromPath(Attachment).Build());
            var logs = new List<Log>(_test.Test.Logs);
            Assert.AreEqual(logs[0].ExceptionInfo.Exception, ex);
            Assert.AreEqual(logs[0].Status, Status.Info);
            Assert.AreEqual(logs[0].Media.Path, Attachment);
        }

        [Test]
        public void InfoMarkup()
        {
            var m = MarkupHelper.CreateCodeBlock("code");
            _test.Log(Status.Info, m);
            Assert.AreEqual(_test.Test.Status, Status.Pass);
            var logs = new List<Log>(_test.Test.Logs);
            Assert.True(logs[0].Details.Contains("code"));
            Assert.AreEqual(logs[0].Status, Status.Info);
        }
    }
}
