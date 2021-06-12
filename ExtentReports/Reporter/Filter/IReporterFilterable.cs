namespace AventStack.ExtentReports.Reporter.Filter
{
    public interface IReporterFilterable<T> where T : AbstractReporter
    {
        EntityFilters<T> Filter { get; }
    }
}
