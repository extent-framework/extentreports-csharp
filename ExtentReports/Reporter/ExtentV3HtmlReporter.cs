using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Views.V3Html;
using AventStack.ExtentReports.Reporter.Configuration;
using AventStack.ExtentReports.Reporter.TemplateEngine;

using RazorEngine.Templating;

using System;
using System.IO;

namespace AventStack.ExtentReports.Reporter
{
    /// <summary>
    /// The ExtentHtmlReporter creates a rich standalone HTML file. It allows several configuration options
    /// via the <code>Configuration()</code> method.
    /// </summary>
    public class ExtentV3HtmlReporter : BasicFileReporter
    {
        public new string ReporterName => "html";

        private static readonly string TemplateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views");
        private static readonly string HtmlTemplateFolderPath = Path.Combine(TemplateFolderPath, "V3Html");

        public ExtentV3HtmlReporter(string filePath) : base(filePath)
        {
            Config = new ExtentHtmlReporterConfiguration(this);
            Initialize(Config);
        }

        public ExtentHtmlReporterConfiguration Config { get; }

        public override void Flush(ReportAggregates reportAggregates)
        {
            base.Flush(reportAggregates);
            var source = RazorEngineManager.Instance.Razor.RunCompile("V3Index", typeof(ExtentV3HtmlReporter), this);
            File.WriteAllText(SavePath, source);
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
                "Views.V3Html.V3Index",
                "Views.V3Html.V3Head",
                "Views.V3Html.V3Nav",
                "Views.V3Html.Test.V3Test",
                "Views.V3Html.Test.V3Charts",
                "Views.V3Html.Author.V3Author",
                "Views.V3Html.Category.V3Category",
                "Views.V3Html.Dashboard.V3Dashboard",
                "Views.V3Html.Exception.V3Exception",
                "Views.V3Html.TestRunner.V3Logs"
            };

            TemplateLoadService.LoadTemplate<IV3HtmlMarker>(templates);
        }
    }
}
