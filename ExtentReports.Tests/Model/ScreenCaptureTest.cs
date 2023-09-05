using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Model
{
    public class ScreenCaptureTest
    {
        [Test]
        public void InitWithAllEntitiesNull()
        {
            var capture = new ScreenCapture();
            Assert.Null(capture.Base64);
            Assert.Null(capture.Path);
            Assert.Null(capture.ResolvedPath);
            Assert.Null(capture.Title);
        }
    }
}
