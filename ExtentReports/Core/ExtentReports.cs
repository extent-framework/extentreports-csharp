using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Gherkin;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Listener.Entity;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using System;
using System.Collections.Generic;

namespace AventStack.ExtentReports
{
    public class ExtentReports : AbstractProcessor
    {
        public string GherkinDialect
        {
            get => GherkinDialectProvider.Lang;
            set => GherkinDialectProvider.Lang = value;
        }

        public bool UseNaturalTime
        {
            get => UsingNaturalConf;
            set => UsingNaturalConf = value;
        }

        public Status Status => Report.Status;

        public new Report Report => base.Report;

        public void AttachReporter(params IObserver<ReportEntity>[] observer)
        {
            foreach (IObserver<ReportEntity> x in observer)
            {
                Subscribe(x);
            }
        }

        public ExtentTest CreateTest(GherkinKeyword keyword, string name, string description = "")
        {
            var test = new ExtentTest(this, keyword, name, description);
            OnTestCreated(test.Test);
            return test;
        }

        public ExtentTest CreateTest<T>(string name, string description = "") where T : IGherkinFormatterModel
        {
            var type = typeof(T).Name;
            var keyword = new GherkinKeyword(type);
            return CreateTest(keyword, name, description);
        }

        public ExtentTest CreateTest(string name, string description = "")
        {
            var test = new ExtentTest(this, name, description);
            OnTestCreated(test.Test);
            return test;
        }

        public void RemoveTest(int id)
        {
            Report.RemoveTest(id);
        }

        public void RemoveTest(string name)
        {
            var test = Report.FindTest(name);
            Report.RemoveTest(test);
        }

        public void RemoveTest(ExtentTest test)
        {
            Report.RemoveTest(test.Test);
        }

        public new void Flush()
        {
            OnFlush();
        }

        public void AddSystemInfo(string name, string value)
        {
            OnSystemInfoAdded(new SystemEnvInfo(name, value));
        }

        /// <summary>
        /// Adds logs from test framework tools to the test-runner logs view (if available in the reporter)
        /// </summary>
        /// <param name="log"></param>
        public void AddTestRunnerLogs(string log)
        {
            OnReportLogAdded(log);
        }

        /// <summary>
        /// Adds logs from test framework tools to the test-runner logs view (if available in the reporter)
        /// </summary>
        /// <param name="log"></param>
        public void AddTestRunnerLogs(string[] log)
        {
            foreach (string l in log)
            {
                AddTestRunnerLogs(l);
            }
        }

        /// <summary>
        /// Adds logs from test framework tools to the test-runner logs view (if available in the reporter)
        /// </summary>
        /// <param name="log"></param>
        public void AddTestRunnerLogs(List<string> log)
        {
            log.ForEach(x => AddTestRunnerLogs(x));
        }

        /// <summary>
        /// Tries to resolve a <seealso cref="Media"/> location if the absolute path is not
        /// found using the supplied locations. This can resolve cases where the default
        /// path was supplied to be relative for a FileReporter. If the absolute path is not
        /// determined, the supplied path will be used.
        /// </summary>
        /// <param name="path">An array of paths to be used if the provided media path is not 
        /// resolved
        /// </param>
        public new void TryResolveMediaPath(string[] path)
        {
            base.TryResolveMediaPath(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void TryResolveMediaPath(string path)
        {
            TryResolveMediaPath(new string[] { path });
        }

        /// <summary>
        /// Creates the internal model by consuming a JSON archive using <code>ExtentJsonFormatter</code>
        /// from a previous run session. This provides the same functionality available in earlier
        /// versions with <code>AppendExisting</code>.
        /// </summary>
        /// <param name="filePath">The JSON archive file created by <seealso cref="ExtentJsonFormatter"/></param>
        public void CreateDomainFromJsonArchive(string filePath)
        {
            ConvertRawEntities(this, filePath);
        }
    }
}
