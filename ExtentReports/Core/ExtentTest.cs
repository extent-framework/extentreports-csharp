using AventStack.ExtentReports.Gherkin;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Util;
using System;
using System.Linq;

namespace AventStack.ExtentReports
{
    public class ExtentTest
    {
        public Status Status => Model.Status;
        public Test Model { get; private set; }
        public ExtentReports Extent { get; private set; }

        internal ExtentTest(ExtentReports extent, GherkinKeyword bddType, string name, string description)
        {
            Assert.NotEmpty(name, "Test name must not be null or empty");

            Extent = extent;

            Model = new Test
            {
                Name = name,
                BddType = bddType,
                Description = description,
                UseNaturalConf = extent.UsingNaturalConf
            };

            Extent = extent;
        }

        internal ExtentTest(ExtentReports extent, string name, string description = "")
            : this(extent, null, name, description)
        { }

        public ExtentTest CreateNode(GherkinKeyword keyword, string name, string description = "")
        {
            Assert.NotEmpty(name, "Test name must not be null or empty");

            var t = new ExtentTest(Extent, keyword, name, description);
            Notify(t.Model);
            return t;
        }

        private void Notify(Test t)
        {
            Model.AddChild(t);
            Extent.OnNodeCreated(t);
        }

        public ExtentTest CreateNode<T>(string name, string description = "") where T : IGherkinFormatterModel
        {
            var type = typeof(T).Name;
            var keyword = new GherkinKeyword(type);
            return CreateNode(keyword, name, description);
        }

        public ExtentTest CreateNode(string name, string description = "")
        {
            var t = new ExtentTest(Extent, name, description);
            Notify(t.Model);
            return t;
        }

        /// <summary>
        /// Removes a direct child of the current test
        /// </summary>
        /// <param name="test">The child node to be removed</param>
        public void RemoveNode(ExtentTest test)
        {
            Extent.RemoveTest(test);
        }

        /// <summary>
        /// Create a non-standard log with details. This is unlike the <code>Log</code>
        /// method which creates a fixed table layout with the following columns:
        /// Timestamp, Status, Details. 
        /// 
        /// <code>GenerateLog</code> with <see cref="IMarkup"/> allows for a user-defined
        /// log with a supported <see cref="IMarkup"/> provided by <see cref="MarkupHelper"/>
        /// </summary>
        /// <param name="status"><see cref="Status"/></param>
        /// <param name="details">Test details of the step</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest GenerateLog(Status status, string details)
        {
            var evt = new Log
            {
                Status = status,
                Details = details ?? ""
            };

            Model.AddGeneratedLog(evt);
            Extent.OnLogCreated(evt, Model);

            return this;
        }

        /// <summary>
        /// Create a non-standard log with details. This is unlike the <code>Log</code>
        /// method which creates a fixed table layout with the following columns:
        /// Timestamp, Status, Details. 
        /// 
        /// <code>GenerateLog</code> with <see cref="IMarkup"/> allows for a user-defined
        /// log with a supported <see cref="IMarkup"/> provided by <see cref="MarkupHelper"/>
        /// </summary>
        /// <param name="status"><see cref="Status"/></param>
        /// <param name="markup"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest GenerateLog(Status status, IMarkup markup)
        {
            return GenerateLog(status, markup.GetMarkup());
        }

        /// <summary>
        /// Logs an event with <see cref="Status"/>, details and a media object: <see cref="ScreenCapture"/>
        /// 
        /// <code>
        /// test.Log(Status.Fail, "details", MediaEntityBuilder.CreateScreenCaptureFromPath("screen.png").Build());
        /// </code>
        /// </summary>
        /// 
        /// <param name="status"><see cref="Status"/></param>
        /// <param name="details">Log details</param>
        /// <param name="media">A <see cref="Media"/> object</param>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Log(Status status, string details, Exception ex, Media media)
        {
            Assert.NotNull(status, "Status must not be null");

            var evt = new Log
            {
                Status = status,
                Details = details ?? ""
            };

            if (ex != null)
            {
                var ei = new ExceptionInfo(ex);
                evt.ExceptionInfo = ei;
                Model.ExceptionInfo.Add(ei);
            }

            Model.AddLog(evt);
            Extent.OnLogCreated(evt, Model);

            if (media != null)
            {
                evt.AddMedia(media);
                Extent.OnMediaAdded(media, evt, Model);
            }

            return this;
        }

        /// <summary>
        /// Logs an event with <see cref="Status"/>, details and a media object: <see cref="ScreenCapture"/>
        /// 
        /// <code>
        /// test.Log(Status.Fail, "details", MediaEntityBuilder.CreateScreenCaptureFromPath("screen.png").Build());
        /// </code>
        /// </summary>
        /// 
        /// <param name="status"><see cref="Status"/></param>
        /// <param name="details">Log details</param>
        /// <param name="media">A <see cref="Media"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Log(Status status, string details, Media media)
        {
            return Log(status, details, null, media);
        }

