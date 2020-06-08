using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Views.V3Html;
using AventStack.ExtentReports.Reporter.Configuration;
using AventStack.ExtentReports.Reporter.TemplateEngine;

using RazorEngine.Templating;

using System;
using System.IO;

namespace AventStack.ExtentReports.Reporter
{
    [Obsolete("ExtentV3HtmlReporter has been deprecated and will be removed in a future release")]
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
                "V3Index",
                "V3Head",
                "V3Nav",
                "Test.V3Test",
                "Test.V3Charts",
                "Author.V3Author",
                "Category.V3Category",
                "Dashboard.V3Dashboard",
                "Exception.V3Exception",
                "TestRunner.V3Logs"
            };

            TemplateLoadService.LoadTemplate<IV3HtmlMarker>(templates);
        }
    }
}
