using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Reporter.Configuration;
using AventStack.ExtentReports.Reporter.TemplateEngine;
using AventStack.ExtentReports.Views.Html;

using RazorEngine.Templating;

using System;
using System.IO;

namespace AventStack.ExtentReports.Reporter
{
    /// <summary>
    /// The ExtentHtmlReporter creates a rich standalone HTML file. It allows several configuration options
    /// via the <code>Configuration()</code> method.
    /// </summary>
    public class ExtentHtmlReporter : BasicFileReporter
    {
        public new string ReporterName => "html";

        private static readonly string TemplateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views");
        private static readonly string HtmlTemplateFolderPath = Path.Combine(TemplateFolderPath, "Html");

        public ExtentHtmlReporter(string folderPath) : base(folderPath)
        {
            Config = new ExtentHtmlReporterConfiguration(this);
            Initialize(Config);
        }

        public ExtentHtmlReporterConfiguration Config { get; }

        public override void Flush(ReportAggregates reportAggregates)
        {
            base.Flush(reportAggregates);

            Directory.CreateDirectory(FolderSavePath);

            var source = RazorEngineManager.Instance.Razor.RunCompile("Index", typeof(ExtentHtmlReporter), this);
            File.WriteAllText(Path.Combine(FolderSavePath, "index.html"), source);
            source = RazorEngineManager.Instance.Razor.RunCompile("Dashboard", typeof(ExtentHtmlReporter), this);
            File.WriteAllText(Path.Combine(FolderSavePath, "dashboard.html"), source);
            if (AuthorContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("Author", typeof(ExtentHtmlReporter), this);
                File.WriteAllText(Path.Combine(FolderSavePath, "author.html"), source);
            }
            if (CategoryContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("Tag", typeof(ExtentHtmlReporter), this);
                File.WriteAllText(Path.Combine(FolderSavePath, "tag.html"), source);
            }
            if (DeviceContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("Device", typeof(ExtentHtmlReporter), this);
                File.WriteAllText(Path.Combine(FolderSavePath, "device.html"), source);
            }
            if (ExceptionInfoContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("Exception", typeof(ExtentHtmlReporter), this);
                File.WriteAllText(Path.Combine(FolderSavePath, "exception.html"), source);
            }
        }

        public override void Start()
        {
            base.Start();
            AddTemplates();
        }

        private void AddTemplates()
        {
            string[] templates = new string[]
            {
                "Author",
                "Dashboard",
                "Device",
                "Exception",
                "Index",
                "Tag",
                "Partials.Attributes",
                "Partials.AttributesView",
                "Partials.Head",
                "Partials.Log",
                "Partials.Navbar",
                "Partials.RecurseNodes",
                "Partials.Scripts",
                "Partials.Sidenav"
            };

            TemplateLoadService.LoadTemplate<IHtmlMarker>(templates);
        }
    }
}
