namespace AventStack.ExtentReports.Core
{
    public interface IReportService
    {
        void AttachReporter(params IExtentReporter[] reporter);
    }
}