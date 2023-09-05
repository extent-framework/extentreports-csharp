using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Listener.Entity;
using System;
using System.Reactive.Subjects;

namespace AventStack.ExtentReports.Core
{
    public abstract class ReactiveSubject
    {
        private readonly Subject<ReportEntity> _reportSubject = new Subject<ReportEntity>();
        protected Report Report { get; } = new Report();

        protected internal void Subscribe(IObserver<ReportEntity> obj)
        {
            _reportSubject.Subscribe(obj);
        }

        protected internal void Flush()
        {
            _reportSubject.OnNext(new ReportEntity { Report = Report });
        }
    }
}