        /// <summary>
        /// Logs an event with <see cref="Status"/>, details and a media object: <see cref="ScreenCapture"/>
        /// 
        /// <code>
        /// test.Log(Status.Fail, "details", MediaEntityBuilder.CreateScreenCaptureFromPath("screen.png").Build());
        /// </code>
        /// </summary>
        /// 
        /// <param name="status"><see cref="Status"/></param>
        /// <param name="media">A <see cref="Media"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Log(Status status, Media media)
        {
            return Log(status, null, null, media);
        }

        /// <summary>
        /// Logs an event with <see cref="Status"/>, details and a media object: <see cref="ScreenCapture"/>
        /// 
        /// <code>
        /// test.Log(Status.Fail, "details", MediaEntityBuilder.CreateScreenCaptureFromPath("screen.png").Build());
        /// </code>
        /// </summary>
        /// 
        /// <param name="status"><see cref="Status"/></param>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="media">A <see cref="Media"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Log(Status status, Exception ex, Media media)
        {
            return Log(status, "", ex, media);
        }

        /// <summary>
        /// Logs an event with <see cref="Status"/>, details and a media object: <see cref="ScreenCapture"/>
        /// 
        /// <code>
        /// test.Log(Status.Fail, "details", MediaEntityBuilder.CreateScreenCaptureFromPath("screen.png").Build());
        /// </code>
        /// </summary>
        /// 
        /// <param name="status"><see cref="Status"/></param>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Log(Status status, Exception ex)
        {
            return Log(status, null, ex, null);
        }

        /// <summary>
        /// Logs an event with <see cref="Status"/>, details and a media object: <see cref="ScreenCapture"/>
        /// 
        /// <code>
        /// test.Log(Status.Fail, "details", MediaEntityBuilder.CreateScreenCaptureFromPath("screen.png").Build());
        /// </code>
        /// </summary>
        /// 
        /// <param name="status"><see cref="Status"/></param>
        /// <param name="details">Log details</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Log(Status status, string details)
        {
            return Log(status, details, null, null);
        }

        /// <summary>
        /// Logs an event with <see cref="Status"/>, details and a media object: <see cref="ScreenCapture"/>
        /// 
        /// <code>
        /// test.Log(Status.Fail, "details", MediaEntityBuilder.CreateScreenCaptureFromPath("screen.png").Build());
        /// </code>
        /// </summary>
        /// 
        /// <param name="status"><see cref="Status"/></param>
        /// <param name="m"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Log(Status status, IMarkup m)
        {
            return Log(status, m.GetMarkup(), null, null);
        }

        /// <summary>
        /// Logs an <see cref="Status.Info"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="details">Details</param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Info(string details, Media media = null)
        {
            Log(Status.Info, details, null, media);
            return this;
        }

        /// <summary>
        /// Logs an <see cref="Status.Info"/> event with an exception and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Info(Exception ex, Media media = null)
        {
            return Log(Status.Info, ex, media);
        }

        /// <summary>
        /// Logs an <see cref="Status.Info"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="media">A <see cref="Media"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Info(Media media = null)
        {
            return Log(Status.Info, media);
        }

        /// <summary>
        /// Logs an <see cref="Status.Info"/> event with <see cref="IMarkup"/>
        /// </summary>
        /// <param name="m"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Info(IMarkup m)
        {
            return Log(Status.Info, m.GetMarkup(), null);
        }

        /// <summary>
        /// Logs an <see cref="Status.Pass"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="details">Details</param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Pass(string details, Media media = null)
        {
            return Log(Status.Pass, details, null, media);
        }

        /// <summary>
        /// Logs an <see cref="Status.Pass"/> event with an exception and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Pass(Exception ex, Media media = null)
        {
            return Log(Status.Pass, ex, media);
        }

        /// <summary>
        /// Logs an <see cref="Status.Pass"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="media">A <see cref="Media"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Pass(Media media = null)
        {
            return Log(Status.Pass, media);
        }

        /// <summary>
        /// Logs an <see cref="Status.Pass"/> event with <see cref="IMarkup"/>
        /// </summary>
        /// <param name="m"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Pass(IMarkup m)
        {
            return Log(Status.Pass, m.GetMarkup(), null);
        }

        /// <summary>
        /// Logs an <see cref="Status.Fail"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="media">A <see cref="Media"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Fail(string details, Media media = null)
        {
            return Log(Status.Fail, details, null, media);
        }

