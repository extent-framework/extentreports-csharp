using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Views.Html;
using AventStack.ExtentReports.Reporter.Configuration;

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

        public ExtentHtmlReporter(string filePath) : base(filePath)
        {
            Config = new ExtentHtmlReporterConfiguration(this);
            Initialize(Config);
        }

        public ExtentHtmlReporterConfiguration Config { get; }

        public override void Flush(ReportAggregates reportAggregates)
        {
            base.Flush(reportAggregates);
            var source = RazorEngineManager.Instance.Razor.RunCompile("Index", typeof(ExtentHtmlReporter), this);
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
                "Index",
                "Head",
                "Nav",
                "Test.Test",
                "Test.Charts",
                "Author.Author",
                "Category.Category",
                "Dashboard.Dashboard",
                "Exception.Exception",
                "TestRunner.Logs"
            };

            foreach (string template in templates)
            {
                string resourceName = typeof(IHtmlMarker).Namespace + "." + template + ".cshtml";
                using (var resourceStream = typeof(IHtmlMarker).Assembly.GetManifestResourceStream(resourceName))
                {
                    using (var reader = new StreamReader(resourceStream))
                    {
                        if (resourceStream != null)
                        {
                            var arr = template.Split('.');
                            var name = arr.Length > 1 ? arr[arr.Length - 1] : arr[0];
                            RazorEngineManager.Instance.Razor.AddTemplate(name, reader.ReadToEnd());
                        }
                    }
                }
            }
        }
    }
}
