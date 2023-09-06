using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Listener.Entity;
using System;
using System.Reactive.Subjects;
using System.Collections.Concurrent;
using System.Linq;

namespace AventStack.ExtentReports.Core
{
    public abstract class ReactiveSubject
    {
        private readonly Subject<ReportEntity> _reportSubject = new Subject<ReportEntity>();
        private readonly Subject<TestEntity> _testSubject = new Subject<TestEntity>();

        protected Report Report { get; } = new Report();
        protected ConcurrentQueue<Test> Tests { get; } = new ConcurrentQueue<Test>();

        protected internal void Subscribe(IObserver<ReportEntity> obj)
        {
            _reportSubject.Subscribe(obj);
        }

        protected internal void Subscribe(IObserver<TestEntity> obj)
        {
            _testSubject.Subscribe(obj);
        }

        protected internal void Flush()
        {
            _reportSubject.OnNext(new ReportEntity { Report = Report });
            _testSubject.OnNext(new TestEntity { Tests = Tests.ToList() });
        }
    }
}
