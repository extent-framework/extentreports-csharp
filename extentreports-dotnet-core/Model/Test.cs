using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Gherkin.Model;

using MongoDB.Bson;

using System;
using System.Linq;
using System.Threading;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class Test : BasicMongoReportElement, IRunResult
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public int TestId { get; } = Interlocked.Increment(ref _cntr);
        public ObjectId ObjectId { get; set; }
        public Status Status { get; set; } = Status.Pass;
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now;
        public IGherkinFormatterModel BehaviorDrivenType { get; set; }
        public ExtentReports Extent { get; set; }
        public Test Parent { get; set; }

        public string BehaviorDrivenTypeName
        {
            get
            {
                return IsBehaviorDrivenType ? BehaviorDrivenType.ToString() : null;
            }
        }

        public int Level
        {
            get
            {
                return Parent == null ? 0 : Parent.Level + 1;
            }
        }

        public string HierarchicalName
        {
            get
            {
                var name = Name;
                var parent = Parent;
                while (parent != null)
                {
                    name = parent.Name + "." + name;
                    parent = parent.Parent;
                }
                return name;
            }
        }

        public Test Root
        {
            get
            {
                var root = this;
                while (root.Parent != null)
                {
                    root = root.Parent;
                }
                return root;
            }
        }

        private static int _cntr;
        private GenericStructure<Log> _logContext;
        private GenericStructure<Test> _nodeContext;
        private GenericStructure<Attribute> _authorContext;
        private GenericStructure<Attribute> _categoryContext;
        private GenericStructure<Attribute> _deviceContext;
        private GenericStructure<Attribute> _exceptionInfoContext;
        private GenericStructure<ScreenCapture> _screenCaptureContext;

        private readonly object _syncLock = new object();

        public bool IsChild
        {
            get
            {
                return Level > 0;
            }
        }

        public TimeSpan RunDuration
        {
            get
            {
                return EndTime.Subtract(StartTime);
            }
        }
        
        public GenericStructure<Test> NodeContext
        {
            get
            {
                if (_nodeContext == null)
                    _nodeContext = new GenericStructure<Test>();
                return _nodeContext;
            }
        }

        public bool HasChildren
        {
            get
            {
                return NodeContext != null && NodeContext.All() != null && NodeContext.Count > 0;
            }
        }

        public GenericStructure<Log> LogContext
        {
            get
            {
                if (_logContext == null)
                    _logContext = new GenericStructure<Log>();
                return _logContext;
            }
        }

        public bool HasLog
        {
            get
            {
                return LogContext != null && LogContext.All() != null && LogContext.Count > 0;
            }
        }

        public bool HasAttributes
        {
            get
            {
                return HasAuthor || HasCategory || HasDevice;
            }
        }

        public GenericStructure<Attribute> CategoryContext
        {
            get
            {
                if (_categoryContext == null)
                    _categoryContext = new GenericStructure<Attribute>();
                return _categoryContext;
            }
        }

        public bool HasCategory
        {
            get
            {
                return CategoryContext != null && CategoryContext.Count > 0;
            }
        }

        public GenericStructure<Attribute> ExceptionInfoContext
        {
            get
            {
                if (_exceptionInfoContext == null)
                    _exceptionInfoContext = new GenericStructure<Attribute>();
                return _exceptionInfoContext;
            }
        }

        public bool HasException
        {
            get
            {
                return ExceptionInfoContext != null && ExceptionInfoContext.Count > 0;
            }
        }

        public GenericStructure<Attribute> AuthorContext
        {
            get
            {
                if (_authorContext == null)
                    _authorContext = new GenericStructure<Attribute>();
                return _authorContext;
            }
        }

        public bool HasAuthor
        {
            get
            {
                return AuthorContext != null && AuthorContext.Count > 0;
            }
        }

        public GenericStructure<Attribute> DeviceContext
        {
            get
            {
                if (_deviceContext == null)
                    _deviceContext = new GenericStructure<Attribute>();
                return _deviceContext;
            }
        }

        public bool HasDevice
        {
            get
            {
                return DeviceContext != null && DeviceContext.Count > 0;
            }
        }

        public GenericStructure<ScreenCapture> ScreenCaptureContext
        {
            get
            {
                if (_screenCaptureContext == null)
                    _screenCaptureContext = new GenericStructure<ScreenCapture>();
                return _screenCaptureContext;
            }
        }

        public bool HasScreenCapture
        {
            get
            {
                return _screenCaptureContext != null && _screenCaptureContext.Count > 0;
            }
        }

        public bool IsBehaviorDrivenType
        {
            get
            {
                return BehaviorDrivenType != null;
            }
        }

        public void End()
        {
            UpdateTestStatusRecursive(this);
            EndChildTestsRecursive(this);
            lock (_syncLock)
            {
                Status = (Status == Status.Info || Status == Status.Debug) ? Status.Pass : Status;
            }
            SetEndTimeFromChildren();
        }

        private void SetEndTimeFromChildren()
        {
            if (HasChildren)
            {
                lock (_syncLock)
                {
                    EndTime = NodeContext.LastOrDefault().EndTime;
                }
            }
            else if (HasLog)
            {
                lock (_syncLock)
                {
                    EndTime = LogContext.LastOrDefault().Timestamp;
                }
            }
        }

        private void UpdateTestStatusRecursive(Test test)
        {
            if (test.HasLog)
            {
                test.LogContext.All().ToList().ForEach(x => UpdateStatus(x.Status));
            }

            if (test.HasChildren)
            {
                test.NodeContext.All().ToList().ForEach(x => UpdateTestStatusRecursive(x));
            }

            if (!test.IsBehaviorDrivenType)
            {
                bool hasNodeNotSkipped = test.NodeContext.All().Any(x => x.Status != Status.Skip);
                if (this.Status == Status.Skip && hasNodeNotSkipped)
                {
                    lock (_syncLock)
                    {
                        Status = Status.Pass;
                    }
                    test.NodeContext.All().ToList()
                        .FindAll(x => x.Status != Status.Skip)
                        .ForEach(x => UpdateTestStatusRecursive(x));
                }
            }
        }

        private void UpdateStatus(Status status)
        {
            var statusIndex = StatusHierarchy.GetStatusHierarchy().IndexOf(status);
            var testStatusIndex = StatusHierarchy.GetStatusHierarchy().IndexOf(Status);
            lock (_syncLock)
            {
                Status = statusIndex < testStatusIndex ? status : Status;
            }
        }
        
        private void EndChildTestsRecursive(Test test)
        {
            if (test.HasChildren)
            {
                test.NodeContext.All().ToList().ForEach(x => x.End());
            }
        }
    }
}
