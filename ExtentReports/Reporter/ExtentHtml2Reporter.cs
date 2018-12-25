using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Reporter.Configuration;
using AventStack.ExtentReports.Views.Html2;

using RazorEngine.Templating;

using System;
using System.IO;

namespace AventStack.ExtentReports.Reporter
{
    /// <summary>
    /// The ExtentHtmlReporter creates a rich standalone HTML file. It allows several configuration options
    /// via the <code>Configuration()</code> method.
    /// </summary>
    public class ExtentHtml2Reporter : BasicFileReporter
    {
        public new string ReporterName => "html";

        private static readonly string TemplateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views");
        private static readonly string HtmlTemplateFolderPath = Path.Combine(TemplateFolderPath, "Html2");

        public ExtentHtml2Reporter(string folderPath) : base(folderPath)
        {
            Config = new ExtentHtmlReporterConfiguration(this);
            Initialize(Config);
        }

        public ExtentHtmlReporterConfiguration Config { get; }

        public override void Flush(ReportAggregates reportAggregates)
        {
            base.Flush(reportAggregates);

            Directory.CreateDirectory(SavePath);

            var source = RazorEngineManager.Instance.Razor.RunCompile("Index", typeof(ExtentHtml2Reporter), this);
            File.WriteAllText(SavePath + "index.html", source);
            source = RazorEngineManager.Instance.Razor.RunCompile("Dashboard", typeof(ExtentHtml2Reporter), this);
            File.WriteAllText(SavePath + "dashboard.html", source);
            if (AuthorContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("Author", typeof(ExtentHtml2Reporter), this);
                File.WriteAllText(SavePath + "author.html", source);
            }
            if (CategoryContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("Tag", typeof(ExtentHtml2Reporter), this);
                File.WriteAllText(SavePath + "tag.html", source);
            }
            if (DeviceContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("Device", typeof(ExtentHtml2Reporter), this);
                File.WriteAllText(SavePath + "device.html", source);
            }
            if (ExceptionInfoContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("Exception", typeof(ExtentHtml2Reporter), this);
                File.WriteAllText(SavePath + "exception.html", source);
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
                "Partials.Log",
                "Partials.Navbar",
                "Partials.RecurseNodes",
                "Partials.Sidenav"
            };

            foreach (string template in templates)
            {
                string resourceName = typeof(IHtml2Marker).Namespace + "." + template + ".cshtml";
                using (var resourceStream = typeof(IHtml2Marker).Assembly.GetManifestResourceStream(resourceName))
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
