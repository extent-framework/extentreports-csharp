using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Model;

using System;
using System.Linq;

namespace AventStack.ExtentReports
{
    public class ExtentTest : IMediaContainer<ExtentTest>
    {
        public Status Status => Model.Status;

        public Test Model { get; private set; }
        public ExtentReports Extent { get; private set; }

        internal ExtentTest(ExtentReports extent, IGherkinFormatterModel bddType, string name, string description)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Test name cannot be null or empty");

            Extent = extent;
            Model = new Test
            {
                Name = name,
                Description = description,
                BehaviorDrivenType = bddType
            };
        }

        internal ExtentTest(ExtentReports extent, string name, string description = "")
            : this(extent, null, name, description)
        { }

        public ExtentTest CreateNode<T>(string name, string description = "") where T : IGherkinFormatterModel
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Test name cannot be null or empty");

            Type type = typeof(T);
            var obj = (IGherkinFormatterModel)Activator.CreateInstance(type);

            var node = new ExtentTest(Extent, obj, name, description);
            ApplyCommonNodeSettings(node);
            return node;
        }

        private void ApplyCommonNodeSettings(ExtentTest node)
        {
            node.Model.Parent = Model;
            Model.NodeContext.Add(node.Model);
            AddNodeToReport(node);
        }

        private void AddNodeToReport(ExtentTest node)
        {
            Extent.AddNode(node.Model);
        }

        public ExtentTest CreateNode(string name, string description = "")
        {
            var node = new ExtentTest(Extent, name, description);
            ApplyCommonNodeSettings(node);
            return node;
        }

        public ExtentTest CreateNode(GherkinKeyword keyword, string name, string description = "")
        {
            var node = new ExtentTest(Extent, name, description);
            node.Model.BehaviorDrivenType = keyword.Model;
            ApplyCommonNodeSettings(node);
            return node;
        }
        
        /// <summary>
        /// Removes a direct child of the current test
        /// </summary>
        /// <param name="test">The child node to be removed</param>
        public void RemoveNode(ExtentTest test)
        {
            if (Model.HasChildren)
            {
                Model.NodeContext.Remove(test.Model);
            }
        }

        /// <summary>
        /// Logs an event with <see cref="Status"/>, details and a media object: <see cref="ScreenCapture"/>
        /// 
        /// <code>
        /// test.Log(Status.Fail, "details", MediaEntityBuilder.CreateScreenCaptureFromPath("screen.png").Build());
        /// </code>
        /// </summary>
        /// <param name="status"><see cref="Status"/></param>
        /// <param name="details">Log details</param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Log(Status status, string details, MediaEntityModelProvider provider = null)
        {
            var evt = CreateLog(status, details);

            if (provider != null)
            {
                AddsScreenCapture(evt, provider.Media);
            }

            return AddLog(evt);
        }

        private void AddsScreenCapture(Log evt, Media m)
        {
            evt.AddScreenCapture((ScreenCapture)m);
        }

        /// <summary>
        /// Logs an event with <see cref="Status"/> and a media object: <see cref="ScreenCapture"/>
        /// 
        /// <code>
        /// test.Log(Status.Fail, MediaEntityBuilder.CreateScreenCaptureFromPath("screen.png").Build());
        /// </code>
        /// </summary>
        /// <param name="status"><see cref="Status"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Log(Status status, MediaEntityModelProvider provider = null)
        {
            var evt = CreateLog(status);

            if (provider != null)
            {
                AddsScreenCapture(evt, provider.Media);
            }

            return AddLog(evt);
        }

        /// <summary>
        /// Logs an event with <see cref="Status"/>, an exception and a media object: <see cref="ScreenCapture"/>
        /// 
        /// <code>
        /// test.Log(Status.Fail, exception, MediaEntityBuilder.CreateScreenCaptureFromPath("screen.png").Build());
        /// </code>
        /// </summary>
        /// <param name="status"><see cref="Status"/></param>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Log(Status status, Exception ex, MediaEntityModelProvider provider = null)
        {
            ExceptionInfo e = new ExceptionInfo(ex);
            Model.ExceptionInfoContext.Add(e);
            var evt = new Log(Model)
            {
                ExceptionInfo = e
            };
            return AddLog(evt);
        }

        /// <summary>
        /// Logs an event with <see cref="Status"/> and custom <see cref="IMarkup"/> such as:
        /// 
        /// <list type="bullet">
        /// <item>CodeBlock</item>
        /// <item>Label</item>
        /// <item>Table</item>
        /// </list>
        /// </summary>
        /// <param name="status"></param>
        /// <param name="markup"></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Log(Status status, IMarkup markup)
        {
            string details = markup.GetMarkup();
            return Log(status, details);
        }

        private ExtentTest AddLog(Log evt)
        {
            Model.LogContext.Add(evt);
            Extent.AddLog(Model, evt);

            if (evt.HasScreenCapture)
            {
                Extent.AddScreenCapture(evt, evt.ScreenCaptureContext.FirstOrDefault());
            }

            return this;
        }

        private Log CreateLog(Status status, string details = null)
        {
            details = details == null ? "" : details.Trim();
            Log evt = new Log(this.Model)
            {
                Status = status,
                Details = details,
                Sequence = Model.LogContext.All().Count + 1
            };
            return evt;
        }

        /// <summary>
        /// Logs an <see cref="Status.Info"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="details">Details</param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Info(string details, MediaEntityModelProvider provider = null)
        {
            Log(Status.Info, details, provider);
            return this;
        }

        /// <summary>
        /// Logs an <see cref="Status.Info"/> event with an exception and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Info(Exception ex, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Info, ex, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Info"/> event with <see cref="IMarkup"/>
        /// </summary>
        /// <param name="m"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Info(IMarkup m)
        {
            return Log(Status.Info, m);
        }

        /// <summary>
        /// Logs an <see cref="Status.Pass"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="details">Details</param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Pass(string details, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Pass, details, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Pass"/> event with an exception and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Pass(Exception ex, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Pass, ex, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Pass"/> event with <see cref="IMarkup"/>
        /// </summary>
        /// <param name="m"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Pass(IMarkup m)
        {
            return Log(Status.Pass, m);
        }

        /// <summary>
        /// Logs an <see cref="Status.Fail"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="details">Details</param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Fail(string details, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Fail, details, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Fail"/> event with an exception and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Fail(Exception ex, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Fail, ex, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Fail"/> event with <see cref="IMarkup"/>
        /// </summary>
        /// <param name="m"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Fail(IMarkup m)
        {
            return Log(Status.Fail, m);
        }

        /// <summary>
        /// Logs an <see cref="Status.Fatal"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="details">Details</param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Fatal(string details, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Fatal, details, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Fatal"/> event with an exception and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Fatal(Exception ex, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Fatal, ex, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Fatal"/> event with <see cref="IMarkup"/>
        /// </summary>
        /// <param name="m"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Fatal(IMarkup m)
        {
            return Log(Status.Fatal, m);
        }

        /// <summary>
        /// Logs an <see cref="Status.Warning"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="details">Details</param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Warning(string details, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Warning, details, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Warning"/> event with an exception and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Warning(Exception ex, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Warning, ex, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Warning"/> event with <see cref="IMarkup"/>
        /// </summary>
        /// <param name="m"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Warning(IMarkup m)
        {
            return Log(Status.Warning, m);
        }

        /// <summary>
        /// Logs an <see cref="Status.Error"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="details">Details</param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Error(string details, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Error, details, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Error"/> event with an exception and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Error(Exception ex, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Error, ex, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Error"/> event with <see cref="IMarkup"/>
        /// </summary>
        /// <param name="m"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Error(IMarkup m)
        {
            return Log(Status.Error, m);
        }

        /// <summary>
        /// Logs an <see cref="Status.Skip"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="details">Details</param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Skip(string details, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Skip, details, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Skip"/> event with an exception and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Skip(Exception ex, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Skip, ex, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Skip"/> event with <see cref="IMarkup"/>
        /// </summary>
        /// <param name="m"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Skip(IMarkup m)
        {
            return Log(Status.Skip, m);
        }

        /// <summary>
        /// Logs an <see cref="Status.Debug"/> event with details and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="details">Details</param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Debug(string details, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Debug, details, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Debug"/> event with an exception and a media object <see cref="ScreenCapture"/>
        /// </summary>
        /// <param name="ex"><see cref="Exception"/></param>
        /// <param name="provider">A <see cref="MediaEntityModelProvider"/> object</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Debug(Exception ex, MediaEntityModelProvider provider = null)
        {
            return Log(Status.Debug, ex, provider);
        }

        /// <summary>
        /// Logs an <see cref="Status.Debug"/> event with <see cref="IMarkup"/>
        /// </summary>
        /// <param name="m"><see cref="IMarkup"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest Debug(IMarkup m)
        {
            return Log(Status.Debug, m);
        }

        /// <summary>
        /// Assigns an author
        /// </summary>
        /// <param name="author"><see cref="Author"/></param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest AssignAuthor(params string[] author)
        {
            if (author == null || author.Length == 0)
                return this;

            author.ToList().Distinct().Where(c => !string.IsNullOrEmpty(c)).ToList().ForEach(a =>
            {
                var auth = new Author(a.Replace(" ", ""));
                Model.AuthorContext.Add(auth);
                Extent.AssignAuthor(Model, auth);
            });

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
                return this;

            category.ToList().Distinct().Where(c => !string.IsNullOrEmpty(c)).ToList().ForEach(c =>
            {
                var tag = new Category(c.Replace(" ", ""));
                Model.CategoryContext.Add(tag);
                Extent.AssignCategory(Model, tag);
            });

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
                return this;

            device.ToList().Distinct().Where(c => !string.IsNullOrEmpty(c)).ToList().ForEach(a =>
            {
                var d = new Device(a.Replace(" ", ""));
                Model.DeviceContext.Add(d);
                Extent.AssignDevice(Model, d);
            });

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
            ScreenCapture sc = new ScreenCapture
            {
                Path = path,
                Title = title
            };
            AddScreenCapture(sc);
            return this;
        }

        private void AddScreenCapture(ScreenCapture sc)
        {
            Model.ScreenCaptureContext.Add(sc);

            if (Model.ObjectId != null)
            {
                int seq = Model.ScreenCaptureContext.Count;
                sc.TestObjectId = Model.ObjectId;
            }

            Extent.AddScreenCapture(Model, sc);
        }

        /// <summary>
        /// Adds a screenshot using a base64 string
        /// </summary>
        /// <param name="s">Base64 string</param>
        /// <param name="title">Image title</param>
        /// <returns>A <see cref="ExtentTest"/> object</returns>
        public ExtentTest AddScreenCaptureFromBase64String(string s, string title = null)
        {
            ScreenCapture sc = new ScreenCapture
            {
                Base64String = s,
                Title = title
            };
            AddScreenCapture(sc);
            return this;
        }
    }
}
