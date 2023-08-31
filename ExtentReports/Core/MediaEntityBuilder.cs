using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Util;
using System.Threading;

namespace AventStack.ExtentReports
{
    public class MediaEntityBuilder
    {
        private static readonly string Base64Encoded = "data:image/png;base64,";
        private static readonly MediaEntityBuilder _instance = new MediaEntityBuilder();
        private static ThreadLocal<Media> _media;

        public Media Build()
        {
            return _media.Value;
        }

        public static MediaEntityBuilder CreateScreenCaptureFromPath(string path, string title)
        {
            Assert.NotEmpty(path, "ScreenCapture path must not be null or empty");

            _media = new ThreadLocal<Media>
            {
                Value = new ScreenCapture(path, title)
            };
            return _instance;
        }

        public static MediaEntityBuilder CreateScreenCaptureFromPath(string path)
        {
            return CreateScreenCaptureFromPath(path, null);
        }

        public static MediaEntityBuilder CreateScreenCaptureFromBase64String(string base64, string title)
        {
            Assert.NotEmpty(base64, "ScreenCapture's base64 string must not be null or empty");

            if (!base64.StartsWith("data:"))
            {
                base64 = Base64Encoded + base64;
            }

            _media = new ThreadLocal<Media>
            {
                Value = new ScreenCapture(null, title)
                {
                    Base64 = base64
                }
            };

            return _instance;
        }

        public static MediaEntityBuilder CreateScreenCaptureFromBase64String(string base64)
        {
            return CreateScreenCaptureFromBase64String(base64, null);
        }
    }
}
