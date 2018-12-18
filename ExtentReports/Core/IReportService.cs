namespace AventStack.ExtentReports
{
    public interface IReportService
    {
        void AttachReporter(params IExtentReporter[] reporter);
    }
}