using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Reporter.Configuration;
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
            File.WriteAllText(FolderSavePath + "index.html", source);
            source = RazorEngineManager.Instance.Razor.RunCompile("Dashboard", typeof(ExtentHtmlReporter), this);
            File.WriteAllText(FolderSavePath + "dashboard.html", source);
            if (AuthorContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("Author", typeof(ExtentHtmlReporter), this);
                File.WriteAllText(FolderSavePath + "author.html", source);
            }
            if (CategoryContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("Tag", typeof(ExtentHtmlReporter), this);
                File.WriteAllText(FolderSavePath + "tag.html", source);
            }
            if (DeviceContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("Device", typeof(ExtentHtmlReporter), this);
                File.WriteAllText(FolderSavePath + "device.html", source);
            }
            if (ExceptionInfoContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("Exception", typeof(ExtentHtmlReporter), this);
                File.WriteAllText(FolderSavePath + "exception.html", source);
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
