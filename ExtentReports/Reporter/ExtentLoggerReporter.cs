using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Reporter.Configuration;
using AventStack.ExtentReports.Views.Commons;
using AventStack.ExtentReports.Views.Logger;

using RazorEngine.Templating;

using System;
using System.IO;

namespace AventStack.ExtentReports.Reporter
{
    public class ExtentLoggerReporter : BasicFileReporter
    {
        public new string ReporterName => "logger";

        private static readonly string TemplateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views");
        private static readonly string LoggerTemplateFolderPath = Path.Combine(TemplateFolderPath, "Logger");

        public ExtentLoggerReporter(string folderPath) : base(folderPath)
        {
            Config = new ExtentLoggerReporterConfiguration(this);
            Initialize(Config);
        }

        public ExtentLoggerReporterConfiguration Config { get; }

        public override void Flush(ReportAggregates reportAggregates)
        {
            base.Flush(reportAggregates);

            Directory.CreateDirectory(SavePath);

            var source = RazorEngineManager.Instance.Razor.RunCompile("LoggerTest", typeof(ExtentLoggerReporter), this);
            File.WriteAllText(SavePath + "Index.html", source);
            source = RazorEngineManager.Instance.Razor.RunCompile("LoggerDashboard", typeof(ExtentLoggerReporter), this);
            File.WriteAllText(SavePath + "Dashboard.html", source);
            source = RazorEngineManager.Instance.Razor.RunCompile("LoggerTag", typeof(ExtentLoggerReporter), this);
            File.WriteAllText(SavePath + "Tag.html", source);
            source = RazorEngineManager.Instance.Razor.RunCompile("LoggerException", typeof(ExtentLoggerReporter), this);
            File.WriteAllText(SavePath + "Exception.html", source);
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
                "LoggerDashboard",
                "LoggerException",
                "LoggerTag",
                "LoggerTest",
                "Partials.LoggerHead",
                "Partials.LoggerNav",
                "Partials.LoggerNavRight",
                "LoggerMacro"
            };
            AddTemplates<ILoggerMarker>(templates);

            templates = new string[]
            {
                "CommonsAttributes",
                "CommonsDashboard",
                "CommonsDashboardScripts",
                "CommonsException",
                "CommonsInjectCss",
                "CommonsInjectJs",
                "CommonsMedia",
                "CommonsRow",
                "CommonsTag"
            };
            AddTemplates<ICommonsMarker>(templates);
        }

        private void AddTemplates<T>(string[] templates)
        {
            foreach (string template in templates)
            {
                var resourceName = typeof(T).Namespace + "." + template + ".cshtml";
                using (var resourceStream = typeof(T).Assembly.GetManifestResourceStream(resourceName))
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
