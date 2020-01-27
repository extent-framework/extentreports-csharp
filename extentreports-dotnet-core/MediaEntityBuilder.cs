using System.IO;
using System.Threading;

using AventStack.ExtentReports.Model;

namespace AventStack.ExtentReports
{
    public class MediaEntityBuilder
    {
        private static readonly MediaEntityBuilder _instance = new MediaEntityBuilder();
        private static ThreadLocal<Media> _media;

        public MediaEntityModelProvider Build()
        {
            return new MediaEntityModelProvider(_media.Value);
        }

        /// <summary>
        /// Adds a snapshot to the test or log with title
        /// </summary>
        /// <param name="path">Image path</param>
        /// <param name="title">Image title</param>
        /// <returns>A <see cref="MediaEntityBuilder"/> object</returns>
        public static MediaEntityBuilder CreateScreenCaptureFromPath(string path, string title = null)
        {
            if (string.IsNullOrEmpty(path))
                throw new IOException("ScreenCapture path cannot be null or empty.");

            return CreateScreenCapture(path, title, false);
        }

        /// <summary>
        /// Adds a base64 snapshot to the test or log with title
        /// </summary>
        /// <param name="base64String">A Base64 string</param>
        /// <param name="title">Image title</param>
        /// <returns>A <see cref="MediaEntityBuilder"/> object</returns>
        public static MediaEntityBuilder CreateScreenCaptureFromBase64String(string base64String, string title = null)
        {
            if (string.IsNullOrEmpty(base64String))
                throw new IOException("Base64 string cannot be null or empty.");

            return CreateScreenCapture(base64String, null, true);
        }

        private static MediaEntityBuilder CreateScreenCapture(string pathOrBase64String, string title, bool isBase64)
        {
            var sc = new ScreenCapture
            {
                Title = title
            };

            if (isBase64)
                sc.Base64String = pathOrBase64String;
            else
                sc.Path = pathOrBase64String;

            _media = new ThreadLocal<Media>
            {
                Value = sc
            };

            return _instance;
        }

    }
}