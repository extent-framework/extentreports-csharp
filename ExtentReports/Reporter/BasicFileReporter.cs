using AventStack.ExtentReports.Configuration;
using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter.Configuration;
using AventStack.ExtentReports.Reporter.Configuration.Default;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace AventStack.ExtentReports.Reporter
{
    public abstract class BasicFileReporter : ConfigurableReporter
    {
        public override string ReporterName { get; }

        protected string SavePath { get; set; }
        protected string FolderSavePath { get; set; }

        public override AnalysisStrategy AnalysisStrategy { get; set; }

        public override ReportStatusStats ReportStatusStats { get; protected internal set; }

        public DateTime StartTime { get; private set; }

        public DateTime EndTime { get; private set; }

        public List<Test> TestList { get; protected internal set; }

        public List<Status> StatusList { get; protected internal set; }

        public TestAttributeTestContextProvider<Author> AuthorContext { get; protected internal set; }

        public TestAttributeTestContextProvider<Category> CategoryContext { get; protected internal set; }

        public TestAttributeTestContextProvider<Device> DeviceContext { get; protected internal set; }

        public TestAttributeTestContextProvider<ExceptionInfo> ExceptionInfoContext { get; protected internal set; }

        public SystemAttributeContext SystemAttributeContext { get; protected internal set; }

        public List<string> TestRunnerLogs { get; protected internal set; }

        protected BasicFileConfiguration Configuration { get; private set; }

        protected BasicFileReporter(string filePath)
        {
            SavePath = filePath;
            FolderSavePath = Path.GetDirectoryName(filePath);
            FolderSavePath = string.IsNullOrEmpty(FolderSavePath) ? "./" : FolderSavePath;
        }

        protected void Initialize(BasicFileConfiguration userConfig)
        {
            Configuration = userConfig;

            foreach (SettingsProperty setting in ExtentHtmlReporterSettings.Default.Properties)
            {
                var key = setting.Name;
                var value = ExtentHtmlReporterSettings.Default.Properties[setting.Name].DefaultValue.ToString();

                var c = new Config(key, value);
                MasterConfig.AddConfig(c);
            }
        }

        public override void Flush(ReportAggregates reportAggregates)
        {
            TestList = reportAggregates.TestList;
            StatusList = reportAggregates.StatusList;
            ReportStatusStats = reportAggregates.ReportStatusStats;
            AuthorContext = reportAggregates.AuthorContext;
            CategoryContext = reportAggregates.CategoryContext;
            DeviceContext = reportAggregates.DeviceContext;
            ExceptionInfoContext = reportAggregates.ExceptionInfoContext;
            SystemAttributeContext = reportAggregates.SystemAttributeContext;
            TestRunnerLogs = reportAggregates.TestRunnerLogs;

            EndTime = DateTime.Now;

            LoadUserConfig();
        }

        private void LoadUserConfig()
        {
            foreach (var pair in Configuration.UserConfigurationMap)
            {
                var key = pair.Key;
                var value = pair.Value;

                var c = new Config(key, value);
                MasterConfig.AddConfig(c);
            }
        }

        public override void OnAuthorAssigned(Test test, Author author)
        {

        }

        public override void OnCategoryAssigned(Test test, Category category)
        {

        }

        public override void OnDeviceAssigned(Test test, Device device)
        {

        }

        public override void OnLogAdded(Test test, Log log)
        {

        }

        public override void OnNodeStarted(Test node)
        {

        }

        public override void OnScreenCaptureAdded(Test test, ScreenCapture screenCapture)
        {

        }

        public override void OnScreenCaptureAdded(Log log, ScreenCapture screenCapture)
        {

        }

        public override void OnTestRemoved(Test test)
        {

        }

        public override void OnTestStarted(Test test)
        {

        }

        public override void Start()
        {
            StartTime = DateTime.Now;
        }

        public override void Stop()
        {

        }

        public bool ContainsStatus(Status status)
        {
            return StatusList.Contains(status);
        }
    }
}
