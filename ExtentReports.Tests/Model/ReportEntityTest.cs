using System;
using System.Linq;
using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Model
{
    public class ReportEntityTest
    {
        private ExtentReports _extent;
        private Report _report;
        private Test _test;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
            _report = new Report();
            _test = new Test("Test");
        }

        [Test]
        public void StartAndEndTimesNonNullAtInit()
        {
            Assert.NotNull(_report.StartTime);
            Assert.NotNull(_report.EndTime);
        }

        [Test]
        public void StartIsPassOnInit()
        {
            Assert.AreEqual(Status.Pass, _report.Status);
        }

        [Test]
        public void TestsEmptyOnInit()
        {
            Assert.True(_report.Tests.Count == 0);
        }

        [Test]
        public void StatsNonNullAtInit()
        {
            Assert.NotNull(_report.Stats);
        }

        [Test]
        public void ReportTestListSize()
        {
            Assert.AreEqual(0, _report.Tests.Count);
            _report.Tests.Add(_test);
            Assert.AreEqual(1, _report.Tests.Count);
        }

        [Test]
        public void ReportIsBDD()
        {
            Assert.False(_report.IsBDD);
            _report.AddTest(_test);
            Assert.False(_report.IsBDD);
            _test.BddType = new Gherkin.GherkinKeyword("given");
            Assert.True(_report.IsBDD);
        }

        [Test]
        public void ReportTestHasStatus()
        {
            var skip = new Log(Status.Skip);
            var pass = new Log();

            _report.AddTest(_test);
            Assert.True(_report.AnyTestHasStatus(Status.Pass));
            Assert.False(_report.AnyTestHasStatus(Status.Skip));

            _test.AddLog(skip);
            Assert.False(_report.AnyTestHasStatus(Status.Pass));
            Assert.True(_report.AnyTestHasStatus(Status.Skip));

            _test.AddLog(pass);
            Assert.False(_report.AnyTestHasStatus(Status.Pass));
            Assert.True(_report.AnyTestHasStatus(Status.Skip));
        }

        [Test]
        public void AuthorCtx()
        {
            var context = _report.AuthorCtx;
            Assert.AreEqual(0, context.Context.Count);
            var author = new Author("x");
            _test.Author.Add(author);
            _report.AuthorCtx.AddContext(author, _test);
            Assert.AreEqual(1, context.Context.Count);
            Assert.True(context.Context.Where(x => x.Key.Equals("x")).Any());
            Assert.AreEqual(1, context.Context.Select(x => x.Key.Equals("x")).ToList().Count);
            Assert.AreEqual("Test", context.Context.Where(x => x.Key.Equals("x")).First().Value.Tests.First().Name);
        }

        [Test]
        public void CategoryCtx()
        {
            var context = _report.CategoryCtx;
            Assert.AreEqual(0, context.Context.Count);
            var cat = new Category("x");
            _test.Category.Add(cat);
            _report.CategoryCtx.AddContext(cat, _test);
            Assert.AreEqual(1, context.Context.Count);
            Assert.True(context.Context.Where(x => x.Key.Equals("x")).Any());
            Assert.AreEqual(1, context.Context.Select(x => x.Key.Equals("x")).ToList().Count);
            Assert.AreEqual("Test", context.Context.Where(x => x.Key.Equals("x")).First().Value.Tests.First().Name);
        }

        [Test]
        public void DeviceCtx()
        {
            var context = _report.DeviceCtx;
            Assert.AreEqual(0, context.Context.Count);
            var device = new Device("x");
            _test.Device.Add(device);
            _report.DeviceCtx.AddContext(device, _test);
            Assert.AreEqual(1, context.Context.Count);
            Assert.True(context.Context.Where(x => x.Key.Equals("x")).Any());
            Assert.AreEqual(1, context.Context.Select(x => x.Key.Equals("x")).ToList().Count);
            Assert.AreEqual("Test", context.Context.Where(x => x.Key.Equals("x")).First().Value.Tests.First().Name);
        }

        [Test]
        public void ExceptionContext()
        {
            var msg = "An exception has occurred.";
            var ex = new Exception(msg);
            Assert.AreEqual(0, _report.ExceptionInfoCtx.Context.Count);
            var info = new ExceptionInfo(ex);
            var log = new Log(Status.Fail)
            {
                ExceptionInfo = info
            };
            _test.AddLog(log);
            var device = new Device("x");
            _test.Device.Add(device);
            _report.ExceptionInfoCtx.AddContext(info, _test);
            Assert.AreEqual(1, _report.ExceptionInfoCtx.Context.Count);
            Assert.True(_report.ExceptionInfoCtx.Context.Where(x => x.Key.Equals("System.Exception")).Any());
            Assert.AreEqual(1, _report.ExceptionInfoCtx.Context.Select(x => x.Key.Equals("System.Exception")).ToList().Count);
            Assert.AreEqual("Test", _report.ExceptionInfoCtx.Context.Where(x => x.Key.Equals("System.Exception")).First().Value.Tests.First().Name);
        }

        [Test]
        public void TestRunnerLogs()
        {
            var s = new string[] { "Log 1", "Log 2", "Log 3" };
            Assert.AreEqual(0, _report.Logs.Count);
            s.ToList().ForEach(x => _report.Logs.Enqueue(x));
            Assert.AreEqual(3, _report.Logs.Count);
            s.ToList().ForEach(x => Assert.True(_report.Logs.Contains(x)));
        }

        [Test]
        public void TimeTaken()
        {
            var duration = _report.TimeTaken;
            Assert.True(duration.TotalMilliseconds < 5);
        }
    }
}
