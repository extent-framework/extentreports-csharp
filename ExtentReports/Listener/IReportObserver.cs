using AventStack.ExtentReports.Listener.Entity;

namespace AventStack.ExtentReports.Listener
{
    public interface IReportObserver<T> : IExtentObserver<T> where T : ReportEntity
    {
        
    }
}
