using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Model;

namespace AventStack.ExtentReports.Reporter
{
    public abstract class AbstractReporter : IExtentReporter
    {
        public abstract string ReporterName { get; }

        public abstract AnalysisStrategy AnalysisStrategy { get; set; }

        public abstract ReportStatusStats ReportStatusStats { get; protected internal set; }

        public abstract void Flush(ReportAggregates reportAggregates);

        public abstract void OnAuthorAssigned(Test test, Author author);

        public abstract void OnCategoryAssigned(Test test, Category category);

        public abstract void OnDeviceAssigned(Test test, Device device);

        public abstract void OnLogAdded(Test test, Log log);

        public abstract void OnNodeStarted(Test node);

        public abstract void OnScreenCaptureAdded(Test test, ScreenCapture screenCapture);

        public abstract void OnScreenCaptureAdded(Log log, ScreenCapture screenCapture);

        public abstract void OnTestRemoved(Test test);

        public abstract void OnTestStarted(Test test);

        public abstract void Start();

        public abstract void Stop();
    }
}