        /// <summary>
        /// Logs an <see cref="Status.Fail"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="media">A <see cref="Media"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Fail(Media media = null)
        {
            return Log(Status.Fail, media);
        }

        /// <summary>
        /// Logs an <see cref="Status.Fail"/> event with an exception and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Fail(Exception ex, Media media = null)
        {
            return Log(Status.Fail, ex, media);
        }

        /// <summary>
        /// Logs an <see cref="Status.Fail"/> event with <see cref="IMarkup"/>
        /// </summary>
        /// <param name="m"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Fail(IMarkup m)
        {
            return Log(Status.Fail, m.GetMarkup(), null);
        }

        /// <summary>
        /// Logs an <see cref="Status.Warning"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="details">Details</param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Warning(string details, Media media = null)
        {
            return Log(Status.Warning, details, null, media);
        }

        /// <summary>
        /// Logs an <see cref="Status.Warning"/> event with an exception and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Warning(Exception ex, Media media = null)
        {
            return Log(Status.Warning, ex, media);
        }

        /// <summary>
        /// Logs an <see cref="Status.Warning"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="media">A <see cref="Media"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Warning(Media media = null)
        {
            return Log(Status.Warning, media);
        }

        /// <summary>
        /// Logs an <see cref="Status.Warning"/> event with <see cref="IMarkup"/>
        /// </summary>
        /// <param name="m"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Warning(IMarkup m)
        {
            return Log(Status.Warning, m.GetMarkup(), null);
        }

        /// <summary>
        /// Logs an <see cref="Status.Skip"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="details">Details</param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Skip(string details, Media media = null)
        {
            return Log(Status.Skip, details, null, media);
        }

        /// <summary>
        /// Logs an <see cref="Status.Skip"/> event with an exception and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Skip(Exception ex, Media media = null)
        {
            return Log(Status.Skip, ex, media);
        }

        /// <summary>
        /// Logs an <see cref="Status.Skip"/> event with <see cref="IMarkup"/>
        /// </summary>
        /// <param name="m"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Skip(IMarkup m)
        {
            return Log(Status.Skip, m.GetMarkup(), null);
        }

        /// <summary>
        /// Logs an <see cref="Status.Skip"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="media">A <see cref="Media"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Skip(Media media = null)
        {
            return Log(Status.Skip, media);
        }

        /// <summary>
        /// Assigns an author
        /// </summary>
        /// <param name="author"><see cref="Author"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest AssignAuthor(params string[] author)
        {
            if (author == null || author.Length == 0)
            {
                return this;
            }

            var authors = author.Where(x => !string.IsNullOrEmpty(x)).Select(x => new Author(x));

            foreach (var x in authors)
            {
                Model.Author.Add(x);
                Extent.OnAuthorAdded(x, Model);
            }

            return this;
        }

        /// <summary>
        /// Assigns a category or group
        /// </summary>
        /// <param name="category"><see cref="Category"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest AssignCategory(params string[] category)
        {
            if (category == null || category.Length == 0)
            {
                return this;
            }

            var categories = category.Where(x => !string.IsNullOrEmpty(x)).Select(x => new Category(x));

            foreach (var x in categories)
            {
                Model.Category.Add(x);
                Extent.OnCategoryAdded(x, Model);
            }

            return this;
        }

        /// <summary>
        /// Assigns a device
        /// </summary>
        /// <param name="device"><see cref="Device"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest AssignDevice(params string[] device)
        {
            if (device == null || device.Length == 0)
            {
                return this;
            }

            var devices = device.Where(x => !string.IsNullOrEmpty(x)).Select(x => new Device(x));

            foreach (var x in devices)
            {
                Model.Device.Add(x);
                Extent.OnDeviceAdded(x, Model);
            }

            return this;
        }

        /// <summary>
        /// Adds a screenshot using a file path
        /// </summary>
        /// <param name="path">Image path</param>
        /// <param name="title">Image title</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest AddScreenCaptureFromPath(string path, string title = null)
        {
            Assert.NotEmpty(path, "ScreenCapture path must not be null or empty");

            ScreenCapture sc = new ScreenCapture
            {
                Path = path,
                Title = title
            };

            Model.Media.Add(sc);

            return this;
        }

        /// <summary>
        /// Adds a screenshot using a file path
        /// </summary>
        /// <param name="base64">Base64 string</param>
        /// <param name="title">Image title</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest AddScreenCaptureFromBase64String(string base64, string title = null)
        {
            Assert.NotEmpty(base64, "ScreenCapture Base64 string must not be null or empty");

            if (!base64.StartsWith("data:"))
            {
                base64 = "data:image/png;base64," + base64;
            }

            ScreenCapture sc = new ScreenCapture
            {
                Base64 = base64,
                Title = title
            };

            Model.Media.Add(sc);

            return this;
        }
    }
}
