using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Reporter.Configuration;
using AventStack.ExtentReports.Reporter.TemplateEngine;
using AventStack.ExtentReports.Views.Html;

using RazorEngine.Templating;

using System;
using System.IO;

namespace AventStack.ExtentReports.Reporter
{
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

            var source = RazorEngineManager.Instance.Razor.RunCompile("Index", typeof(ExtentHtmlReporter), this);
            File.WriteAllText(Path.Combine(FolderSavePath, "index.html"), source);
            source = RazorEngineManager.Instance.Razor.RunCompile("Dashboard", typeof(ExtentHtmlReporter), this);
            File.WriteAllText(Path.Combine(FolderSavePath, "dashboard.html"), source);
            if (CategoryContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("Tag", typeof(ExtentHtmlReporter), this);
                File.WriteAllText(Path.Combine(FolderSavePath, "tag.html"), source);
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
                "Views.Html.Dashboard",
                "Views.Html.Exception",
                "Views.Html.Index",
                "Views.Html.Tag",
                "Views.Html.Partials.Attributes",
                "Views.Html.Partials.AttributesView",
                "Views.Html.Partials.Head",
                "Views.Html.Partials.Log",
                "Views.Html.Partials.Navbar",
                "Views.Html.Partials.RecurseNodes",
                "Views.Html.Partials.Scripts",
                "Views.Html.Partials.Sidenav",
                "Views.Html.Partials.SparkBDD",
                "Views.Html.Partials.SparkStandard",
                "Views.Html.Partials.SparkStepDetails",
            };

            TemplateLoadService.LoadTemplate<IHtmlMarker>(templates);
        }
    }
}
