using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Reporter.Configuration;
using AventStack.ExtentReports.Reporter.TemplateEngine;
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
            if (CategoryContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("LoggerTag", typeof(ExtentLoggerReporter), this);
                File.WriteAllText(SavePath + "Tag.html", source);
            }
            if (ExceptionInfoContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("LoggerException", typeof(ExtentLoggerReporter), this);
                File.WriteAllText(SavePath + "Exception.html", source);
            }
        }

        public override void Start()
        {
            base.Start();
            AddTemplates();
        }

        private static void AddTemplates()
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
            TemplateLoadService.LoadTemplate<ILoggerMarker>(templates);

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
            TemplateLoadService.LoadTemplate<ICommonsMarker>(templates);
        }
    }
}
