using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Model;

using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports
{
    public class ReportStatusStats
    {
        public int ParentCount
        {
            get
            {
                return ParentCountPass
                    + ParentCountFail
                    + ParentCountFatal
                    + ParentCountError
                    + ParentCountWarning
                    + ParentCountSkip;
            }
            private set => ParentCount = value;
        }
        public int ParentCountPass { get; private set; }
        public int ParentCountFail { get; private set; }
        public int ParentCountFatal { get; private set; }
        public int ParentCountError { get; private set; }
        public int ParentCountWarning { get; private set; }
        public int ParentCountSkip { get; private set; }
        public int ParentCountExceptions { get; private set; }

        public double ParentPercentagePass
        {
            get
            {
                var i = ParentCount > 0 ? (double)ParentCountPass / (double)ParentCount : 0;
                return i * 100;
            }
        }
        public double ParentPercentageFail
        {
            get
            {
                var i = ParentCount > 0 ? (double)(ParentCountFail + ParentCountFatal) / (double)ParentCount : 0;
                return i * 100;
            }
        }
        public double ParentPercentageSkip
        {
            get
            {
                var i = ParentCount > 0 ? (double)ParentCountSkip / (double)ParentCount : 0;
                return i * 100;
            }
        }
        public double ParentPercentageWarning
        {
            get
            {
                var i = ParentCount > 0 ? (double)ParentCountWarning / (double)ParentCount : 0;
                return i * 100;
            }
        }
        public double ParentPercentageOthers
        {
            get
            {
                var i = ParentCount > 0 ? (double)(ParentCountWarning + ParentCountError) / (double)ParentCount : 0;
                return i * 100;
            }
        }

        public int ChildCount
        {
            get
            {
                return ChildCountPass
                    + ChildCountFail
                    + ChildCountFatal
                    + ChildCountError
                    + ChildCountWarning
                    + ChildCountSkip
                    + ChildCountInfo
                    + ChildCountDebug;
            }
            private set => ChildCount = value;
        }
        public int ChildCountPass { get; private set; }
        public int ChildCountFail { get; private set; }
        public int ChildCountFatal { get; private set; }
        public int ChildCountError { get; private set; }
        public int ChildCountWarning { get; private set; }
        public int ChildCountSkip { get; private set; }
        public int ChildCountInfo { get; private set; }
        public int ChildCountDebug { get; private set; }
        public int ChildCountExceptions { get; private set; }

        public double ChildPercentagePass
        {
            get
            {
                var i = ChildCount > 0 ? (double)ChildCountPass / (double)ChildCount : 0;
                return i * 100;
            }
        }
        public double ChildPercentageFail
        {
            get
            {
                var i = ChildCount > 0 ? (double)(ChildCountFail + ChildCountFatal) / (double)ChildCount : 0;
                return i * 100;
            }
        }
        public double ChildPercentageSkip
        {
            get
            {
                var i = ChildCount > 0 ? (double)ChildCountSkip / (double)ChildCount : 0;
                return i * 100;
            }
        }
        public double ChildPercentageWarning
        {
            get
            {
                var i = ChildCount > 0 ? (double)ChildCountWarning / (double)ChildCount : 0;
                return i * 100;
            }
        }
        public double ChildPercentageOthers
        {
            get
            {
                var i = ChildCount > 0 ? (double)(ChildCountWarning + ChildCountError) / (double)ChildCount : 0;
                return i * 100;
            }
        }

        public int GrandChildCount
        {
            get
            {
                return GrandChildCountPass
                    + GrandChildCountFail
                    + GrandChildCountFatal
                    + GrandChildCountError
                    + GrandChildCountWarning
                    + GrandChildCountSkip
                    + GrandChildCountInfo
                    + GrandChildCountDebug;
            }
            private set => GrandChildCount = value;
        }
        public int GrandChildCountPass { get; private set; }
        public int GrandChildCountFail { get; private set; }
        public int GrandChildCountFatal { get; private set; }
        public int GrandChildCountError { get; private set; }
        public int GrandChildCountWarning { get; private set; }
        public int GrandChildCountSkip { get; private set; }
        public int GrandChildCountInfo { get; private set; }
        public int GrandChildCountDebug { get; private set; }
        public int GrandChildCountExceptions { get; private set; }

        public double GrandChildPercentagePass
        {
            get
            {
                var i = GrandChildCount > 0 ? (double)GrandChildCountPass / (double)GrandChildCount : 0;
                return i * 100;
            }
        }
        public double GrandChildPercentageFail
        {
            get
            {
                var i = GrandChildCount > 0 ? (double)(GrandChildCountFail + GrandChildCountFatal) / (double)GrandChildCount : 0;
                return i * 100;
            }
        }
        public double GrandChildPercentageSkip
        {
            get
            {
                var i = GrandChildCount > 0 ? (double)GrandChildCountSkip / (double)GrandChildCount : 0;
                return i * 100;
            }
        }
        public double GrandChildPercentageWarning
        {
            get
            {
                var i = GrandChildCount > 0 ? (double)GrandChildCountWarning / (double)GrandChildCount : 0;
                return i * 100;
            }
        }
        public double GrandChildPercentageOthers
        {
            get
            {
                var i = GrandChildCount > 0 ? (double)(GrandChildCountWarning + GrandChildCountError) / (double)GrandChildCount : 0;
                return i * 100;
            }
        }

        private readonly AnalysisStrategy _strategy = AnalysisStrategy.Test;
        private List<Test> _testList;
        private readonly object _synclock = new object();

        public ReportStatusStats()
        {
        }

        public ReportStatusStats(AnalysisStrategy strategy) : this()
        {
            _strategy = strategy;
        }

        public void Refresh(List<Test> testList)
        {
            Reset();
            _testList = testList;
            RefreshStats();
        }

        private void Reset()
        {
            ParentCountPass = 0;
            ParentCountFail = 0;
            ParentCountFatal = 0;
            ParentCountError = 0;
            ParentCountWarning = 0;
            ParentCountSkip = 0;
            ParentCountExceptions = 0;

            ChildCountPass = 0;
            ChildCountFail = 0;
            ChildCountFatal = 0;
            ChildCountError = 0;
            ChildCountWarning = 0;
            ChildCountSkip = 0;
            ChildCountInfo = 0;
            ChildCountDebug = 0;
            ChildCountExceptions = 0;

            GrandChildCountPass = 0;
            GrandChildCountFail = 0;
            GrandChildCountFatal = 0;
            GrandChildCountError = 0;
            GrandChildCountWarning = 0;
            GrandChildCountSkip = 0;
            GrandChildCountInfo = 0;
            GrandChildCountExceptions = 0;
        }

        private void RefreshStats()
        {
            lock (_synclock)
            {
                _testList.ForEach(x => AddTestForStatusStatsUpdate(x));
            }
        }

        private void AddTestForStatusStatsUpdate(Test test)
        {
            if (_strategy == AnalysisStrategy.BDD || test.IsBehaviorDrivenType || (test.HasChildren && test.NodeContext.FirstOrDefault().IsBehaviorDrivenType))
            {
                UpdateGroupCountsBDD(test);
                return;
            }

            if (_strategy == AnalysisStrategy.Test || _strategy == AnalysisStrategy.Class)
            {
                UpdateGroupCountsTestStrategy(test);
                return;
            }

            throw new InvalidAnalysisStrategyException("No such strategy found: " + _strategy);
        }

        private void UpdateGroupCountsBDD(Test test)
        {
            IncrementItemCountByStatus(ItemLevel.Parent, test.Status);

            if (test.HasChildren)
            {
                foreach (var child in test.NodeContext.All())
                { 
                    if (child.BehaviorDrivenType == null)
                    {
                        throw new InvalidAnalysisStrategyException(child.Name + " is not a valid BDD test. " +
                            "This happens when using AnalysisStrategy.BDD without BDD-style tests or if the test was not marked " +
                            "with the appropriate BDD type.");
                    }

                    if (child.BehaviorDrivenType.GetType() == typeof(Scenario))
                    {
                        IncrementItemCountByStatus(ItemLevel.Child, child.Status);
                    }

                    if (child.HasChildren)
                    {
                        foreach (var grandchild in child.NodeContext.All())
                        {
                            if (grandchild.BehaviorDrivenType.GetType() == typeof(Scenario))
                            {
                                IncrementItemCountByStatus(ItemLevel.Child, grandchild.Status);
                                grandchild.NodeContext.All().ForEach(x => IncrementItemCountByStatus(ItemLevel.GrandChild, x.Status));
                            }
                            else
                            {
                                IncrementItemCountByStatus(ItemLevel.GrandChild, grandchild.Status);
                            }
                        }
                    }
                    else
                    {
                        IncrementItemCountByStatus(ItemLevel.GrandChild, child.Status);
                    }
                }
            }
        }

        private void UpdateGroupCountsTestStrategy(Test test)
        {
            IncrementItemCountByStatus(ItemLevel.Parent, test.Status);

            if (test.HasChildren)
            {
                UpdateGroupCountsTestStrategyChildren(test);
            }
        }

        private void UpdateGroupCountsTestStrategyChildren(Test test)
        {
            if (test.HasChildren)
            {
                foreach (var node in test.NodeContext.All().ToList())
                {
                    if (node.HasChildren)
                    {
                        UpdateGroupCountsTestStrategyChildren(node);
                    }
                    else
                    {
                        IncrementItemCountByStatus(ItemLevel.Child, node.Status);
                    }
                }
            }
        }

        enum ItemLevel
        {
            Parent,
            Child,
            GrandChild
        }

        private void IncrementItemCountByStatus(ItemLevel item, Status status)
        {
            switch (item)
            {
                case ItemLevel.Parent:
                    IncrementParent(status);
                    break;
                case ItemLevel.Child:
                    IncrementChild(status);
                    break;
                case ItemLevel.GrandChild:
                    IncrementGrandChild(status);
                    break;
                default:
                    break;
            }
        }

        private void IncrementParent(Status status)
        {
            switch (status)
            {
                case Status.Pass:
                    ParentCountPass++;
                    return;
                case Status.Fail:
                    ParentCountFail++;
                    break;
                case Status.Fatal:
                    ParentCountFatal++;
                    break;
                case Status.Error:
                    ParentCountError++;
                    break;
                case Status.Warning:
                    ParentCountWarning++;
                    break;
                case Status.Skip:
                    ParentCountSkip++;
                    break;
                default:
                    break;
            }

            ParentCountExceptions++;
        }

        private void IncrementChild(Status status)
        {
            switch (status)
            {
                case Status.Pass:
                    ChildCountPass++;
                    break;
                case Status.Fail:
                    ChildCountFail++;
                    break;
                case Status.Fatal:
                    ChildCountFatal++;
                    break;
                case Status.Error:
                    ChildCountError++;
                    break;
                case Status.Warning:
                    ChildCountWarning++;
                    break;
                case Status.Skip:
                    ChildCountSkip++;
                    break;
                case Status.Info:
                    ChildCountInfo++;
                    break;
                case Status.Debug:
                    ChildCountDebug++;
                    break;
                default:
                    break;
            }

            if (status != Status.Pass && status != Status.Info)
                ChildCountExceptions++;
        }

        private void IncrementGrandChild(Status status)
        {
            switch (status)
            {
                case Status.Pass:
                    GrandChildCountPass++;
                    break;
                case Status.Fail:
                    GrandChildCountFail++;
                    break;
                case Status.Fatal:
                    GrandChildCountFatal++;
                    break;
                case Status.Error:
                    GrandChildCountError++;
                    break;
                case Status.Warning:
                    GrandChildCountWarning++;
                    break;
                case Status.Skip:
                    GrandChildCountSkip++;
                    break;
                case Status.Info:
                    GrandChildCountInfo++;
                    break;
                case Status.Debug:
                    GrandChildCountDebug++;
                    break;
                default:
                    break;
            }

            if (status != Status.Pass && status != Status.Info)
                GrandChildCountExceptions++;
        }
    }
}
