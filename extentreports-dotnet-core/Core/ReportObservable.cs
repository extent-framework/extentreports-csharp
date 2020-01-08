using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Model.Context;
using AventStack.ExtentReports.Utils;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports.Core
{
    public abstract class ReportObservable
    {
        /// <summary>
        /// The current AnalysisStrategy for the run session. Ths decides the technique used
        /// to count the test status at different levels. For example, for a BDD-style report,
        /// the levels to be calculated are Feature, Scenario and Step:  3 levels. For a generic,
        /// non-BDD style report, levels can be dynamic and typically consist of:
        /// 
        /// <list type="bullet">
        ///     <item>1 Level: Test</item>
        ///     <item>2 Levels: Test, Method</item>
        ///     <item>2 Levels: Class, Test</item>
        /// </list>
        /// </summary>
        protected internal AnalysisStrategy AnalysisStrategy
        {
            get
            {
                return _analysisStrategy;
            }
            set
            {
                _analysisStrategy = value;
                ReportStatusStats = new ReportStatusStats(value);
            }
        }

        /// <summary>
        /// Execution status
        /// </summary>
        protected internal Status ReportStatus { get; private set; } = Status.Pass;

        /// <summary>
        /// Instance of <see cref="AventStack.ExtentReports.ReportStatusStats"/>
        /// </summary>
        protected internal ReportStatusStats ReportStatusStats { get; private set; } = new ReportStatusStats();

        /// <summary>
        /// Report start time
        /// </summary>
        protected internal DateTime ReportStartDateTime { get; } = DateTime.Now;

        /// <summary>
        /// Report end time
        /// </summary>
        protected internal DateTime ReportEndDateTime { get; private set; } = DateTime.Now;

        /// <summary>
        /// A list of all <see cref="IExtentReporter"/> reporters started by the <code>attachReporter</code> method
        /// </summary>
        protected internal List<IExtentReporter> StarterReporterList { get; private set; } = new List<IExtentReporter>();

        protected TestAttributeTestContextProvider<Author> AuthorContext = new TestAttributeTestContextProvider<Author>();
        protected TestAttributeTestContextProvider<Category> CategoryContext = new TestAttributeTestContextProvider<Category>();
        protected TestAttributeTestContextProvider<Device> DeviceContext = new TestAttributeTestContextProvider<Device>();
        protected TestAttributeTestContextProvider<ExceptionInfo> ExceptionInfoContext = new TestAttributeTestContextProvider<ExceptionInfo>();
        protected SystemAttributeContext SystemAttributeContext = new SystemAttributeContext();

        /// <summary>
        /// Any logs added by to the test runner can be added to Extent
        /// </summary>
        protected internal List<string> TestRunnerLogs { get; private set; } = new List<string>();

        /// <summary>
        /// A list of all started tests
        /// </summary>
        private List<Test> _testList = new List<Test>();

        /// <summary>
        /// A unique list of test status
        /// 
        /// Consider a report having 5 tests:
        /// 
        /// <list type="bullet">
        ///     <item>Test 1: Pass</item>
        ///     <item>Test 2: Skip</item>
        ///     <item>Test 3: Pass</item>
        ///     <item>Test 4: Fail</item>
        ///     <item>Test 5: Fail</item>
        /// </list>
        /// 
        /// Distinct list of contained status:
        /// 
        /// <list type="bullet">
        ///     <item>Test 1: Pass</item>
        ///     <item>Test 2: Skip</item>
        ///     <item>Test 5: Fail</item>
        /// </list>
        /// </summary>
        private List<Status> _statusList = new List<Status>();

        /// <summary>
        /// Contains status as keys, which are translated over to <code>_statusList</code>
        /// </summary>
        private Dictionary<Status, bool> _statusMap = new Dictionary<Status, bool>();

        private readonly object _synclock = new object();
        private AnalysisStrategy _analysisStrategy = AnalysisStrategy.Test;

        protected internal ReportObservable() { }

        /// <summary>
        /// Subscribe the reporter to receive updates
        /// </summary>
        /// <param name="reporter"><see cref="IExtentReporter"/></param>
        protected void Register(IExtentReporter reporter)
        {
            lock (_synclock)
            {
                reporter.Start();
                StarterReporterList.Add(reporter);
            }
        }

        protected void Deregister(IExtentReporter reporter)
        {
            lock (_synclock)
            {
                reporter.Stop();
                StarterReporterList.Remove(reporter);
            }
        }

        internal void SaveTest(Test test)
        {
            lock (_synclock)
            {
                _testList.Add(test);
                StarterReporterList.ForEach(x => x.OnTestStarted(test));
            }
        }

        internal void RemoveTest(Test test)
        {
            lock (_synclock)
            {
                RemoveTestTestList(test);
                RemoveTestTestAttributeContext(test);
                StarterReporterList.ForEach(x => x.OnTestRemoved(test));
            }
        }

        private void RemoveTestTestList(Test test)
        {
            TestRemoveService.Remove(_testList, test);
            RefreshReportEntities();
        }

        private void RemoveTestTestAttributeContext(Test test)
        {
            if (test.HasAuthor)
            {
                AuthorContext.RemoveTest(test);
            }
            if (test.HasCategory)
            {
                CategoryContext.RemoveTest(test);
            }
            if (test.HasDevice)
            {
                DeviceContext.RemoveTest(test);
            }
            if (test.HasException)
            {
                ExceptionInfoContext.RemoveTest(test);
            }
        }

        private void RefreshReportEntities()
        {
            RefreshReportStats();
        }

        private void RefreshReportStats()
        {
            ReportStatusStats.Refresh(_testList);
            RefreshStatusList();
        }

        private void RefreshStatusList()
        {
            lock (_synclock)
            {
                _statusMap.Clear();
                _statusList.Clear();
                RefreshStatusList(_testList);
                _statusMap.ToList().ForEach(x => _statusList.Add(x.Key));
            }
        }

        private void RefreshStatusList(List<Test> testList)
        {
            lock (_synclock)
            {
                if (testList.IsNullOrEmpty())
                {
                    return;
                }

                var distinctStatusList = testList.Select(x => x.Status).Distinct();

                foreach (var s in distinctStatusList)
                {
                    if (!_statusMap.ContainsKey(s))
                    {
                        _statusMap.Add(s, false);
                    }
                }
                
                // recursively, do the same for child tests
                foreach (var test in testList.ToList())
                {
                    if (test.HasChildren)
                    {
                        RefreshStatusList(test.NodeContext.All().ToList());
                    }
                }
            }
        }

        internal void AddNode(Test node)
        {
            lock(_synclock)
            {
                StarterReporterList.ForEach(x => x.OnNodeStarted(node));
            }
        }

        internal void AddLog(Test test, Log log)
        {
            lock (_synclock)
            {
                CollectRunInfo();
                StarterReporterList.ForEach(x => x.OnLogAdded(test, log));
            }
        }

        private void CollectRunInfo()
        {
            lock (_synclock)
            {
                if (_testList.IsNullOrEmpty())
                    return;

                ReportEndDateTime = DateTime.Now;
                RefreshReportEntities();

                foreach (var test in _testList)
                {
                    test.End();
                    CopyContextInfoToObservable(test);
                }
            }
        }

        private void CopyContextInfoToObservable(Test test)
        {
            if (test.HasAuthor)
            {
                test.AuthorContext.All().ToList()
                    .ForEach(x => AuthorContext.AddAttributeContext((Author)x, test));
            }
            if (test.HasCategory)
            {
                test.CategoryContext.All().ToList()
                    .ForEach(x => CategoryContext.AddAttributeContext((Category)x, test));
            }
            if (test.HasDevice)
            {
                test.DeviceContext.All().ToList()
                    .ForEach(x => DeviceContext.AddAttributeContext((Device)x, test));
            }
            if (test.HasException)
            {
                test.ExceptionInfoContext.All().ToList()
                    .ForEach(x => ExceptionInfoContext.AddAttributeContext((ExceptionInfo)x, test));
            }
            if (test.HasChildren)
            {
                test.NodeContext.All().ToList()
                    .ForEach(x => CopyContextInfoToObservable(x));
            }
        }
        
        private void EndTest(Test test)
        {
            lock (_synclock)
            {
                test.End();
                UpdateReportStatus(test.Status);
            }
        }

        internal void AssignAuthor(Test test, Author author)
        {
            StarterReporterList.ForEach(x => x.OnAuthorAssigned(test, author));
        }

        internal void AssignCategory(Test test, Category category)
        {
            StarterReporterList.ForEach(x => x.OnCategoryAssigned(test, category));
        }

        internal void AssignDevice(Test test, Device device)
        {
            StarterReporterList.ForEach(x => x.OnDeviceAssigned(test, device));
        }

        internal void AddScreenCapture(Test test, ScreenCapture screenCapture)
        {
            StarterReporterList.ForEach(x => x.OnScreenCaptureAdded(test, screenCapture));
        }

        internal void AddScreenCapture(Log log, ScreenCapture screenCapture)
        {
            StarterReporterList.ForEach(x => x.OnScreenCaptureAdded(log, screenCapture));
        }

        private void UpdateReportStatus(Status status)
        {
            var statusIndex = StatusHierarchy.GetStatusHierarchy().IndexOf(status);
            var testStatusIndex = StatusHierarchy.GetStatusHierarchy().IndexOf(ReportStatus);
            ReportStatus = statusIndex < testStatusIndex ? status : ReportStatus;
        }

        protected void Flush()
        {
            lock (_synclock)
            {
                CollectRunInfo();
                NotifyReporters();
            }
        }

        private void NotifyReporters()
        {
            if (_testList.IsNullOrEmpty())
                return;

            if (!_testList.IsNullOrEmpty() && _testList[0].IsBehaviorDrivenType)
                AnalysisStrategy = AnalysisStrategy.BDD;

            lock (_synclock)
            {
                ReportStatusStats.Refresh(_testList);
            }

            var reportAggregates = new ReportAggregates
            {
                TestList = _testList,
                StatusList = _statusList,
                ReportStatusStats = this.ReportStatusStats,
                AuthorContext = this.AuthorContext,
                CategoryContext = this.CategoryContext,
                DeviceContext = this.DeviceContext,
                ExceptionInfoContext = this.ExceptionInfoContext,
                TestRunnerLogs = this.TestRunnerLogs,
                SystemAttributeContext = this.SystemAttributeContext
            };

            StarterReporterList.ForEach(x => {
                x.AnalysisStrategy = AnalysisStrategy;
                x.Flush(reportAggregates);
            });
        }

        protected void End()
        {
            Flush();
            StarterReporterList.ForEach(x => x.Stop());
            StarterReporterList.Clear();
        }

        protected void AddSystemInfo(string name, string value)
        {
            var sa = new SystemAttribute(name, value);
            AddSystemInfo(sa);
        }

        protected void AddSystemInfo(SystemAttribute sa)
        {
            SystemAttributeContext.AddSystemAttribute(sa);   
        }

        protected void AddTestRunnerLogs(List<string> logs)
        {
            TestRunnerLogs.AddRange(logs);
        }

        protected void AddTestRunnerLogs(string log)
        {
            TestRunnerLogs.Add(log);
        }
    }
}
