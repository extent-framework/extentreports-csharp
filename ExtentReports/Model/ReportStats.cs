using AventStack.ExtentReports.Util;
using AventStack.ExtentReports.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports.Model
{
    public class ReportStats
    {
        public AnalysisStrategy AnalysisStrategy { get; set; } = AnalysisStrategy.Test;
        public ConcurrentDictionary<Status, int> Parent = new ConcurrentDictionary<Status, int>();
        public ConcurrentDictionary<Status, int> Child = new ConcurrentDictionary<Status, int>();
        public ConcurrentDictionary<Status, int> Grandchild = new ConcurrentDictionary<Status, int>();
        public ConcurrentDictionary<Status, int> Log = new ConcurrentDictionary<Status, int>();
        public ConcurrentDictionary<Status, double> ParentPercentage = new ConcurrentDictionary<Status, double>();
        public ConcurrentDictionary<Status, double> ChildPercentage = new ConcurrentDictionary<Status, double>();
        public ConcurrentDictionary<Status, double> GrandchildPercentage = new ConcurrentDictionary<Status, double>();
        public ConcurrentDictionary<Status, double> LogPercentage = new ConcurrentDictionary<Status, double>();

        private readonly static List<Status> _status = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

        public void Update(IEnumerable<Test> tests)
        {
            Reset();

            if (tests == null || tests.Count() == 0)
            {
                return;
            }

            // Level 0
            Update(tests, ref Parent, ref ParentPercentage);

            // level 1, for BDD, this would also include Scenario and excludes ScenarioOutline
            var children = tests.SelectMany(x => x.Children)
                .Where(x => x.BddType == null || (x.BddType != null && !x.BddType.Name.ToUpper().Equals("SCENARIOOUTLINE")));
            var scenarios = tests.SelectMany(x => x.Children)
                .SelectMany(x => x.Children)
                .Where(x => x.BddType != null && x.BddType.Name.ToUpper().Equals("SCENARIO"));
            children.Concat(scenarios);
            Update(children, ref Child, ref ChildPercentage);

            // level 2, for BDD, this only includes Steps
            var grandchildren = children.SelectMany(x => x.Children)
                .Where(x => x.BddType == null || (x.BddType != null && !x.BddType.Name.ToUpper().Equals("SCENARIO")));
            Update(grandchildren, ref Grandchild, ref GrandchildPercentage);

            // events
            var logs = tests.SelectMany(x => x.Logs);
            logs = logs.Concat(children.SelectMany(x => x.Logs));
            logs = logs.Concat(grandchildren.SelectMany(x => x.Logs));
            Update(logs, ref Log, ref LogPercentage);
        }

        private void Update<T>(IEnumerable<IRunResult<T>> list, ref ConcurrentDictionary<Status, int> countDict, ref ConcurrentDictionary<Status, double> percentDict)
        {
            Assert.NotNull(list, "Collection must not be null");

            countDict.AddRange(_status.ToDictionary(x => x, x => 0));
            percentDict.AddRange(_status.ToDictionary(x => x, x => 0.0));

            if (list.Count() > 0)
            {
                var count = list.Select(x => x.Status).GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
                countDict.AddRange(count);

                var percent = countDict.ToDictionary(x => x.Key, x => (double)(x.Value / list.Count()));
                percentDict.AddRange(percent);
            }
        }

        public void Reset()
        {
            var list = new List<IRunResult<Test>>();
            Update(list, ref Parent, ref ParentPercentage);
            Update(list, ref Child, ref ChildPercentage);
            Update(list, ref Grandchild, ref GrandchildPercentage);
            Update(list, ref Log, ref LogPercentage);
        }

        public int SumStat(IDictionary<Status, int> dict)
        {
            return dict.Values.Sum();
        }
    }
}
