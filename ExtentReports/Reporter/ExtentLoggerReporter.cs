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

            var source = RazorEngineManager.Instance.Razor.RunCompile("LoggerTest", typeof(ExtentLoggerReporter), this);
            File.WriteAllText(SavePath + "Index.html", source);
            source = RazorEngineManager.Instance.Razor.RunCompile("CommonsDashboard", typeof(ExtentLoggerReporter), this);
            File.WriteAllText(SavePath + "Dashboard.html", source);
            if (CategoryContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("CommonsTag", typeof(ExtentLoggerReporter), this);
                File.WriteAllText(SavePath + "Tag.html", source);
            }
            if (ExceptionInfoContext.Context.Count > 0)
            {
                source = RazorEngineManager.Instance.Razor.RunCompile("CommonsException", typeof(ExtentLoggerReporter), this);
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
                "Views.Logger.LoggerTest",
                "Views.Logger.LoggerMacro"
            };
            TemplateLoadService.LoadTemplate<ILoggerMarker>(templates);

            templates = new string[]
            {
                "Views.Commons.CommonsAttributes",
                "Views.Commons.CommonsDashboard",
                "Views.Commons.CommonsException",
                "Views.Commons.CommonsHead",
                "Views.Commons.CommonsInjectCss",
                "Views.Commons.CommonsInjectJs",
                "Views.Commons.CommonsMedia",
                "Views.Commons.CommonsNav",
                "Views.Commons.CommonsNavRight",
                "Views.Commons.CommonsRow",
                "Views.Commons.CommonsTag"
            };
            TemplateLoadService.LoadTemplate<ICommonsMarker>(templates);
        }
    }
}
