using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Model
{
    public class LogEntityTest
    {
        private Log _log;

        [SetUp]
        public void Setup()
        {
            _log = new Log(Status.Pass);
        }

        [Test]
        public void DefaultStatusBuilder()
        {
            Assert.AreEqual(Status.Pass, _log.Status);
        }

        [Test]
        public void ChangedStatus()
        {
            _log.Status = Status.Fail;
            Assert.AreEqual(Status.Fail, _log.Status);
            _log.Status = Status.Pass;
            Assert.AreEqual(Status.Pass, _log.Status);
        }

        [Test]
        public void TimestampNonNullOnInit()
        {
            Assert.NotNull(_log.Timestamp);
        }

        [Test]
        public void DetailsNullOnInit()
        {
            Assert.Null(_log.Details);
        }

        [Test]
        public void SeqNegOnInit()
        {
            Assert.AreEqual(_log.Seq, -1);
        }

        [Test]
        public void MediaEmptyOnInit()
        {
            Assert.AreEqual(_log.Media, null);
        }

        [Test]
        public void ExceptionsEmptyOnInit()
        {
            Assert.AreEqual(_log.ExceptionInfo, null);
        }

        [Test]
        public void AddMediaDefault()
        {
            Assert.False(_log.HasMedia);
        }

        [Test]
        public void AddMediaWithPathToLog()
        {
            Media m = new ScreenCapture("img.png");
            _log.AddMedia(m);
            Assert.True(_log.HasMedia);
        }

        [Test]
        public void AddMediaWithResolvedPathToLog()
        {
            Media m = new ScreenCapture
            {
                ResolvedPath = "img.png"
            };
            _log.AddMedia(m);
            Assert.True(_log.HasMedia);
        }
    }
}
