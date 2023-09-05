using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AventStack.ExtentReports.Collections;
using AventStack.ExtentReports.Model.Context.Manager;

namespace AventStack.ExtentReports.Model
{
    public class Report : IBaseEntity, IMetaDataStorable
    {
        public AnalysisStrategy AnalysisStrategy = AnalysisStrategy.Test;
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public ReportStats Stats { get; } = new ReportStats();
        public IList<Test> Tests { get; private set; } = new SynchronizedList<Test>();
        public NamedAttributeContextManager<Author> AuthorCtx { get; } = new NamedAttributeContextManager<Author>();
        public NamedAttributeContextManager<Category> CategoryCtx { get; } = new NamedAttributeContextManager<Category>();
        public NamedAttributeContextManager<Device> DeviceCtx { get; } = new NamedAttributeContextManager<Device>();
        public NamedAttributeContextManager<ExceptionInfo> ExceptionInfoCtx { get; } = new NamedAttributeContextManager<ExceptionInfo>();
        public ConcurrentQueue<string> Logs { get; } = new ConcurrentQueue<string>();
        public List<SystemEnvInfo> SystemEnvInfo { get; } = new List<SystemEnvInfo>();
        public IDictionary<string, object> Info { get; set; } = new Dictionary<string, object>();

        private readonly object _synclock = new object();

        public void Refresh()
        {
            AuthorCtx.RefreshAll();
            CategoryCtx.RefreshAll();
            DeviceCtx.RefreshAll();
            ExceptionInfoCtx.RefreshAll();
            Stats.Update(Tests);

            lock (_synclock)
            {
                EndTime = DateTime.Now;
            }
        }

        public Status Status
        {
            get
            {
                if (Tests.Count == 0)
                {
                    return Status.Pass;
                }

                var allStatus = Tests.Select(x => x.Status).Distinct();
                var s = allStatus.Max();
                return s == Status.Skip ? Status.Pass : s;
            }
        }

        public bool IsBDD => Tests.Count != 0 && Tests.Any(x => x.IsBdd);

        public TimeSpan TimeTaken => EndTime.Subtract(StartTime);

        public bool HasAuthors => AuthorCtx.HasItems;

        public bool HasCategories => CategoryCtx.HasItems;

        public bool HasDevices => DeviceCtx.HasItems;

        public bool HasExceptions => ExceptionInfoCtx.HasItems;

        public bool HasTests => Tests.Count > 0;

        public void AddTest(Test test)
        {
            Tests.Add(test);
        }

        public void RemoveTest(IList<Test> tests, Test test, bool deep = true)
        {
            var item = tests.SingleOrDefault(x => x.Id == test.Id);

            if (item == null && deep)
            {
                foreach (Test t in tests)
                {
                    RemoveTest(t.Children, test);
                }
            }

            tests.Remove(test);
        }

        public void RemoveTest(Test test)
        {
            RemoveTest(Tests, test, true);
        }

        public void RemoveTest(int id)
        {
            var test = FindTest(id);
            RemoveTest(test);
        }

        public void ApplyOverrideConf()
        {
            var list = Tests.ToList();
            var min = list.Select(x => x.StartTime).Min();
            var max = list.Select(x => x.EndTime).Max();

            lock (_synclock)
            {
                StartTime = min;
                EndTime = max;
            }
        }

        public bool AnyTestHasStatus(Status status)
        {
            return Tests.Any(x => x.Status == status);
        }

        public Test FindTest(IList<Test> list, string name)
        {
            var test = list.SingleOrDefault(x => x.Name.Equals(name));

            if (test == null)
            {
                foreach (Test t in list)
                {
                    return FindTest(t.Children, name);
                }
            }

            return test;
        }

        public Test FindTest(string name)
        {
            return FindTest(Tests, name);
        }

        public Test FindTest(IList<Test> list, int id)
        {
            var test = list.SingleOrDefault(x => x.Id.Equals(id));

            if (test == null)
            {
                foreach (Test t in list)
                {
                    return FindTest(t.Children, id);
                }
            }

            return test;
        }

        public Test FindTest(int id)
        {
            return FindTest(Tests, id);
        }
    }
}
