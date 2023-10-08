using AventStack.ExtentReports.Config;
using AventStack.ExtentReports.Listener.Entity;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter.Config;
using AventStack.ExtentReports.Reporter.Filter;
using AventStack.ExtentReports.Reporter.Templating;
using AventStack.ExtentReports.Views.Spark;
using RazorEngine.Templating;
using System;
using System.IO;

namespace AventStack.ExtentReports.Reporter
{
    public class ExtentSparkReporter : AbstractFileReporter, IReporterFilterable<ExtentSparkReporter>, IReporterConfigurable, IObserver<ReportEntity>
    {
        private EntityFilters<ExtentSparkReporter> _filter;

        public ExtentSparkReporterConfig Config;
        public Report Report;

        public EntityFilters<ExtentSparkReporter> Filter
        {
            get
            {
                if (_filter == null)
                {
                    _filter = new EntityFilters<ExtentSparkReporter>(this);
                }

                return _filter;
            }
        }

        public ExtentSparkReporter(string filePath) : base(filePath)
        {
            AddTemplates();
            Config = new ExtentSparkReporterConfig(this);
        }

        public void LoadJSONConfig(string filePath)
        {
            var loader = new JsonConfigLoader();
            loader.LoadJSONConfig(ref Config, filePath);
            ApplyConfig();
        }

        private void ApplyConfig()
        {
            if (Config.OfflineMode)
            {
                Config.Reporter = this;
                Config.SaveOfflineResources();
            }
        }

        public void LoadXMLConfig(string filePath)
        {
            var loader = new XmlConfigLoader();
            loader.LoadXMLConfig(ref Config, filePath);
            ApplyConfig();
        }

        public void LoadConfig(string filePath)
        {
            LoadXMLConfig(filePath);
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(ReportEntity value)
        {
            Report = _filter == null ? value.Report : FilterReport(value.Report, _filter.StatusFilter.Status);

            if (Report.HasTests)
            {
                var source = RazorEngineManager.Instance.Razor.RunCompile("SparkIndexSPA", typeof(ExtentSparkReporter), this);
                File.WriteAllText(Path.Combine(FolderSavePath, SavePath), source);
            }
        }

        private void AddTemplates()
        {
            string[] templates = new string[]
            {
                "Partials.Attributes",
                "Partials.AttributesView",
                "Partials.Head",
                "Partials.Log",
                "Partials.Navbar",
                "Partials.RecurseNodes",
                "Partials.Scripts",
                "Partials.Sidenav",
                "Partials.SparkBDD",
                "Partials.SparkStandard",
                "Partials.SparkStepDetails",
                "SparkIndexSPA",
                "Partials.SparkMedia",
                "Partials.SparkTestSPA",
                "Partials.SparkAuthorSPA",
                "Partials.SparkDeviceSPA",
                "Partials.SparkTagSPA",
                "Partials.SparkExceptionSPA",
                "Partials.SparkDashboardSPA",
                "Partials.SparkLogsSPA",
                "Partials.StepDetails"
            };

            TemplateLoadService.LoadTemplate<ISparkMarker>(templates);
        }
    }
}
