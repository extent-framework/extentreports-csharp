using AventStack.ExtentReports.Gherkin.Model;

using MongoDB.Bson;

using System;
using System.Linq;
using System.Threading;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class Test
    {
        public string Name { get; set; }
        public string Description { get; set; }
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

        public string HierarchicalName {
            get
            {
               if (Parent != null)
                    return Parent.Name + "." + Name;
                return Name;
            }
        }

        private static int _cntr;
        private GenericStructure<Log> _logContext;
        private GenericStructure<Test> _nodeContext;
        private GenericStructure<TestAttribute> _authorContext;
        private GenericStructure<TestAttribute> _categoryContext;
        private GenericStructure<TestAttribute> _deviceContext;
        private GenericStructure<TestAttribute> _exceptionInfoContext;
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

        public GenericStructure<TestAttribute> CategoryContext
        {
            get
            {
                if (_categoryContext == null)
                    _categoryContext = new GenericStructure<TestAttribute>();
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

        public GenericStructure<TestAttribute> ExceptionInfoContext
        {
            get
            {
                if (_exceptionInfoContext == null)
                    _exceptionInfoContext = new GenericStructure<TestAttribute>();
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

        public GenericStructure<TestAttribute> AuthorContext
        {
            get
            {
                if (_authorContext == null)
                    _authorContext = new GenericStructure<TestAttribute>();
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

        public GenericStructure<TestAttribute> DeviceContext
        {
            get
            {
                if (_deviceContext == null)
                    _deviceContext = new GenericStructure<TestAttribute>();
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
            lock (_syncLock)
            {
                UpdateTestStatusRecursive(this);
                EndChildTestsRecursive(this);
                Status = (Status == Status.Info || Status == Status.Debug) ? Status.Pass : Status;
                SetEndTimeFromChildren();
            }
        }

        private void SetEndTimeFromChildren()
        {
            if (HasChildren)
            {
                EndTime = NodeContext.LastOrDefault().EndTime;
            }
            else if (HasLog)
            {
                EndTime = LogContext.LastOrDefault().Timestamp;
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
                    Status = Status.Pass;
                    test.NodeContext.All()
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
                lock (_syncLock)
                {
                    test.NodeContext.All().ToList().ForEach(x => x.End());
                }
            }
        }
    }
}
