using AventStack.ExtentReports.Collections;
using AventStack.ExtentReports.Extensions;
using AventStack.ExtentReports.Gherkin;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AventStack.ExtentReports.Model
{
    public class Test : IMetaDataStorable, IRunResult<Test>, IBaseEntity
    {
        private const char Separator = '.';

        private readonly StatusDeterminator _determinator = new StatusDeterminator();
        private static int _cntr;
        private readonly object _synclock = new object();

        public Test() { }

        public Test(string name, string description = null, GherkinKeyword bddType = null)
        {
            Name = name;
            Description = description;
            BddType = bddType;
        }

        public Guid ReportUuid { get; set; }
        public int Id = Interlocked.Increment(ref _cntr);
        public bool UseNaturalConf { get; set; } = true;
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Pass;
        public int Level { get; set; } = 0;
        public bool Leaf { get; set; } = true;
        public string Name { get; set; }
        public string Description { get; set; }
        public GherkinKeyword BddType { get; set; }
        public IList<Test> Children { get; set; } = new SynchronizedList<Test>();
        public ConcurrentQueue<Log> Logs { get; set; } = new ConcurrentQueue<Log>();
        public ConcurrentQueue<Log> GeneratedLog { get; set; } = new ConcurrentQueue<Log>();
        public List<ScreenCapture> Media { get; set; } = new List<ScreenCapture>();
        public List<ExceptionInfo> ExceptionInfo { get; set; } = new List<ExceptionInfo>();
        public ISet<Author> Author { get; set; } = new HashSet<Author>();
        public ISet<Category> Category { get; set; } = new HashSet<Category>();
        public ISet<Device> Device { get; set; } = new HashSet<Device>();
        public IDictionary<string, object> Info { get; set; } = new Dictionary<string, object>();

        [JsonIgnore]
        public Test Parent { get; set; }

        public string FullName
        {
            get
            {
                var test = this;
                var sb = new StringBuilder(test.Name);
                while (test.Parent != null)
                {
                    test = test.Parent;
                    if (!test.IsBdd || test.BddType.GetType() == typeof(ScenarioOutline))
                    {
                        sb.Insert(0, test.Name + Separator);
                    }
                }
                return sb.ToString();
            }
        }

        [JsonIgnore]
        public Test Ancestor
        {
            get
            {
                var test = this;
                while (test.Parent != null)
                {
                    test = test.Parent;
                }

                return test;
            }
        }

        public double TimeTaken => EndTime.Subtract(StartTime).TotalMilliseconds;

        public bool IsBdd => BddType != null;

        public void AddChild(Test test)
        {
            Assert.NotNull(test, "The assigned node must not be null");

            test.Level = Level + 1;
            test.Parent = this;
            test.Leaf = true;
            Leaf = false;
            if (!test.IsBdd || test.BddType.GetType() == typeof(Scenario))
            {
                test.Author.UnionWith(Author);
                test.Category.UnionWith(Category);
                test.Device.UnionWith(Device);
            }
            End(test.Status);
            Children.Add(test);
        }

        private void End(Status s)
        {
            Status = Status.Max(s);

            if (UseNaturalConf)
            {
                EndTime = DateTime.Now;
            }

            Propagate();
        }

        private void Propagate()
        {
            if (Parent != null)
            {
                Parent.End(Status);
            }
        }

        public void AddLog(Log log)
        {
            if (log != null)
            {
                Logs.Enqueue(log);
                AddLogCommon(log);
            }
        }

        public void AddGeneratedLog(Log log)
        {
            if (log != null)
            {
                GeneratedLog.Enqueue(log);
                AddLogCommon(log);
            }
        }

        private void AddLogCommon(Log log)
        {
            log.Seq = Logs.Count + GeneratedLog.Count;
            End(log.Status);
            //UpdateResult();
        }

        public void AddMedia(ScreenCapture m)
        {
            if (m != null && (m.Path != null || m.ResolvedPath != null || ((ScreenCapture)m).Base64 != null))
            {
                Media.Add(m);
            }
        }

        public bool HasAnyLog => !Logs.IsEmpty || !GeneratedLog.IsEmpty;

        public bool HasGeneratedLog => !GeneratedLog.IsEmpty;

        public bool HasLog => !Logs.IsEmpty;

        public bool HasChildren => Children.Count != 0;

        public bool HasAttributes => HasCategory || HasDevice || HasAuthor;

        public bool HasAuthor => Author.Count > 0;

        public bool HasCategory => Category.Count > 0;

        public bool HasDevice => Device.Count > 0;

        public bool HasScreenCapture => Media.Count > 0;

        public bool HasScreenCaptureDeep => HasScreenCapture || HasScreenCaptureDeepImpl(this);

        private bool HasScreenCaptureDeepImpl(Test test)
        {
            if (test.HasChildren)
            {
                var nodes = test.Children.ToList();

                foreach (Test node in nodes)
                {
                    if (node.HasScreenCapture)
                    {
                        return true;
                    }

                    return HasScreenCaptureDeepImpl(node);
                }
            }

            return false;
        }

        public void UpdateResult()
        {
            lock (_synclock)
            {
                _determinator.ComputeStatus(this);
            }
        }

        private class StatusDeterminator
        {
            public void ComputeStatus(Test test)
            {
                var list = FindLeafNodes(test);
                ComputeStatus(list);
            }

            private void ComputeStatus(List<Test> list)
            {
                list.ForEach(ComputeStatusSingle);
            }

            private void ComputeStatusSingle(Test test)
            {
                if (test.Parent != null)
                {
                    test.Parent.Status = test.Status.Max(test.Parent.Status);
                    ComputeStatusSingle(test.Parent);
                }
            }

            private List<Test> FindLeafNodes(Test test)
            {
                var list = new List<Test>();
                if (test.Leaf)
                {
                    list.Add(test);
                }
                else
                {
                    foreach (var t in test.Children)
                    {
                        if (t.Leaf)
                        {
                            list.Add(t);
                        }
                        else
                        {
                            list.AddRange(FindLeafNodes(t));
                        }
                    }
                }
                return list;
            }
        }
    }
}
