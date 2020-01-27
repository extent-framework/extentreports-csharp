using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Gherkin;
using AventStack.ExtentReports.Gherkin.Model;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports
{
    public class ExtentReports : ReportObservable, IReportService
    {
        public ExtentReports()
        {
        }

        public List<IExtentReporter> StartedReporterList
        {
            get
            {
                return base.StarterReporterList;
            }
        }

        public string GherkinDialect
        {
            get
            {
                if (string.IsNullOrEmpty(GherkinDialect))
                    return GherkinDialectProvider.Language;
                return GherkinDialect;
            }
            set
            {
                GherkinDialectProvider.Language = value;
            }            
        }

        public new AnalysisStrategy AnalysisStrategy
        {
            get
            {
                return base.AnalysisStrategy;
            }
            set
            {
                base.AnalysisStrategy = value;
            }
        }

        public Status Status
        {
            get
            {
                return base.ReportStatus;
            }
        }

        public ReportStatusStats Stats
        {
            get
            {
                return ReportStatusStats;
            }
        }

        public ReportConfigurator Config
        {
            get
            {
                return ReportConfigurator.I;
            }
        }

        public void AttachReporter(params IExtentReporter[] reporter)
        {
            reporter.ToList().ForEach(x => Register(x));
        }

        public ExtentTest CreateTest<T>(string name, string description = "") where T : IGherkinFormatterModel
        {
            Type type = typeof(T);
            var obj = (IGherkinFormatterModel)Activator.CreateInstance(type);

            var test = new ExtentTest(this, obj, name, description);
            base.SaveTest(test.Model);

            return test;
        }

        public ExtentTest CreateTest(GherkinKeyword keyword, string name, string description = "")
        {
            var test = new ExtentTest(this, name, description);
            test.Model.BehaviorDrivenType = keyword.Model;
            base.SaveTest(test.Model);
            return test;
        }

        public ExtentTest CreateTest(string name, string description = "")
        {
            var test = new ExtentTest(this, name, description);
            base.SaveTest(test.Model);
            return test;
        }

        public void RemoveTest(ExtentTest test)
        {
            base.RemoveTest(test.Model);
        }

        public new void Flush()
        {
            base.Flush();
        }

        public new void AddSystemInfo(string name, string value)
        {
            base.AddSystemInfo(name, value);
        }

        /// <summary>
        /// Adds logs from test framework tools to the test-runner logs view (if available in the reporter)
        /// </summary>
        /// <param name="log"></param>
        public new void AddTestRunnerLogs(string log)
        {
            base.AddTestRunnerLogs(log);
        }

        /// <summary>
        /// Adds logs from test framework tools to the test-runner logs view (if available in the reporter)
        /// </summary>
        /// <param name="log"></param>
        public void AddTestRunnerLogs(string[] log)
        {
            base.AddTestRunnerLogs(log.ToList());
        }

        /// <summary>
        /// Adds logs from test framework tools to the test-runner logs view (if available in the reporter)
        /// </summary>
        /// <param name="log"></param>
        public new void AddTestRunnerLogs(List<string> log)
        {
            base.AddTestRunnerLogs(log);
        }

        /// <summary>
        /// Resets the current report session, and clears all logged information
        /// </summary>
        public void ResetSession()
        {
            base.Reset();
        }
    }
}
